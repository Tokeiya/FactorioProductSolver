using System;
using System.Collections.Generic;

namespace FPS.CoreLib.Entity
{
	public abstract class Element
	{
		protected Element(string identifier)
		{
			Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
		}

		public string Identifier { get; }


		public abstract IEnumerable<Element> Traverse();
	}
}