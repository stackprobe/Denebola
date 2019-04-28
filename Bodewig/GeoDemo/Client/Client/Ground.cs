using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Charlotte
{
	public class Gnd
	{
		private static Gnd _i = null;

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

		public TileCrafter TileCrafter;
		public TileStore TileStore = new TileStore();

		public ByDiskClient ByDiskClient;

		public WearLayer WearLayer = new WearLayer();

		public UserInfoManager UserMgr = new UserInfoManager();

		// ==== ここから MapPanel ====

		public MapPoint CenterXY = new MapPoint(0.0, 0.0);
		public int MeterPerMDot = 1000000; // MPMD_MIN ～ MPMD_MAX
		public int Delta;

		public Point? DownPoint = null;
		public MapPoint DownCenterXY;

		public MapPoint? ClickedPoint = null;

		public class Tile
		{
			public Image Bg;
			public PictureBox Pic; // Bg + wear

			public int MeterPerMDot;
			public int L; // 左座標 == L * TILE_WH * ( MeterPerMDot / 1,000,000 )
			public int T; // 上座標 == T * TILE_WH * ( MeterPerMDot / 1,000,000 )

			public int[] Pic_LTWH = null;

			public TileStore.TSTile TSTile = null;
		}

		public class ActiveTileTable
		{
			public MapPoint CenterXY;
			public int MeterPerMDot;
			public int MapPanelW;
			public int MapPanelH;

			// [0][0] == 左上
			// [W - 1][0] == 右上
			// [0][H - 1] == 左下
			// [W - 1][H - 1] == 右下

			public Tile[][] Table;
			public int L; // == Table[0][0].L
			public int T; // == Table[0][0].T
			public int W; // == Table.Length
			public int H; // == Table[0].Length

			public List<Tile> AddedTiles = new List<Tile>();
			public List<Tile> DeletedTiles = new List<Tile>();
		}

		public ActiveTileTable ActiveTiles = null;

		public int LastMeterPerMDot = -1;
		public int ZoomingCounter = 0;

		// ==== MapPanel ここまで ====

		public AddressDlg AddressDlg = null;

		public bool MapSlided = true;
		public string CenterAddress = "取得中...";
	}
}
