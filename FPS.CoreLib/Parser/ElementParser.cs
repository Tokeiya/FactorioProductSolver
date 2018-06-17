using System.Collections.Generic;
using FPS.CoreLib.Entity;
using Parseq;
using Parseq.Combinators;

namespace FPS.CoreLib.Parser
{
	public static class ElementParser
	{
		public static Parser<char, TableElement> TableElementParser { get; }
		public static Parser<char, TableElement> RecipeParser { get; }

		private static readonly Parser<char, Unit> WhiteSpace;

		static ElementParser()
		{
			WhiteSpace = BuildWhiteSpace();
			TableElementParser = BuildTableElement();
			RecipeParser = BuildRecipe();
		}

		private static Parser<char, Unit> BuildWhiteSpace()
		{
			var newLine = Combinator.Choice(
				Chars.Sequence("\r\n").Ignore(),
				Chars.Char('\r').Ignore(),
				Chars.Char('\n').Ignore()
			);

			var comment =
				from _ in Chars.Sequence("--")
				from __ in Chars.NoneOf('\r', '\n').Many0()
				from ___ in newLine
				select Unit.Instance;

			var whiteSpace =
				from _ in Chars.WhiteSpace()
				select Unit.Instance;

			return whiteSpace.Or(comment).Many0().Ignore();
		}

		private static Parser<char, TableElement> BuildTableElement()
		{
			var namedValueElement =
				from id in Literals.Identifier
				from __ in Sandwich('=')
				from value in Literals.Literal
				select new ValueElement(id.Value, Value.Create(value));

			var anonymousValueElement =
				from value in Literals.Literal
				select new ValueElement("", Value.Create(value));

			Parser<char, Element> valueElementParser = namedValueElement.Or(anonymousValueElement);


			var tableElementParser = new FixedPoint<char, TableElement>("TableElement");


			var contentParser = valueElementParser.Or(tableElementParser.Parse);

			var followingParser =
				(from _ in Sandwich(',')
					from ctnt in contentParser
					select ctnt).Many0();


			var tableContentsParser =
				from first in contentParser
				from following in followingParser
				from _ in Chars.Char(',').Optional()
				select Merge(first, following);

			var anonymousTableElement =
				from _ in Sandwich('{')
				from contents in tableContentsParser
				from __ in Sandwich('}')
				select contents;


			var namedTableElementParser =
				from ident in Literals.Identifier
				from _ in Sandwich('=')
				from ctnt in anonymousTableElement
				select new TableElement(ident.Value, ctnt);


			tableElementParser.FixedParser =
				namedTableElementParser.Or(anonymousTableElement.Select(ctnts => new TableElement("", ctnts)));


			return tableElementParser.Parse;
		}

		private static Parser<char, TableElement> BuildRecipe()
		{
			var header =
				from _ in WhiteSpace
				from __ in Chars.Sequence("data")
				from ___ in Sandwich(':')
				from ____ in Chars.Sequence("extend")
				from _____ in Sandwich('(')
				select Unit.Instance;

			return from _ in header
				from table in TableElementParser
				from __ in Sandwich(')')
				from ___ in Chars.EndOfInput()
				select table;
		}


		public static Parser<char, Unit> Sandwich(char value)
		{
			return from _ in WhiteSpace
				from __ in Chars.Char(value)
				from ___ in WhiteSpace
				select Unit.Instance;
		}


		private static IEnumerable<Element> Merge(Element first, IEnumerable<Element> following)
		{
			yield return first;
			foreach (var element in following) yield return element;
		}
	}
}