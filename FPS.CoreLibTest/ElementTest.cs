using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPS.CoreLib.Parser;
using Xunit;
using Xunit.Abstractions;

namespace FPS.CoreLibTest
{
	public class ElementTest
	{
		private readonly ITestOutputHelper _output;

		public ElementTest(ITestOutputHelper output)
		{
			_output = output;
		}




		[Fact]
		public void TraverseTest()
		{

			var rootVal = new ValueElement("rootVal", new TextValue("rootVal"));
			var tblVal = new ValueElement("tblVal", new IntegerValue(42));

			var tbl2Val = new ValueElement("tbl2Val", new RealValue(Math.PI));

			var tbl2 = new TableElement("tbl2", new[] {tbl2Val});
			var tbl = new TableElement("tbl", new Element[] {tbl2, tblVal});
			var root=new TableElement("root",new Element[]{tbl,rootVal});



			var set = new HashSet<string>();

			foreach (var element in root.Traverse())
			{
				if (!set.Add(element.Identifier))
				{
					Assert.False(true);
				}
			}

			set.Count.Is(5);
			set.Contains("tbl").IsTrue();
			set.Contains("tbl2").IsTrue();
			set.Contains("tbl2Val").IsTrue();
			set.Contains("tblVal").IsTrue();
			set.Contains("rootVal").IsTrue();


		}

		
	}

}
