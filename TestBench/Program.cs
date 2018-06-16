using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FPS.CoreLib.Parser;
using Parseq;

namespace TestBench
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var dict = new Dictionary<string, int>();

			var elements = Directory.GetFiles(".\\SampleRecipe")
				.Select(p => ElementParser.RecipeParser(File.ReadAllText(p).AsStream()))
				.Select(x => x.Case((_, __) => throw new Exception(), (_, elem) => elem))
				.SelectMany(x => x.GedChildren()).ToArray();

			Console.WriteLine(elements.Length);

			foreach (var element in elements.Where(x=>x.Identifier!=""))
			{
				if (dict.TryGetValue(element.Identifier, out int cnt))
				{
					dict[element.Identifier] = ++cnt;
				}
				else
				{
					dict.Add(element.Identifier, 1);
				}
			}

			File.WriteAllLines("G:\\output.txt", dict.Select(pair => $"{pair.Key}\t{pair.Value}"));


		}

	}
}