using System;
using FPS.CoreLib.Parser;
using Parseq;
using Xunit;
using Xunit.Abstractions;

namespace FPS.CoreLibTest
{

	static class Extends
	{
		public static void Test(this Parser<char, Token> target, string source, TokenTypes expectedTokenType)
		{
			target.IsNotNull();
			source.IsNotNull();

			var actual = target(source.AsStream());

			actual.Case((_, __) => Assert.True(false), (str, token) =>
			{
				token.Value.Is(source);
				token.TokenType.Is(expectedTokenType);
			});
		}

		public static void Test(this Parser<char, Token> target, string source, string expectedValue,
			TokenTypes expectedToken)
		{
			target.IsNotNull();
			source.IsNotNull();
			expectedValue.IsNotNull();

			var actual = target(source.AsStream());


			actual.Case((_, __) => Assert.False(true), (str, token) =>
			{
				token.Value.Is(expectedValue);
				token.TokenType.Is(expectedToken);
			});
		}

		public static void FailTest(this Parser<char, Token> target, string source)
		{
			target.IsNotNull();
			source.IsNotNull();

			var actual = target(source.AsStream());

			actual.Case((_, __) => Assert.True(true),
				(_, __) =>
				{
					Assert.False(true);
				});
		}
	}

	public class LiteralParserTest
	{
		private readonly ITestOutputHelper _output;

		public LiteralParserTest(ITestOutputHelper output)
		{
			_output = output;
		}


		[Fact]
		public void IntegerLiteralTest()
		{
			Literals.IntegerLiteral.Test("10", TokenTypes.IntegerLiteral);
			Literals.IntegerLiteral.Test("1", TokenTypes.IntegerLiteral);
		}

		[Fact]
		public void StringLiteralTest()
		{
			Literals.StringLiteral.Test("\"hello world\"","hello world", TokenTypes.StringLiteral);
			Literals.StringLiteral.Test("\"\"", string.Empty, TokenTypes.StringLiteral);
		}

		[Fact]
		public void RealLiteralTest()
		{
			Literals.RealLiteral.Test("42.195", TokenTypes.RealLiteral);
			Literals.RealLiteral.Test("42.", TokenTypes.RealLiteral);
			Literals.RealLiteral.FailTest(".42");
		}

		[Fact]
		public void BooleanLteralTest()
		{
			Literals.BooleanLiteral.Test("true", TokenTypes.BooleanLiteral);
			Literals.BooleanLiteral.Test("false", TokenTypes.BooleanLiteral);
		}

		[Fact]
		public void IdentifierTest()
		{
			Literals.Identifier.Test("ident", TokenTypes.Identifier);
			Literals.Identifier.Test("i7", TokenTypes.Identifier);
			Literals.Identifier.FailTest("7i");
			Literals.Identifier.FailTest("10");
			Literals.Identifier.FailTest("");
		}

		[Fact]
		public void LiteralTest()
		{
			Literals.Literal.Test("10", TokenTypes.IntegerLiteral);
			Literals.Literal.Test("\"hello world\"", "hello world", TokenTypes.StringLiteral);
			Literals.Literal.Test("42.195", TokenTypes.RealLiteral);
			Literals.Literal.Test("true", TokenTypes.BooleanLiteral);

			Literals.Literal.FailTest(" ");
			Literals.Literal.FailTest("ident");

		}






	}

}
