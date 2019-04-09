using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class Layer
	{
		public List<MapLine> Wk_Lines = new List<MapLine>();
		public List<MapPoint> Wk_Points = new List<MapPoint>();

		public MapLine[] Lines;
		public MapPoint[] Points;

		public MeshedGroupManager<MapLine> LineMgm;
		public MeshedGroupManager<MapPoint> PointMgm;
	}
}
