using Charlotte.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charlotte
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			ProcMain.GUIMain(() => new MainWin(), APP_IDENT, APP_TITLE);
		}

		public const string APP_IDENT = "{76bac74a-da56-4ddb-846c-c096b2f343f6}";
		public const string APP_TITLE = "TTTTTMPL";
	}
}
