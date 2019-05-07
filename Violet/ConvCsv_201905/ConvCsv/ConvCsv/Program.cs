using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Charlotte.Tools;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{6f3b9ac7-578e-405b-964f-9d9b9962d2d2}";
		public const string APP_TITLE = "ConvCsv";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			Console.WriteLine("Press ENTER.");
			Console.ReadLine();
#endif
		}

		public static string RFile;
		public static string WFile;
		public static string CellToAdd;
		public static Encoding Encoding = StringTools.ENCODING_SJIS;

		private void Main2(ArgsReader ar)
		{
		readArgs:
			if (ar.ArgIs("/R"))
			{
				RFile = ar.NextArg();
				goto readArgs;
			}
			if (ar.ArgIs("/W"))
			{
				WFile = ar.NextArg();
				goto readArgs;
			}
			if (ar.ArgIs("/S"))
			{
				CellToAdd = ar.NextArg();
				goto readArgs;
			}
			if (ar.ArgIs("/E"))
			{
				Encoding = Encoding.GetEncoding(ar.NextArg());
				goto readArgs;
			}
			if (ar.ArgIs("/?"))
			{
				Console.Write(
					File.ReadAllText(
						Path.Combine(ProcMain.SelfDir, Path.GetFileNameWithoutExtension(ProcMain.SelfFile) + ".txt"),
						StringTools.ENCODING_SJIS
						)
					);
				return;
			}

			Console.WriteLine("読み込みファイル：" + RFile);
			Console.WriteLine("書き出しファイル：" + WFile);
			Console.WriteLine("追加文字列(セル)：" + CellToAdd);
			Console.WriteLine("エンコーディング：" + Encoding);

			Main3();
		}

		private void Main3()
		{
			using (CsvFileReader reader = new CsvFileReader(RFile, Encoding))
			using (CsvFileWriter writer = new CsvFileWriter(WFile, false, Encoding))
			{
				for (; ; )
				{
					string[] row = reader.ReadRow();

					if (row == null)
						break;

					writer.WriteCells(row);
					writer.WriteCell(CellToAdd);
					writer.EndRow();
				}
			}

			Console.WriteLine("OK");
		}
	}
}
