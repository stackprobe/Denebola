using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.t0001;

namespace Charlotte.Tests.t0001
{
	public class t00010001Test // -- 0001
	{
		public void Test01()
		{
			string message = "t00019999";

			if (new t00010001().Echo(message) != message)
				throw null;

			Console.WriteLine("OK!");
		}
	}
}
