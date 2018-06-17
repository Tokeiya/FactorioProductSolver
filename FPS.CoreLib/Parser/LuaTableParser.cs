using System.Collections.Generic;
using FPS.CoreLib.Entity;
using Parseq;
using Parseq.Combinators;
using static Parseq.Combinator;
using static Parseq.Combinators.Chars;

namespace FPS.CoreLib.Parser
{
	public static class LuaTableParser
	{
		private static readonly Parser<char, Unit> WhiteSpace;

		static LuaTableParser()
		{
			Literal = Choice(RealLiteral, IntegerLiteral, BooleanLiteral, StringLiteral);

			WhiteSpace = BuildWhiteSpace();
			TableElementParser = BuildTableElement();
			RecipeParser = BuildRecipe();
		}

		public static Parser<char, TableElement> TableElementParser { get; }
		public static Parser<char, TableElement> RecipeParser { get; }

		public static Parser<char, Token> StringLiteral
			=> from _ in Char('"')
				from literal in NoneOf('"', '\\', '\r', '\n').Many0()
				from __ in Char('"')
				select Token.CreateToken(TokenTypes.StringLiteral, literal);

		public static Parser<char, Token> IntegerLiteral
			=> Digit().Many1().Select(c => Token.CreateToken(TokenTypes.IntegerLiteral, c));

		public static Parser<char, Token> RealLiteral =>
			from ip in Digit().Many1()
			from _ in Char('.')
			from fp in Digit().Many0()
			select Token.CreateToken(TokenTypes.RealLiteral, ip, '.'.Convert(), fp);

		public static Parser<char, Token> BooleanLiteral
			=> Choice(Sequence("true"), Sequence("false"))
				.Select(x => Token.CreateToken(TokenTypes.BooleanLiteral, x));

		public static Parser<char, Token> Identifier => BuildIdentifier();
		public static Parser<char, Token> Literal { get; }

		private static Parser<char, Unit> BuildWhiteSpace()
		{
			var newLine = Choice(
				Sequence("\r\n").Ignore(),
				Char('\r').Ignore(),
				Char('\n').Ignore()
			);

			var comment =
				from _ in Sequence("--")
				from __ in NoneOf('\r', '\n').Many0()
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
				from id in Identifier
				from __ in Sandwich('=')
				from value in Literal
				select new ValueElement(id.Value, Value.Create(value));

			var anonymousValueElement =
				from value in Literal
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
				from _ in Char(',').Optional()
				select Merge(first, following);

			var anonymousTableElement =
				from _ in Sandwich('{')
				from contents in tableContentsParser
				from __ in Sandwich('}')
				select contents;


			var namedTableElementParser =
				from ident in Identifier
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
				from __ in Sequence("data")
				from ___ in Sandwich(':')
				from ____ in Sequence("extend")
				from _____ in Sandwich('(')
				select Unit.Instance;

			return from _ in header
				from table in TableElementParser
				from __ in Sandwich(')')
				from ___ in EndOfInput()
				select table;
		}

		public static Parser<char, Unit> Sandwich(char value)
		{
			return from _ in WhiteSpace
				from __ in Char(value)
				from ___ in WhiteSpace
				select Unit.Instance;
		}

		private static IEnumerable<Element> Merge(Element first, IEnumerable<Element> following)
		{
			yield return first;
			foreach (var element in following) yield return element;
		}

		private static Parser<char, Token> BuildIdentifier()
		{
			var firstChar = Letter().Or(Char('_'));
			var followingChar = LetterOrDigit().Or(Char('_'));


			return from first in firstChar
				from following in followingChar.Many0()
				select Token.CreateToken(TokenTypes.Identifier, first.Convert(), following);
		}
	}
}