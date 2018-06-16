using System;

namespace FPS.CoreLib.Parser
{
	public abstract class Value
	{
		protected Value(ValueTypes valueType)
		{
			if (!valueType.Verify()) throw new ArgumentException($"{valueType} is unexpected.");

			Type = valueType;
		}

		public ValueTypes Type { get; }
		public abstract object ValueAsObject { get; }

		public static Value Create(Token token)
		{
			switch (token.TokenType)
			{
				case TokenTypes.BooleanLiteral:
					return new BooleanValue(bool.Parse(token.Value));

				case TokenTypes.IntegerLiteral:
					return new IntegerValue(int.Parse(token.Value));

				case TokenTypes.StringLiteral:
					return new TextValue(token.Value);

				case TokenTypes.RealLiteral:
					return new RealValue(double.Parse(token.Value));
				default:
					throw new InconsistencyException();
			}
		}
	}
}