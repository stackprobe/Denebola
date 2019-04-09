using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Utils
{
	public class CrashUtils
	{
		public static bool IsCrashed_Range_Range(double l1, double r1, double l2, double r2)
		{
			if (r1 < l1) throw null; // test
			if (r2 < l2) throw null; // test

			return l1 < r2 && l2 < r1;
		}

		public static bool IsCrashed_Rect_Rect(
			double l1, double t1, double r1, double b1,
			double l2, double t2, double r2, double b2
			)
		{
			return
				IsCrashed_Range_Range(l1, r1, l2, r2) &&
				IsCrashed_Range_Range(t1, b1, t2, b2);
		}

		public static bool IsCrashed_Rect_Line(
			double l, double t, double r, double b,
			double x1, double y1,
			double x2, double y2
			)
		{
			// FIXME 雑な判定
			return IsCrashed_Rect_Rect(
				l, t, r, b,
				Math.Min(x1, x2),
				Math.Min(y1, y2),
				Math.Max(x1, x2),
				Math.Max(y1, y2)
				);
		}

		public static bool IsCrashed_Range_Point(double l, double r, double p)
		{
			if (r < l) throw null; // test

			return l < p && p < r;
		}

		public static bool IsCrashed_Rect_Point(
			double l, double t, double r, double b,
			double x, double y
			)
		{
			return
				IsCrashed_Range_Point(l, r, x) &&
				IsCrashed_Range_Point(t, b, y);
		}

		public static double GetDistance(MapPoint a, MapPoint b)
		{
			return GetDistance(a.X, a.Y, b.X, b.Y);
		}

		public static double GetDistance(double x1, double y1, double x2, double y2)
		{
			return GetDistance(x1 - x2, y1 - y2);
		}

		public static double GetDistance(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}
	}
}
