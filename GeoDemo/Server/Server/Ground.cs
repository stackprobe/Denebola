using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class Gnd
	{
		public static Gnd _i;

		public static Gnd I
		{
			get
			{
				if (_i == null)
					_i = new Gnd();

				return _i;
			}
		}

		public Config Config = new Config();

		public void ConfigChanged()
		{
			MPD_Lat = MeterPerDegree.GetMeterPerDegree_Lat();
			MPD_Lon = MeterPerDegree.GetMeterPerDegree_Lon(Config.GOrigin.Lat);

			Map = new Map();
		}

		public double MPD_Lat; // 南北1度の距離(メートル)
		public double MPD_Lon; // 東西1度の距離(メートル)

		public Map Map;
	}
}
