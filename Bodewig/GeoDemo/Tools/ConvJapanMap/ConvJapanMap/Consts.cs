using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class Consts
	{
		public static readonly string[] LayerNames = new string[]
		{
			"AdmArea",
			"AdmBdry",
			"AdmPt",
			"BldA",
			"BldL",
			"Cntr",
			"CommBdry",
			"CommPt",
			"Cstline",
			"ElevPt",
			"GCP",
			"RailCL",
			"RdCompt",
			"RdEdg",
			"SBAPt",
			"SBBdry",
			"WA",
			"WL",
			"WStrA",
			"WStrL",
		};

		public const double LAT_MIN = 20.0;
		public const double LAT_MAX = 46.0;
		public const double LON_MIN = 122.0;
		public const double LON_MAX = 154.0;
	}
}
