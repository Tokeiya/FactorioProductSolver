using System;
using System.Collections.Generic;
using FPS.CoreLib.Parser;

namespace FPS.CoreLib.Entity
{
	public sealed class TableElement : Element
	{
		private readonly List<Element> _children;


		public TableElement(string identifier, IEnumerable<Element> contents) : base(identifier, ElementTypes.Table)
		{
			if (contents == null) throw new ArgumentNullException(nameof(contents));

			_children = new List<Element>(contents);
		}


		public override IEnumerable<Element> GedChildren()
		{
			return _children;
		}

		public override IEnumerable<Element> Traverse()
		{
			foreach (var element in _children)
				if (element is ValueElement val)
				{
					yield return val;
				}
				else
				{
					yield return element;
					var tmp = element.Traverse();

					foreach (var elem in tmp) yield return elem;
				}
		}
	}
}