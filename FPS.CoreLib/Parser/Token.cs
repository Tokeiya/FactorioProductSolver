using System;
using System.Collections.Generic;
using System.Text;

namespace FPS.CoreLib.Parser
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
	}
}
