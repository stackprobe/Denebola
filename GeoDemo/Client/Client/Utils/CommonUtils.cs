using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Tools;
using System.Windows.Forms;

namespace Charlotte
{
	public class CommonUtils
	{
		public static string ToString(Point? p)
		{
			return p == null ? "null" : ToString(p.Value);
		}

		public static string ToString(Point p)
		{
			return "(" + p.X + ", " + p.Y + ")";
		}

		public static string ToString(MapPoint? p)
		{
			return p == null ? "null" : ToString(p.Value);
		}

		public static string ToString(MapPoint p)
		{
			return "(" + p.X + ", " + p.Y + ")";
		}

		public static string PlotIndexToName(int index)
		{
			if (index < 0)
				throw new Exception("不正なプロット番号：" + index);

			if (index < 26)
				return StringTools.ALPHA.Substring(index, 1);

			return "" + index;
		}

		public static bool IsSame(Color a, Color b)
		{
			return
				a.A == b.A &&
				a.R == b.R &&
				a.G == b.G &&
				a.B == b.B;
		}

		public static void ShowMessage(string title, object message)
		{
			message = JString.ToJString("" + message, true, true, true, true); // 難読化によりスタックトレースがめちゃくちゃになるため。

			MessageBox.Show("" + message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
	}
}
