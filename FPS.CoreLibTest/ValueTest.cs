using System;
using FPS.CoreLib.Parser;
using Xunit;
using Xunit.Abstractions;

namespace FPS.CoreLibTest
{
	public class ValueTest
	{
		private readonly ITestOutputHelper _output;

		public ValueTest(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void TextValueTest()
		{
			var target = new TextValue("hello world");

			target.Value.Is("hello world");
			target.ValueAsObject.Is("hello world");
			target.Type.Is(ValueTypes.String);

			Assert.Throws<ArgumentNullException>(() =>
			{
				var ret = new TextValue(null);
			});
		}

		[Fact]
		public void IntegerValueTest()
		{
			var target = new IntegerValue(42);
			target.Value.Is(42);
			target.ValueAsObject.Is(42);
			target.Type.Is(ValueTypes.Integer);
		}

		[Fact]
		public void RealValueTest()
		{
			var target = new RealValue(42.195);
			target.Value.Is(42.195);
			target.ValueAsObject.Is(42.195);
			target.Type.Is(ValueTypes.Real);
		}

		[Fact]
		public void BooleanValue()
		{
			var target=new BooleanValue(true);
			target.Value.Is(true);
			target.ValueAsObject.Is(true);
			target.Type.Is(ValueTypes.Boolean);

			target = new BooleanValue(false);
			target.Value.Is(false);
			target.ValueAsObject.Is(false);
		}




	}

}
