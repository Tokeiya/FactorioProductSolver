namespace FPS.CoreLib.Parser
{
	public sealed class IntegerValue : Value
	{
		public IntegerValue(int value) : base(ValueTypes.Integer)
		{
			Value = value;
		}

		public int Value { get; }
		public override object ValueAsObject => Value;
	}
}