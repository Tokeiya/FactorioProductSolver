using System;
using System.Collections.Generic;
using System.Text;
using Parseq;
using Parseq.Combinators;

namespace FPS.CoreLib.Parser
{
	public static class ElementParser
	{
		public static readonly Parser<char, ValueElement> ValueElementParser;


		static ElementParser()
		{
			var whiteSpace =
				from _ in Chars.WhiteSpace().Many0().Ignore()
				select Unit.Instance;

			var lcb = from _ in Chars.Char('{').Ignore()
				from __ in whiteSpace
				select Unit.Instance;

			var rcb =
				from _ in Chars.WhiteSpace()
				from __ in Chars.Char('}').Ignore()
				select Unit.Instance;


			var comma =
				from _ in whiteSpace
				from __ in Chars.Char(',').Ignore()
				from ___ in whiteSpace
				select Unit.Instance;


			{
				var namedValueElement =
					from _ in Chars.WhiteSpace().Many0().Ignore()
					from id in Literals.Identifier
					from __ in Chars.WhiteSpace().Many0().Ignore()
					from ___ in Chars.Char('=').Ignore()
					from ____ in Chars.WhiteSpace().Many0().Ignore()
					from value in Literals.Literal
					select new ValueElement(id.Value, Value.Create(value));

				var anonymousValueElement =
					from _ in Chars.WhiteSpace().Many0().Ignore()
					from __ in Literals.Identifier.Not()
					from value in Literals.Literal
					select new ValueElement("", Value.Create(value));

				ValueElementParser = namedValueElement.Or(anonymousValueElement);
			}

			var element = new FixedPoint<char, Element>("");






		}
	}
}
