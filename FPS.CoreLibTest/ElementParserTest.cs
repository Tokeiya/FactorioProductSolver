using System;
using System.Collections.Generic;
using System.Text;
using FPS.CoreLib.Parser;
using Parseq;
using Xunit;
using Xunit.Abstractions;

namespace FPS.CoreLibTest
{
	public class ElementParserTest
	{
		private readonly ITestOutputHelper _output;

		public ElementParserTest(ITestOutputHelper output)
		{
			_output = output;
		}

		public void WriteLine(object value)
		{
			_output.WriteLine(value.ToString());
		}

		public static void Fail()=>Assert.False(true);

		public void Fail(ITokenStream<char> tokenStream, string message)
		{
			WriteLine(message);
			Fail();
		}

		[Fact]
		public void NamedValueElementTest()
		{
			var actual = ElementParser.ValueElementParser("type = \"recipe\"".AsStream());

			actual.Case((_, __) => Assert.False(true), (_, elem) =>
			{
				if (elem is ValueElement e)
				{
					e.Identifier.Is("type");
					if (elem.Content is TextValue t)
					{
						t.Value.Is("recipe");
					}
					else Assert.False(true);

				}
				else Assert.False(true);
			});
		}

		[Fact]
		public void AnonymousValueElementTest()
		{
			var actual = ElementParser.ValueElementParser("1".AsStream());

			actual.Case(Fail, (_, elem) =>
			{
				if (elem is ValueElement e)
				{
					elem.Identifier.Is("");
					elem.Content.ValueAsObject.Is(1);
				}
				else Fail();
			});
		}






	}

}
