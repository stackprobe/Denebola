using Charlotte.Tests.t0001;
using Charlotte.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test01
{
	class Program
	{
		public const string APP_IDENT = "{5ccdcf1d-2844-4d77-b99f-edffa467967d}";
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
