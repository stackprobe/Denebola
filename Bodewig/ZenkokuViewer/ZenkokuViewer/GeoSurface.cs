using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class GeoSurface
	{
		public GeoCurve Exterior;
		public GeoCurve[] Interiors;

		public GeoSurface(GeoCurve exterior, GeoCurve[] interiors)
		{
			this.Exterior = exterior;
			this.Interiors = interiors;
		}
	}
}
