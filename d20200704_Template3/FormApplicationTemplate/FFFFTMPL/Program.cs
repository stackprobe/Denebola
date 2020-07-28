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

		public const string APP_IDENT = "{71e2461a-9638-4aa0-b43c-cd664b4c0b53}";
		public const string APP_TITLE = "FFFFTMPL";
	}
}
