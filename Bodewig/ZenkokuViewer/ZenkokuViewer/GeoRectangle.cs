using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public struct GeoRectangle
	{
		public double LatMin;
		public double LatMax;
		public double LonMin;
		public double LonMax;

		public GeoRectangle(double latMin, double latMax, double lonMin, double lonMax)
		{
			this.LatMin = latMin;
			this.LatMax = latMax;
			this.LonMin = lonMin;
			this.LonMax = lonMax;
		}

		public GeoPoint CenterPoint
		{
			get
			{
				return new GeoPoint((this.LatMin + this.LatMax) / 2.0, (this.LonMin + this.LonMax) / 2.0);
			}
		}
	}
}
