using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
	        var list = new List<(int x, int y)>();

	        IEnumerable<(int, int)> hoge = list;

        }
    }
}
