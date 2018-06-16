using System.Collections.Generic;
using Parseq;
using Parseq.Combinators;

namespace FPS.CoreLib.Parser
{
	public static class ParserHelper
	{
		public static bool IsSuccess<TToken, T>(this IReply<TToken, T> reply) =>
			reply.Case((_, __) => false, (_, __) => true);



		public static readonly Parser<char, Unit> WhiteSpace;

		static ParserHelper()
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

			WhiteSpace = whiteSpace.Or(comment).Many0().Ignore();

		}

		public static Parser<char, Unit> Sandwich(char value)
		{
			return from _ in WhiteSpace
				from __ in Chars.Char(value)
				from ___ in WhiteSpace
				select Unit.Instance;
		}
	}

	public static class ElementParser
	{
		public static readonly Parser<char, TableElement> TableElementParser;
		public static readonly Parser<char, TableElement> RecipeParser;


		static ElementParser()
		{
			Parser<char, Element> valueElementParser;

			{
				var namedValueElement =
					from id in Literals.Identifier
					from __ in ParserHelper.Sandwich('=')
					from value in Literals.Literal
					select new ValueElement(id.Value, Value.Create(value));

				var anonymousValueElement =
					from value in Literals.Literal
					select new ValueElement("", Value.Create(value));

				valueElementParser = namedValueElement.Or(anonymousValueElement);
			}

			var tableElementParser = new FixedPoint<char, TableElement>("TableElement");

			{
				var contentParser = valueElementParser.Or(tableElementParser.Parse);

				var followingParser =
					(from _ in ParserHelper.Sandwich(',')
						from ctnt in contentParser
						select ctnt).Many0();


				var tableContentsParser =
					from first in contentParser
					from following in followingParser
					from _ in Chars.Char(',').Optional()
					select Merge(first, following);

				var anonymousTableElement =
					from _ in ParserHelper.Sandwich('{')
					from contents in tableContentsParser
					from __ in ParserHelper.Sandwich('}')
					select contents;


				var namedTableElementParser =
					from ident in Literals.Identifier
					from _ in ParserHelper.Sandwich('=')
					from ctnt in anonymousTableElement
					select new TableElement(ident.Value, ctnt);


				tableElementParser.FixedParser =
					namedTableElementParser.Or(anonymousTableElement.Select(ctnts => new TableElement("", ctnts)));


				TableElementParser = tableElementParser.Parse;
			}



			var header =
				from _ in ParserHelper.WhiteSpace
				from __ in Chars.Sequence("data")
				from ___ in ParserHelper.Sandwich(':')
				from ____ in Chars.Sequence("extend")
				from _____ in ParserHelper.Sandwich('(')
				select Unit.Instance;

			RecipeParser =
				from _ in header
				from table in TableElementParser
				from __ in ParserHelper.Sandwich(')')
				from ___ in Chars.EndOfInput()
				select table;
		}




		private static IEnumerable<Element> Merge(Element first, IEnumerable<Element> following)
		{
			yield return first;

			foreach (var element in following) yield return element;
		}
	}
}