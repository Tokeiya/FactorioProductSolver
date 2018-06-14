using System.Collections.Generic;
using System.Text;

namespace FPS.CoreLib.Parser
{
	internal static class Extends
	{
		public static StringBuilder ToStringBuilder(this IEnumerable<char> source)
		{
			var bld = new StringBuilder();

			foreach (var c in source)
			{
				bld.Append(c);
			}

			return bld;
		}

		public static IEnumerable<char> Convert(this char c)
		{
			yield return c;
		}
	}
}