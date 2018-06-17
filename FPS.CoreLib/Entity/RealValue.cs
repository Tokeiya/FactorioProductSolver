namespace FPS.CoreLib.Entity
{
	public sealed class RealValue : Value
	{
		public RealValue(double value) : base(ValueTypes.Real)
		{
			Value = value;
		}

		public double Value { get; }

		public override object ValueAsObject => Value;
	}
}