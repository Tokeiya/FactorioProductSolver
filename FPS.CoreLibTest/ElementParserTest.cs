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







	}

}
