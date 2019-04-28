using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class GeoCurve
	{
		public GeoPoint[] Points;

		public GeoCurve(GeoPoint[] points)
		{
			this.Points = points;
		}
	}
}
