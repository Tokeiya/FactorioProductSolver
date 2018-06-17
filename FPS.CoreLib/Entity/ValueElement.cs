using System;
using System.Collections.Generic;

namespace FPS.CoreLib.Entity
{
	public sealed class ValueElement : Element
	{
		public ValueElement(string identifier, Value content) : base(identifier,
			ElementTypes.Value)
		{
			Content = content ?? throw new ArgumentNullException(nameof(content));
		}

		public Value Content { get; }



		public override IEnumerable<Element> Traverse()
		{
			yield return this;
		}
	}
}