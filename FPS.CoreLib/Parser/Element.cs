using System;
using System.Collections.Generic;

namespace FPS.CoreLib.Parser
{
	public abstract class Element
	{
		protected Element(TableElement parent, string identifier, ElementTypes elementType)
		{
			if (!elementType.Verify()) throw new ArgumentException($"{elementType} is unexpected value.");

			Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));

			Type = elementType;

			if (elementType != ElementTypes.Dummy)
			{
				if (parent == null) throw new ArgumentNullException(nameof(parent));
			}
			else
			{
				if (parent != null) throw new ArgumentException("Root element can't have parent");
			}

			Parent = parent;
		}


		public ElementTypes Type { get; }
		public TableElement Parent { get; }
		public string Identifier { get; }


		public abstract IEnumerable<Element> GedChildren();
		public abstract IEnumerable<Element> Traverse();
	}


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
			_children.Add(child);
		}

		public override IEnumerable<Element> GedChildren()
		{
			return _children;
		}

		public override IEnumerable<Element> Traverse()
		{
			foreach (var elem in _children)
			{
				var ret = elem.Traverse();

				foreach (var element in ret) yield return element;
			}
		}
	}

	public sealed class ValueElement : Element
	{
		public ValueElement(TableElement parent, string identifier, Value content) : base(parent, identifier,
			ElementTypes.Value)
		{
			Content = content ?? throw new ArgumentNullException(nameof(content));
		}

		public Value Content { get; }


		public override IEnumerable<Element> GedChildren()
		{
			throw new NotSupportedException();
		}

		public override IEnumerable<Element> Traverse()
		{
			yield return this;
		}
	}
}