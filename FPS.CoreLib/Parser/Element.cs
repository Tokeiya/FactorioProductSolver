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