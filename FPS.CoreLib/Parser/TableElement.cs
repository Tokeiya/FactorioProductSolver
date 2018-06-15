using System;
using System.Collections.Generic;

namespace FPS.CoreLib.Parser
{
	public sealed class TableElement : Element
	{
		private readonly List<Element> _children = new List<Element>();

		private TableElement() : base(null, "Root", ElementTypes.Dummy)
		{
		}

		public TableElement(TableElement parent, string identifier) : base(parent, identifier, ElementTypes.Table)
		{
		}

		public static TableElement DummyElement => new TableElement();

		public void Add(Element child)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (child.Parent != this) throw new ArgumentException($"Parent mismatch.");
			if (child == this) throw new ArgumentException("Recursion");
			_children.Add(child);
		}

		public override IEnumerable<Element> GedChildren()
		{
			return _children;
		}

		public override IEnumerable<Element> Traverse()
		{
			foreach (var element in _children)
			{
				if (element is ValueElement val) yield return val;
				else
				{
					yield return element;
					var tmp = element.Traverse();

					foreach (var elem in tmp)
					{
						yield return elem;
					}
				}

			}
		}
	}
}