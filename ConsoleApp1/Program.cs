using FPS.CoreLib.Parser;
using Parseq;
using Parseq.Combinators;

namespace ConsoleApp1
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var hoge =
				from _ in Chars.WhiteSpace().Many0().Ignore()
				from id in Literals.Identifier
				from __ in Chars.WhiteSpace().Many0().Ignore()
				from ___ in Chars.Char('=').Ignore()
				from ____ in Chars.WhiteSpace().Many0().Ignore()
				from value in Literals.Literal
				select new ValueElement(id.Value, Value.Create(value));
		}
	}
}