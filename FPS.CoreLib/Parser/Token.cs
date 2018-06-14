using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPS.CoreLib.Parser
{
	public class Token
	{
		public static Token CreateToken(TokenTypes tokenType, IEnumerable<char> souce) =>
			new Token(souce.ToStringBuilder().ToString(), tokenType);

		public static Token CreateToken(TokenTypes tokenType, params IEnumerable<char>[] souces) =>
			new Token(souces.SelectMany(x => x).ToStringBuilder().ToString(), tokenType);
		

		public Token(string value, TokenTypes tokenType)
		{
			Value = value ?? throw new ArgumentNullException(nameof(value));
			TokenType = tokenType;
		}

		public string Value { get; }
		public TokenTypes TokenType { get; }
	}
}
