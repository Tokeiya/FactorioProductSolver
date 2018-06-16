using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Parseq;
using Parseq.Combinators;

namespace FPS.CoreLib.Parser
{
	public static class ParserHelper
	{
		public static readonly Parser<char, Unit> WhiteSpace =
			from _ in Chars.WhiteSpace().Many0().Ignore()
			select Unit.Instance;

		public static Parser<char, Unit> Sandwich(char value) =>
			from _ in WhiteSpace
			from __ in Chars.Char(value)
			from ___ in WhiteSpace
			select Unit.Instance;


	}

	public static class ElementParser
	{
		public static readonly Parser<char, TableElement> TableElementParser;



		private static IEnumerable<Element> Merge(Element first, IEnumerable<Element> following)
		{
			yield return first;

			foreach (var element in following)
			{
				yield return element;
			}

		}

		static ElementParser()
		{

			Parser<char, Element> valueElementParser;

			{
				var namedValueElement =
					from _ in ParserHelper.WhiteSpace
					from id in Literals.Identifier
					from __ in ParserHelper.Sandwich('=')
					from value in Literals.Literal
					select new ValueElement(id.Value, Value.Create(value));

				var anonymousValueElement =
					from _ in ParserHelper.WhiteSpace
					from __ in Literals.Identifier.Not()
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






		}
	}
}
