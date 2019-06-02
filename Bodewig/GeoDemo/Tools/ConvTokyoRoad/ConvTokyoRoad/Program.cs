using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Charlotte.Tools;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{00a6156b-bec1-4b99-9e44-3a1a78d410a1}";
		public const string APP_TITLE = "ConvTokyoRoad";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			Console.WriteLine("Press ENTER.");
			Console.ReadLine();
#endif
		}

		private const string R_DIR = @"C:\var2\res\東京道路網";
		private const string W_DIR = @"C:\var2\res\東京道路網T";

		private void Main2(ArgsReader ar)
		{
			if (Directory.Exists(R_DIR) == false)
				throw new Exception("入力ディレクトリは存在しません。" + R_DIR);

			FileTools.Delete(W_DIR);
			FileTools.CreateDir(W_DIR);

			ConvRoad cm = new ConvRoad()
			{
				R_Dir = R_DIR,
				W_Dir = W_DIR,
			};

			cm.Invoke();
		}
	}
}
