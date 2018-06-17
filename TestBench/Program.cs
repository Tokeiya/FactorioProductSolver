using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FPS.CoreLib.Entity;
using FPS.CoreLib.Parser;
using Parseq;

namespace TestBench
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var elements = Directory.GetFiles(".\\SampleRecipe")
				.Select(p => LuaTableParser.RecipeParser(File.ReadAllText(p).AsStream()))
				.Select(x => x.Case((_, __) => throw new Exception(), (_, elem) => elem))
				.SelectMany(x => x.GedChildren()).ToArray();

			var hoge=elements.Where(x => x.GedChildren().OfType<TableElement>().Any(elem => elem.Identifier == "name"))
				.Select(elem => elem.GedChildren().Where(x => x.Identifier == "name").First())
				.ToArray();

			foreach (var element in hoge.Select(x=>((ValueElement)x).Content.ValueAsObject))
			{
				Console.WriteLine(element);
			}


		}

	}
}