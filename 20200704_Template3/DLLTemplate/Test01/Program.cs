using Charlotte.Tests.DDDDTMNS;
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
		public const string APP_IDENT = "{a99e8995-2960-4c6c-90b1-40ed6e04edd0}";
		public const string APP_TITLE = "DDDDTMPL";

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
			new DDDDTMPL0001Test().Test01(); // -- 0001
		}
	}
}
