using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public struct MapPoint
	{
		public double X; // 東方向メートル
		public double Y; // 南方向メートル

		public MapPoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public override string ToString()
		{
			return this.X.ToString("F6") + " " + this.Y.ToString("F6");
		}
	}
}
