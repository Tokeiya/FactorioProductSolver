using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FPS.CoreLib.Entity;
using FPS.CoreLib.Parser;
using Parseq;

namespace FPS.CoreLib
{
	public static class DataLoader
	{
		public static TableElement Load(string path)
		{
			var source = File.ReadAllText(path ?? throw new ArgumentNullException(nameof(path))).AsStream();

			var rep = LuaTableParser.RecipeParser(source);
			return rep.Case((str, _) => throw new ParseException(str.Current), (_, elem) => elem);
		}
	}
}
