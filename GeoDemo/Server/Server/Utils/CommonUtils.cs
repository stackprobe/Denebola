using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Utils
{
	public class CommonUtils
	{
		public static void Print(object message)
		{
			Console.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd hh:mm:ss.fff] ") + JString.ToJString("" + message, true, true, true, true));
		}
	}
}
