using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class GeoPoint
	{
		public double Lat; // 北緯
		public double Lon; // 東経

		public GeoPoint(double lat, double lon)
		{
			this.Lat = lat;
			this.Lon = lon;
		}

		public GeoPoint(MapPoint point)
		{
			double x = point.X;
			double y = point.Y;

			y *= -1; // +-の方向が南北逆

			this.Lon = x / Gnd.I.MPD_Lon + Gnd.I.Config.GOrigin.Lon;
			this.Lat = y / Gnd.I.MPD_Lat + Gnd.I.Config.GOrigin.Lat;
		}

		public override string ToString()
		{
			return this.Lat.ToString("F9") + " " + this.Lon.ToString("F9");
		}
	}
}
