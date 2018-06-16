using System;
using System.IO;
using FPS.CoreLib.Parser;
using Parseq;
using Parseq.Combinators;

namespace ConsoleApp1
{
	internal class Program
	{
		private static void Main(string[] args)
		{

			NewMethod();
			Console.ReadLine();
		}

		private static void NewMethod()
		{
			var recipe = File.ReadAllText("..\\..\\..\\..\\SampleRecipe\\recipe.lua");


			var ret = ElementParser.RecipeParser(recipe.AsStream());

			ret.Case((stream, msg) =>
			{
				var pos = stream.Current.Value.Item1;

				Console.WriteLine($"Line:{pos.Line},Col:{pos.Column}");

				Console.WriteLine("fail");
			}, (_, elem) => { Console.WriteLine("success"); });
		}
	}
}