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
		public void AddTest()
		{
			var target = new TableElement(TableElement.DummyElement, "target");
			var another = new TableElement(TableElement.DummyElement, "another");

			var elem = new ValueElement(target, "elem", new TextValue("hello"));

			target.Add(elem);
			target.GedChildren().Count().Is(1);

			foreach (var act in target.GedChildren())
			{
				switch (act)
				{
					case ValueElement ve:
						if (ve.Content is TextValue v)
						{
							v.Value.Is("hello");
						}
						else Assert.False(true);

						break;

					default:
						Assert.False(true);
						break;
				}
			}

			Assert.Throws<ArgumentException>(() => another.Add(elem));
			another.GedChildren().Any().IsFalse();

			Assert.Throws<ArgumentException>(() => another.Add(another));
			another.GedChildren().Any().IsFalse();

		}



		[Fact]
		public void TraverseTest()
		{
			var root = new TableElement(TableElement.DummyElement, "root");

			var tbl = new TableElement(root, "tbl");
			var tbl2 = new TableElement(tbl, "tbl2");


			root.Add(tbl);
			tbl.Add(tbl2);

			var rootVal = new ValueElement(root, "rootVal", new TextValue("rootVal"));
			root.Add(rootVal);

			var tblVal = new ValueElement(tbl, "tblVal", new IntegerValue(42));
			tbl.Add(tblVal);

			var tbl2Val = new ValueElement(tbl2, "tbl2Val", new RealValue(Math.PI));
			tbl2.Add(tbl2Val);


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
