using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte
{
	public class Dai2JiMesh
	{
		public double LatMin;
		public double LatMax;
		public double LonMin;
		public double LonMax;

		public string Code;

		public Dai2JiMesh(string code)
		{
			if (StringTools.ReplaceChars(code, StringTools.DECIMAL, '9') != "999999")
				throw new Exception("不明なメッシュコードです。" + code);

			int iLat = int.Parse(code.Substring(0, 2));
			int iLon = int.Parse(code.Substring(2, 2));
			int iLatB = int.Parse(code.Substring(4, 1));
			int iLonB = int.Parse(code.Substring(5, 1));

			double lat1 = ILatToLat(iLat);
			double lat2 = ILatToLat(iLat + 1);
			double lon1 = ILonToLon(iLon);
			double lon2 = ILonToLon(iLon + 1);

			LatMin = LatBToLat(lat1, lat2, iLatB);
			LatMax = LatBToLat(lat1, lat2, iLatB + 1);
			LonMin = LatBToLat(lon1, lon2, iLonB);
			LonMax = LatBToLat(lon1, lon2, iLonB + 1);

			Code = code;
		}

		private double LatBToLat(double lat1, double lat2, int iLatB) // lonでもok
		{
			return lat1 + (lat2 - lat1) * iLatB / 8.0;
		}

		private double ILatToLat(int iLat)
		{
			return iLat / 1.5;
		}

		private double ILonToLon(int iLon)
		{
			return iLon + 100.0;
		}

		public double GetLat(double rate)
		{
			return LatMin + rate * (LatMax - LatMin);
		}

		public double GetLon(double rate)
		{
			return LonMin + rate * (LonMax - LonMin);
		}
	}
}
