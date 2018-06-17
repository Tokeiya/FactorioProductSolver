using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FPS.CoreLib.Parser;
using Parseq;
using Xunit;
using Xunit.Abstractions;

namespace FPS.CoreLibTest
{
	public class RecipeReadTest
	{
		private readonly ITestOutputHelper _output;

		public RecipeReadTest(ITestOutputHelper output)
		{
			_output = output;
		}


		[Fact]
		public void ReadAllTest()
		{
			var files = Directory.GetFiles(".\\SampleRecipe");


			foreach (var path in files)
			{
				var source = File.ReadAllText(path).AsStream();
				LuaTableParser.RecipeParser(source).IsSuccess().IsTrue();
			}
		}


	}

}
