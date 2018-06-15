using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FPS.CoreLib.Parser
{
	public enum ElementTypes
	{
		Table=1,
		Value
	}

	public abstract class Element
	{
		protected Element(ElementTypes elementType)
		{
			Type=elementType;
		}


		public ElementTypes Type{get;}
	}

	public class TableElement:Element
	{
		public TableElement():base(ElementTypes.Table)
		{
			
		}

	}


}
