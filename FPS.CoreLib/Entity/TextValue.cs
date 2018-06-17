using System;

namespace FPS.CoreLib.Entity
{
	public sealed class TextValue : Value
	{
		public TextValue(string value) : base(ValueTypes.String)
		{
			Value = value ?? throw new ArgumentNullException(nameof(value));
		}

		public string Value { get; }
		public override object ValueAsObject => Value;
	}
}