using System;
using System.Collections.Generic;
using System.Linq;
using FPS.CoreLib.Parser;

namespace FPS.CoreLib.Entity
{
	public class Token
	{
		public Token(string value, TokenTypes tokenType)
		{
			Value = value ?? throw new ArgumentNullException(nameof(value));
			TokenType = tokenType;
		}

		public string Value { get; }
		public TokenTypes TokenType { get; }

		public static Token CreateToken(TokenTypes tokenType, IEnumerable<char> souce)
		{
			return new Token(souce.ToStringBuilder().ToString(), tokenType);
		}

		public static Token CreateToken(TokenTypes tokenType, params IEnumerable<char>[] souces)
		{
			return new Token(souces.SelectMany(x => x).ToStringBuilder().ToString(), tokenType);
		}
	}
}