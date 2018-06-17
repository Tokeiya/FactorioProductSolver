using FPS.CoreLib.Entity;
using FPS.CoreLib.Parser;
using Parseq;
using Xunit;
using Xunit.Abstractions;

namespace FPS.CoreLibTest
{
	internal static class Extends
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
				(_, __) => { Assert.False(true); });
		}
	}

	public class LiteralParserTest
	{
		public LiteralParserTest(ITestOutputHelper output)
		{
			_output = output;
		}

		private readonly ITestOutputHelper _output;

		[Fact]
		public void BooleanLteralTest()
		{
			LuaTableParser.BooleanLiteral.Test("true", TokenTypes.BooleanLiteral);
			LuaTableParser.BooleanLiteral.Test("false", TokenTypes.BooleanLiteral);
		}

		[Fact]
		public void IdentifierTest()
		{
			LuaTableParser.Identifier.Test("ident", TokenTypes.Identifier);
			LuaTableParser.Identifier.Test("i7", TokenTypes.Identifier);
			LuaTableParser.Identifier.Test("hello_world", TokenTypes.Identifier);
			LuaTableParser.Identifier.FailTest("7i");
			LuaTableParser.Identifier.FailTest("10");
			LuaTableParser.Identifier.FailTest("");
		}


		[Fact]
		public void IntegerLiteralTest()
		{
			LuaTableParser.IntegerLiteral.Test("10", TokenTypes.IntegerLiteral);
			LuaTableParser.IntegerLiteral.Test("1", TokenTypes.IntegerLiteral);
		}

		[Fact]
		public void LiteralTest()
		{
			LuaTableParser.Literal.Test("10", TokenTypes.IntegerLiteral);
			LuaTableParser.Literal.Test("\"hello world\"", "hello world", TokenTypes.StringLiteral);
			LuaTableParser.Literal.Test("42.195", TokenTypes.RealLiteral);
			LuaTableParser.Literal.Test("true", TokenTypes.BooleanLiteral);

			LuaTableParser.Literal.FailTest(" ");
			LuaTableParser.Literal.FailTest("ident");
		}

		[Fact]
		public void RealLiteralTest()
		{
			LuaTableParser.RealLiteral.Test("42.195", TokenTypes.RealLiteral);
			LuaTableParser.RealLiteral.Test("42.", TokenTypes.RealLiteral);
			LuaTableParser.RealLiteral.FailTest(".42");
		}

		[Fact]
		public void StringLiteralTest()
		{
			LuaTableParser.StringLiteral.Test("\"hello world\"", "hello world", TokenTypes.StringLiteral);
			LuaTableParser.StringLiteral.Test("\"\"", string.Empty, TokenTypes.StringLiteral);
		}
	}
}