using Charlotte.Tools;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{cf48ef73-27f8-4781-aaed-4963c2aab7a5}";
		public const string APP_TITLE = "JP2ToBmp";

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
			try
			{
				string rFile = ar.NextArg();
				string wFile = ar.NextArg();

				FileTools.Delete(wFile);

				try
				{
					// ---- FreeImage ここから

					if (FreeImage.IsAvailable() == false)
						throw new Exception("no FreeImage.dll");

					FIBITMAP dib = FreeImage.LoadEx(rFile);

					if (dib.IsNull)
						throw new Exception("Failed load image");

					FreeImage.SaveEx(ref dib, wFile, false);
					FreeImage.UnloadEx(ref dib);

					// ---- FreeImage ここまで
				}
				catch
				{
					FileTools.Delete(wFile);
					throw;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
