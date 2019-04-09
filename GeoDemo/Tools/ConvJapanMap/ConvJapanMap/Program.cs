using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using Charlotte.Tools;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{42c1f935-9cf1-4c1a-90f9-85e9f11766e3}";
		public const string APP_TITLE = "ConvJapanMap";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			Console.WriteLine("Press ENTER.");
			Console.ReadLine();
#endif
		}

		private const string R_DIR = @"T:\Data\全国地図";
		private const string W_DIR = @"T:\Data\全国地図T";

		private void Main2(ArgsReader ar)
		{
			if (Directory.Exists(R_DIR) == false)
				throw new Exception("入力ディレクトリは存在しません。" + R_DIR);

			FileTools.Delete(W_DIR);
			FileTools.CreateDir(W_DIR);

			ConvMap cm = new ConvMap()
			{
				R_Dir = R_DIR,
				W_Dir = W_DIR,
			};

			cm.Invoke();
		}
	}
}
