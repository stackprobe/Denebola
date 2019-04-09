using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte
{
	public class Consts
	{
		public const string MONOSPC_FONT_NAME = "ＭＳ ゴシック";

		public const int MPMD_MIN = 1;
		public const int MPMD_MAX = 1000000000;

		public const int TILE_WH = 300;
		public const int ZOOMING_COUNTER_MAX = 5;

		public const double X_MIN = -1000000.0;
		public const double Y_MIN = -1000000.0;
		public const double X_MAX = 1000000.0;
		public const double Y_MAX = 1000000.0;

		public const int LAYER_NUM = 20;

		public static readonly string[] LAYER_NAMES = new string[]
		{
			"行政区画(AdmArea)",
			"行政区画界線(AdmBdry)",
			"行政区画代表点(AdmPt)",
			"建築物(BldA)",
			"建築物の外周線(BldL)",
			"等高線(Cntr)",
			"町字界線(CommBdry)",
			"町字の代表点(CommPt)",
			"海岸線(Cstline)",
			"標高点(ElevPt)",
			"測量の基準点(GCP)",
			"鉄道中心線(RailCL)",
			"道路構成線(RdCompt)",
			"道路縁(RdEdg)",
			"街区の代表点(SBAPt)",
			"街区線(SBBdry)",
			"水域(WA)",
			"水涯線(WL)",
			"水部構造物面(WStrA)",
			"水部構造物線(WStrL)",
		};

		public const string LAYER_ON_PRESET_FULL = "11111111111111111111";
		public const string LAYER_ON_PRESET_LITE = "00001000100111000100";
		public const string LAYER_ON_PRESET_NONE = "00000000000000000000";
	}
}
