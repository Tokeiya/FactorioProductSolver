using Parseq;
using Parseq.Combinators;

namespace FPS.CoreLib.Parser
{
	public static class Literals
	{
		static Literals()
		{
			IntegerLiteral = Chars.Digit().Many1().Select(c => Token.CreateToken(TokenTypes.IntegerLiteral, c));

			StringLiteral = from _ in Chars.Char('"')
				from literal in Chars.NoneOf('"', '\\', '\r', '\n').Many0()
				from __ in Chars.Char('"')
				select Token.CreateToken(TokenTypes.StringLiteral, literal);


			RealLiteral = from ip in Chars.Digit().Many1()
				from _ in Chars.Char('.')
				from fp in Chars.Digit().Many0()
				select Token.CreateToken(TokenTypes.RealLiteral, ip, '.'.Convert(), fp);

			BooleanLiteral = Combinator.Choice(Chars.Sequence("true"), Chars.Sequence("false"))
				.Select(x => Token.CreateToken(TokenTypes.BooleanLiteral, x));


			var firstChar = Chars.Letter().Or(Chars.Char('_'));
			var followingChar = Chars.LetterOrDigit().Or(Chars.Char('_'));


			Identifier = from first in firstChar
						 from following in followingChar.Many0()
				select Token.CreateToken(TokenTypes.Identifier, first.Convert(), following);

			Literal = Combinator.Choice(RealLiteral, IntegerLiteral, BooleanLiteral, StringLiteral);
		}

		public static Parser<char, Token> StringLiteral { get; }
		public static Parser<char, Token> IntegerLiteral { get; }
		public static Parser<char, Token> RealLiteral { get; }
		public static Parser<char, Token> BooleanLiteral { get; }
		public static Parser<char, Token> Identifier { get; }

		public static Parser<char, Token> Literal { get; }
	}
}