using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class MapLine
	{
		public MapPoint A;
		public MapPoint B;

		public MapLine(MapPoint a, MapPoint b)
		{
			this.A = a;
			this.B = b;
		}

		public MapLine(GeoLine gLine)
		{
			this.A = new MapPoint(gLine.A);
			this.B = new MapPoint(gLine.B);
		}
	}
}
