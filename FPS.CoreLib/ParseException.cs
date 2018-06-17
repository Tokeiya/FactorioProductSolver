using System;
using Parseq;

namespace FPS.CoreLib
{
	public class ParseException:Exception
	{
		public ParseException(IOption<IPair<char,Position>> current)
		{
			if (current.HasValue)
			{
				ExceptionOccuredChar = current.Value.Item0;

				Line = current.Value.Item1.Line;
				Column = current.Value.Item1.Column;
			}
		}

		public char? ExceptionOccuredChar { get; }

		public int? Line { get; }
		public int? Column { get; }



		public override string Message =>
			$"Parse error...Line:{Line ?? -1} Column:{Column ?? -1} Char:{ExceptionOccuredChar?.ToString() ?? "Unknown"}";
	}
}