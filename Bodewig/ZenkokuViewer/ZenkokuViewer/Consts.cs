using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class Consts
	{
		// ==== Core ====

		public const double LAT_MIN = 20.0;
		public const double LAT_MAX = 46.0;
		public const double LON_MIN = 122.0;
		public const double LON_MAX = 154.0;

		public const int MPMD_MIN = 1;
		public const int MPMD_MAX = 1000000000;

		public const int ZOOMING_COUNTER_MAX = 10;

		public const int TILE_WH = 300;

		public const int MP_WH_MIN = 100; // MapPicture Width/Height Min
		public const int DRAW_TILE_WH_MIN = 10;
		public const int DRAW_TILE_WH_MAX = 9000;

		// ==== Core ここまで

		public const string LAYER_ROOT_DIR = @"T:\Data\全国地図TD";
	}
}
