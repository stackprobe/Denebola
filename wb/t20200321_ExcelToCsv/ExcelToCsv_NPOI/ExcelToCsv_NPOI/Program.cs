using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Charlotte.Tools;
using NPOI.SS.UserModel;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{0fe59f9c-ede5-4314-8884-971166ccdb88}";
		public const string APP_TITLE = "ExcelToCsv_NPOI";

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

		private const int EMPTY_ROW_SPAN_MAX = 100;
		private const int COL_EMPTY_CELL_SPAN_MAX = 100;

		private void Main3(ArgsReader ar)
		{
			string rFile = ar.NextArg();
			string wDir = ar.NextArg();

			ExcelToCsv(rFile, wDir);
		}

		private void ExcelToCsv(string rFile, string wDir)
		{
			Console.WriteLine("< " + rFile);
			Console.WriteLine("> " + wDir);

			FileTools.Delete(wDir);
			FileTools.CreateDir(wDir);

			rFile = FileTools.MakeFullPath(rFile);

			IWorkbook book = WorkbookFactory.Create(rFile);
			try
			{
				for (int sheetIndex = 0; sheetIndex < book.NumberOfSheets; sheetIndex++)
				{
					ISheet sheet = book.GetSheetAt(sheetIndex);
					try
					{
						string wFile = Path.Combine(wDir, sheet.SheetName + ".csv");

						using (CsvFileWriter writer = new CsvFileWriter(wFile))
						{
							int emptyRowSpan = 0;

							for (int rowidx = 0; ; rowidx++)
							{
								IRow row = sheet.GetRow(rowidx);

								int colEmptyCellSpan = 0;
								bool emptyRowFlag = true;

								if (row != null)
								{
									for (int colidx = 0; ; colidx++)
									{
										ICell cell = row.GetCell(colidx);
										string value;

										if (cell == null)
											value = S_CELL_VALUE_EMPTY;
										else
											value = GetCellValue(cell, rowidx, colidx);

										if (string.IsNullOrEmpty(value))
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
						// noop
					}
				}
			}
			finally
			{
				//book.Dispose();
				book.Close();
				book = null;
			}

			Console.WriteLine("done");
		}

		private const string S_CELL_VALUE_EMPTY = "";

		private static string GetCellValue(ICell cell, int suppl_rowidx, int suppl_colidx)
		{
			try
			{
				switch (cell.CellType)
				{
					case CellType.Numeric:
						if (DateUtil.IsCellDateFormatted(cell))
						{
							DateTime value = cell.DateCellValue;

							return value.ToString();
						}
						else
						{
							double value = cell.NumericCellValue;

							return value.ToString();
						}

					case CellType.Boolean:
						{
							bool value = cell.BooleanCellValue;

							return value.ToString();
						}

					case CellType.Formula:
						{
							string value = cell.CellFormula;

							if (value == null)
								return S_CELL_VALUE_EMPTY;

							return value;
						}

					default:
						{
							string value = cell.StringCellValue;

							if (value == null)
								return S_CELL_VALUE_EMPTY;

							return value;
						}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(string.Format("Error on ({0}, {1}), e: {2}", suppl_rowidx, suppl_colidx, e));

				return S_CELL_VALUE_EMPTY;
			}
		}
	}
}
