using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using Charlotte.Tests.t0001;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{7faa2a30-dd17-4bdb-ae7f-42e2ddc63f06}";
		public const string APP_TITLE = "t0001";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			//if (ProcMain.CUIError)
			{
				Console.WriteLine("Press ENTER.");
				Console.ReadLine();
			}
#endif
		}

		private void Main2(ArgsReader ar)
		{
			new t00010001Test().Test01(); // -- 0001
		}
	}
}
