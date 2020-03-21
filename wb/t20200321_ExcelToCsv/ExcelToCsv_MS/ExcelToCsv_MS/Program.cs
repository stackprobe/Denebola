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
		public const string APP_IDENT = "{1ef1bab7-658a-4278-8651-63fa76e5242c}";
		public const string APP_TITLE = "ExcelToCsv_MS";

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
#if DEBUG
			Test01();
			//Main3(ar);
#else
			Main3(ar);
#endif
		}

		private void Test01()
		{
			// $_git:secretBegin
//
			///////////
				///////////////////////////////////////////////////////////
				////////////////////////
				//
//
			// $_git:secretEnd
		}

		private void Main3(ArgsReader ar)
		{
			string rFile = ar.NextArg();
			string wDir = ar.NextArg();

			ExcelToCsv(rFile, wDir);
		}

		private const int EMPTY_ROW_SPAN_MAX = 100;
		private const int COL_EMPTY_CELL_SPAN_MAX = 100;

		private void ExcelToCsv(string rFile, string wDir)
		{
			Console.WriteLine("< " + rFile);
			Console.WriteLine("> " + wDir);

			FileTools.CreateDir(wDir);

			rFile = FileTools.MakeFullPath(rFile);

			Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
			try
			{
				//app.DisplayAlerts = true;

				Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Open(rFile);
				try
				{
					for (int sheetIndex = 0; sheetIndex < workbook.Sheets.Count; sheetIndex++)
					{
						Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Sheets[sheetIndex + 1];
						try
						{
							string wFile = Path.Combine(wDir, worksheet.Name + ".csv");

							using (CsvFileWriter writer = new CsvFileWriter(wFile))
							{
								int emptyRowSpan = 0;

								for (int rowidx = 0; ; rowidx++)
								{
									var row = worksheet.Rows[rowidx + 1];
									int colEmptyCellSpan = 0;
									bool emptyRowFlag = true;

									for (int colidx = 0; ; colidx++)
									{
										var cell = row.Cells[colidx + 1];
										string value = "" + cell.Value;

										if (value == "")
										{
											colEmptyCellSpan++;

											if (COL_EMPTY_CELL_SPAN_MAX < colEmptyCellSpan)
												break;
										}
										else
										{
											colEmptyCellSpan = 0;
											emptyRowFlag = false;
										}
										writer.WriteCell(value);
									}
									writer.EndRow();

									if (emptyRowFlag)
									{
										emptyRowSpan++;

										if (EMPTY_ROW_SPAN_MAX < emptyRowSpan)
											break;
									}
									else
										emptyRowSpan = 0;
								}
							}
						}
						finally
						{
							System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
							worksheet = null;
						}
					}
				}
				finally
				{
					workbook.Close();
					System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
					workbook = null;
				}
			}
			finally
			{
				app.Quit();
				System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
				app = null;
			}

			Console.WriteLine("done");
		}
	}
}
