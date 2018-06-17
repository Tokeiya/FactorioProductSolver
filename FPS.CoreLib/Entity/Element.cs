using System;
using System.Collections.Generic;
using FPS.CoreLib.Parser;

namespace FPS.CoreLib.Entity
{
	public abstract class Element
	{
		protected Element(string identifier, ElementTypes elementType)
		{
			if (!elementType.Verify()) throw new ArgumentException($"{elementType} is unexpected value.");

			Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));

			Type = elementType;
		}


		public ElementTypes Type { get; }
		public string Identifier { get; }


		public abstract IEnumerable<Element> GedChildren();
		public abstract IEnumerable<Element> Traverse();
	}
}