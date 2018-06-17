namespace FPS.CoreLib.Entity
{
	public sealed class BooleanValue : Value
	{
		public BooleanValue(bool value) : base(ValueTypes.Boolean)
		{
			Value = value;
		}

		public bool Value { get; }


		public override object ValueAsObject => Value;
	}
}