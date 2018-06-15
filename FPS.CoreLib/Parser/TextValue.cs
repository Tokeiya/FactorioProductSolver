namespace FPS.CoreLib.Parser
{
	public sealed class TextValue : Value
	{
		public TextValue(string value) : base(ValueTypes.String)
		{
			Value = value;
		}

		public string Value { get; }
		public override object ValueAsObject => Value;
	}
}