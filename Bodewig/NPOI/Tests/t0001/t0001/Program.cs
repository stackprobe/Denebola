using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Charlotte.Tools;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{7a26a3ca-59d2-4e83-b285-f89b8252160a}";
		public const string APP_TITLE = "t0001";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			Console.WriteLine("Press ENTER.");
			Console.ReadLine();
#endif
		}

		private void Main2(ArgsReader ar)
		{
			//Test01();
			//Test02();
			Test03();
		}

		private void Test01()
		{
			IWorkbook book = new XSSFWorkbook();

			book.CreateSheet("test_test_test");

			using (FileStream writer = new FileStream(@"C:\temp\1.xlsx", FileMode.Create, FileAccess.Write))
			{
				book.Write(writer);
			}
		}

		private void Test02()
		{
			IWorkbook book = new XSSFWorkbook();
			book.CreateSheet();
			ISheet sheet = book.GetSheetAt(0);
			IRow row = sheet.CreateRow(0);
			ICell cell = row.CreateCell(0);
			cell.SetCellValue("test_test_test 漢字・日本語");

			using (FileStream writer = new FileStream(@"C:\temp\t0001_02.xlsx", FileMode.Create, FileAccess.Write))
			{
				book.Write(writer);
			}
		}

		private void Test03()
		{
			IWorkbook book = new XSSFWorkbook();
			book.CreateSheet();
			ISheet sheet = book.GetSheetAt(0);
			sheet.CreateRow(0);
			IRow row = sheet.GetRow(0);
			row.CreateCell(0);
			ICell cell = row.GetCell(0);
			cell.SetCellValue("!!! test_test_test 漢字・日本語 !!!");

			using (FileStream writer = new FileStream(@"C:\temp\t0001_03.xlsx", FileMode.Create, FileAccess.Write))
			{
				book.Write(writer);
			}
		}
	}
}
