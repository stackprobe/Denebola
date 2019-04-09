using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class MapPoint
	{
		public double X; // 東方向メートル
		public double Y; // 南方向メートル

		public MapPoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public MapPoint(GeoPoint gPoint)
		{
			this.X = (gPoint.Lon - Gnd.I.Config.GOrigin.Lon) * Gnd.I.MPD_Lon;
			this.Y = (gPoint.Lat - Gnd.I.Config.GOrigin.Lat) * Gnd.I.MPD_Lat;

			this.Y *= -1; // +-の方向が南北逆
		}

		public override string ToString()
		{
			return this.X.ToString("F6") + " " + this.Y.ToString("F6");
		}
	}
}
