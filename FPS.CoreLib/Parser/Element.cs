using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FPS.CoreLib.Parser
{
	public abstract class Element
	{
		protected Element(Element parent,ElementTypes elementType)
		{
			if (!elementType.Verify()) throw new ArgumentException($"{elementType} is unexpected value.");

			Type=elementType;

			if (elementType != ElementTypes.Root)
			{
				if (parent == null) throw new ArgumentNullException(nameof(parent));
			}
			else
			{
				if (parent != null) throw new ArgumentException("Root element can't have parent");
			}

			Parent = parent;
		}


		public ElementTypes Type{get;}
		public Element Parent { get; }


		public abstract IEnumerable<Element> GedChild();
		public abstract IEnumerable<Element> Traverse();

	}



}
