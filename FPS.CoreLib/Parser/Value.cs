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
	}
}