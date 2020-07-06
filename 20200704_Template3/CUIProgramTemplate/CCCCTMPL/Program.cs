using Charlotte.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{3f982fe1-acd0-4abd-b5a7-7281d065b50a}";
		public const string APP_TITLE = "CCCCTMPL";

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
			MessageBox.Show(APP_TITLE); // ---- 0001
		}
	}
}
