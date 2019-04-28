using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Charlotte.Tools;
using Charlotte.Utils;
using Charlotte.Layer;
using Charlotte.Layer.MapLayer;
using Charlotte.Layer.BackgroundLayer;

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

		// ==== Core ====

		public int MeterPerMDot = 1000000;
		public double MeterPerLat = 108000.0;
		public double MeterPerLon = 108000.0;
		public GeoPoint CenterPoint = new GeoPoint(35.6652, 139.7125);
		public Point? DownPoint = null;
		public GeoPoint DownCenterPoint;
		public bool Changing = false;
		public bool ChangingUI = false;

		public ActiveTileTable ActiveTiles = null;

		// ==== Core ここまで

		public MainWin MainWin = null;
		public BackgroundTh BgTh = null;
		public KeyCounter DrawTileWHCounter = new KeyCounter();
		public string LastErrorStatus = "";

		public ILayer[] Layers = new ILayer[]
		{
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "AdmArea")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "AdmBdry")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "AdmPt")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "BldA")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "BldL")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "Cntr")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "CommBdry")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "CommPt")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "Cstline")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "ElevPt")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "GCP")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "RailCL")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "RdCompt")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "RdEdg")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "SBAPt")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "SBBdry")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "WA")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "WL")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "WStrA")),
			new MapLayer(Path.Combine(Consts.LAYER_ROOT_DIR, "WStrL")),
			new BackgroundLayer(Color.White),
		};

		public List<ILayer> ActiveLayers = new List<ILayer>();

		public List<SubTile> SubTiles = new List<SubTile>();
	}
}
