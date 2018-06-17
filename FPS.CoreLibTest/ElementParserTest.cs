using System.Linq;
using FPS.CoreLib.Entity;
using FPS.CoreLib.Parser;
using Parseq;
using Xunit;
using Xunit.Abstractions;

namespace FPS.CoreLibTest
{
	public class ElementParserTest
	{
		public ElementParserTest(ITestOutputHelper output)
		{
			_output = output;
		}

		private readonly ITestOutputHelper _output;

		private void WriteLine(object value)
		{
			_output.WriteLine(value.ToString());
		}

		private static void Fail()
		{
			Assert.False(true);
		}

		private void Fail(ITokenStream<char> tokenStream, string message)
		{
			WriteLine(message);
			Fail();
		}

		[Fact]
		public void NestedTest()
		{
			var souce = "{1,2,named={10,20},{30,40}}".AsStream();

			var actual = ElementParser.TableElementParser(souce);

			actual.Case(Fail, (_, elem) =>
			{
				elem.GedChildren().Count().Is(4);
				elem.Traverse().Count().Is(8);
				var array = elem.Traverse().Where(x => x is ValueElement).Cast<ValueElement>()
					.Select(x => ((IntegerValue) x.Content).Value).ToArray();

				var exp = new[] {1, 2, 10, 20, 30, 40};

				foreach (var i in exp) array.Contains(i).IsTrue();
			});
		}


		[Fact]
		public void SimpleTest()
		{
			var source = "{1,2,3}";

			var actual = ElementParser.TableElementParser(source.AsStream());

			actual.Case(Fail, (_, elem) =>
			{
				var array = elem.GedChildren()
					.Cast<ValueElement>()
					.Select(x => x.Content)
					.Cast<IntegerValue>()
					.Select(x => x.Value)
					.ToArray();

				array.Length.Is(3);

				array.Contains(1).IsTrue();
				array.Contains(2).IsTrue();
				array.Contains(3).IsTrue();
			});
		}
	}
}