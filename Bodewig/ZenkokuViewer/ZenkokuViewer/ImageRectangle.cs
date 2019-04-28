using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte
{
	public struct ImageRectangle
	{
		public float L;
		public float T;
		public float R;
		public float B;

		public static readonly ImageRectangle INIT_VALUE = new ImageRectangle()
		{
			L = float.MaxValue,
			T = float.MaxValue,
			R = float.MinValue,
			B = float.MinValue,
		};

		public static void Plot(ref ImageRectangle target, PointF p)
		{
			target.L = Math.Min(target.L, p.X);
			target.T = Math.Min(target.T, p.Y);
			target.R = Math.Max(target.R, p.X);
			target.B = Math.Max(target.B, p.Y);
		}
	}
}
