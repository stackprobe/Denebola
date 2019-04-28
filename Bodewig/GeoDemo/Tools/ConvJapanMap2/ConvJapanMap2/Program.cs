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
		public const string APP_IDENT = "{5b913e43-8001-4dd7-826c-2cd339c23b73}";
		public const string APP_TITLE = "ConvJapanMap2";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			Console.WriteLine("Press ENTER.");
			Console.ReadLine();
#endif
		}

		private const string R_DIR = @"T:\Data\全国地図T";
		private const string W_DIR = @"T:\Data\全国地図TD";

		private void Main2(ArgsReader ar)
		{
			if (Directory.Exists(R_DIR) == false)
				throw new Exception("no R_Dir");

			FileTools.Delete(W_DIR);
			FileTools.CreateDir(W_DIR);

			ConvMap2 cm2 = new ConvMap2()
			{
				R_Dir = R_DIR,
				W_Dir = W_DIR,
			};

			cm2.Invoke();
		}
	}
}
