using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class GeoLine
	{
		public GeoPoint A;
		public GeoPoint B;

		public GeoLine(GeoPoint a, GeoPoint b)
		{
			this.A = a;
			this.B = b;
		}
	}
}
