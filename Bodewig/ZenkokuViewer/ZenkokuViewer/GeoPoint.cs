using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utils;

namespace Charlotte
{
	public struct GeoPoint
	{
		public double Lat;
		public double Lon;

		public GeoPoint(double lat, double lon)
		{
			this.Lat = lat;
			this.Lon = lon;
		}
	}
}
