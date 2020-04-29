using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Permissions;
using Charlotte.Tools;
using Charlotte.Utils;
using Charlotte.Layer;
using Charlotte.Layer.MapLayer;

namespace Charlotte
{
	public partial class MainWin : Form
	{
		#region ALT_F4

		private bool XPressed = false;

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
			{
				XPressed = true;
				return;
			}
			base.WndProc(ref m);
		}

		#endregion

		public MainWin()
		{
			InitializeComponent();
		}

		private static bool WroteLog = false;
		private static int ErrorCount = 0;

		private void MainWin_Load(object sender, EventArgs e)
		{
			ProcMain.WriteLog = message =>
			{
				try
				{
					if (message is Exception)
					{
						this.BeginInvoke((MethodInvoker)delegate
						{
							Gnd.I.LastErrorStatus = (++ErrorCount) + ": " + message.GetType() + ": " + ((Exception)message).Message;
						});
					}
				}
				catch
				{ }

				try
				{
					using (new MSection("{74ad097f-784e-4c7b-ae66-b4552cc91bf4}"))
					using (StreamWriter writer = new StreamWriter(@"C:\temp\ZenkokuViewer.log", WroteLog, Encoding.UTF8))
					{
						writer.WriteLine("[" + DateTime.Now + "] " + message);
						WroteLog = true;
					}
				}
				catch
				{ }
			};
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			ProcMain.WriteLog("全国地図_Start");

			for (int index = 0; index < Gnd.I.Layers.Length; index++)
				if (Gnd.I.Layers[index] is MapLayer)
					((MapLayer)Gnd.I.Layers[index]).SetColor_Index(index);

			Gnd.I.ActiveLayers.AddRange(new ILayer[]
			{
				Gnd.I.Layers[3], // BldA
				//Gnd.I.Layers[4], // BldL
				Gnd.I.Layers[8],
				Gnd.I.Layers[11],
				Gnd.I.Layers[12],
				Gnd.I.Layers[13],
				Gnd.I.Layers[17],
				Gnd.I.Layers[20],
			});

			Gnd.I.MainWin = this;

			Gnd.I.BgTh = new BackgroundTh();
			Gnd.I.BgTh.Start();

			// ==== Core ====

			this.MouseWheel += this.MapPictureMouseWheel;
			//this.MapPicture.MouseWheel += MapPictureMouseWheel; // Win7では拾えない。

			// ==== Core ここまで

			this.ChangeMeterPerLatLon();

			// ----

			this.MTEnabled = true;
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.MTEnabled = false;

			// ----

			ProcMain.WriteLog("全国地図_End");
		}

		private void BeforeDialog()
		{
			this.MTEnabled = false;
		}

		private void AfterDialog()
		{
			this.MTEnabled = true;
		}

		private void CloseWindow()
		{
			this.MTEnabled = false;

			// ----

			ProcMain.WriteLog("CloseWindow_Begin");

			BusyDlg.Invoke(new ThreadEx(() =>
			{
				Gnd.I.BgTh.Stop_B();
				Gnd.I.BgTh = null;
			}));

			Gnd.I.MainWin = null;

			ProcMain.WriteLog("CloseWindow_End");

			// ----

			this.Close();
		}

		private bool MTEnabled;
		private bool MTBusy;
		private long MTCount;

		private int ZoomingCounter = 0;

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			if (this.MTEnabled == false || this.MTBusy)
				return;

			this.MTBusy = true;

			try
			{
				if (this.MTCount % 10 == 0)
				{
					SyncGnd.I.UsedMemoryMB.Post(Environment.WorkingSet / 1000000.0);
				}
				if (this.MTCount % 100 == 0)
				{
					GC.Collect();
				}

				if (this.XPressed)
				{
					this.CloseWindow();
					return;
				}

				if (1 <= this.ZoomingCounter)
				{
					this.ZoomingCounter--;
				}
				else
				{
					this.ChangeActiveTiles();
				}
				this.ChangeUIActiveTiles();
				this.ChangeStatus();

				//GC.Collect(); // moved
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);
			}
			finally
			{
				this.MTBusy = false;
				this.MTCount++;
			}
		}

		private void MapPicture_Click(object sender, EventArgs e)
		{
			// noop
		}

		private void MapPicture_MouseDown(object sender, MouseEventArgs e)
		{
			Gnd.I.DownPoint = e.Location;
			Gnd.I.DownCenterPoint = Gnd.I.CenterPoint;
		}

		private void MapPicture_MouseUp(object sender, MouseEventArgs e)
		{
			if (Gnd.I.DownPoint != null)
			{
				int diffX = e.Location.X - Gnd.I.DownPoint.Value.X;
				int diffY = e.Location.Y - Gnd.I.DownPoint.Value.Y;

				Gnd.I.DownPoint = null;

				double diffLat = diffY * (Gnd.I.MeterPerMDot / 1000000.0) / Gnd.I.MeterPerLat * -1;
				double diffLon = diffX * (Gnd.I.MeterPerMDot / 1000000.0) / Gnd.I.MeterPerLon;

				Gnd.I.CenterPoint.Lat = Gnd.I.DownCenterPoint.Lat - diffLat;
				Gnd.I.CenterPoint.Lon = Gnd.I.DownCenterPoint.Lon - diffLon;

				CenterPointChanged();
			}
		}

		private void MapPicture_MouseMove(object sender, MouseEventArgs e)
		{
			if (Gnd.I.DownPoint != null)
			{
				int diffX = e.Location.X - Gnd.I.DownPoint.Value.X;
				int diffY = e.Location.Y - Gnd.I.DownPoint.Value.Y;

				double diffLat = diffY * (Gnd.I.MeterPerMDot / 1000000.0) / Gnd.I.MeterPerLat * -1;
				double diffLon = diffX * (Gnd.I.MeterPerMDot / 1000000.0) / Gnd.I.MeterPerLon;

				Gnd.I.CenterPoint.Lat = Gnd.I.DownCenterPoint.Lat - diffLat;
				Gnd.I.CenterPoint.Lon = Gnd.I.DownCenterPoint.Lon - diffLon;

				CenterPointChanged();
			}
		}

		private void CenterPointChanged()
		{
			Gnd.I.CenterPoint.Lat = DoubleTools.ToRange(Gnd.I.CenterPoint.Lat, Consts.LAT_MIN, Consts.LAT_MAX);
			Gnd.I.CenterPoint.Lon = DoubleTools.ToRange(Gnd.I.CenterPoint.Lon, Consts.LON_MIN, Consts.LON_MAX);
		}

		private void MapPictureMouseWheel(object sender, MouseEventArgs e)
		{
			int dlt = e.Delta;
			int vDlt = DoubleTools.ToInt(Gnd.I.MeterPerMDot * (dlt / 1000.0));

			if (vDlt == 0)
				vDlt = dlt < 0 ? -1 : 1;

			Gnd.I.MeterPerMDot -= vDlt;
			Gnd.I.MeterPerMDot = IntTools.ToRange(Gnd.I.MeterPerMDot, Consts.MPMD_MIN, Consts.MPMD_MAX);

			this.ChangeMeterPerLatLon();

			this.Zooming();
		}

		private void ChangeMeterPerLatLon()
		{
			Gnd.I.MeterPerLat = MeterPerDegree.GetMeterPerDegree_Lat();
			Gnd.I.MeterPerLon = MeterPerDegree.GetMeterPerDegree_Lon(Gnd.I.CenterPoint.Lat);
		}

		private void Zooming()
		{
			this.ZoomingCounter = Consts.ZOOMING_COUNTER_MAX;

			Gnd.I.ChangingUI = true; // ZoomingCounter によって ChangeActiveTiles() が行われなくなるので、ChangeUIActiveTiles() が行われるように。
		}

		private void MapPicture_Resize(object sender, EventArgs e)
		{
			Gnd.I.Changing = true;
		}

		private void ChangeActiveTiles()
		{
			if (
				Gnd.I.ActiveTiles != null &&
				CrashUtils.IsCrashed_Double_Double(Gnd.I.ActiveTiles.MeterPerMDot, Gnd.I.MeterPerMDot) &&
				CrashUtils.IsCrashed_Double_Double(Gnd.I.ActiveTiles.MeterPerLat, Gnd.I.MeterPerLat) &&
				CrashUtils.IsCrashed_Double_Double(Gnd.I.ActiveTiles.MeterPerLon, Gnd.I.MeterPerLon) &&
				CrashUtils.IsCrashed_Point_Point(Gnd.I.ActiveTiles.CenterPoint, Gnd.I.CenterPoint) &&
				Gnd.I.Changing == false
				)
				return;

			Gnd.I.Changing = false;
			Gnd.I.ChangingUI = true;

			int w = this.MapPicture.Width;
			int h = this.MapPicture.Height;

			if (
				w < Consts.MP_WH_MIN ||
				h < Consts.MP_WH_MIN
				)
				return;

			ActiveTileTable activeTiles = new ActiveTileTable();

			activeTiles.MeterPerMDot = Gnd.I.MeterPerMDot;
			activeTiles.MeterPerLat = Gnd.I.MeterPerLat;
			activeTiles.MeterPerLon = Gnd.I.MeterPerLon;
			activeTiles.CenterPoint = Gnd.I.CenterPoint;

			double latPerDot = (Gnd.I.MeterPerMDot / 1000000.0) / Gnd.I.MeterPerLat;
			double lonPerDot = (Gnd.I.MeterPerMDot / 1000000.0) / Gnd.I.MeterPerLon;

			double latMin = Gnd.I.CenterPoint.Lat - (h / 2) * latPerDot;
			double latMax = Gnd.I.CenterPoint.Lat + (h / 2) * latPerDot;
			double lonMin = Gnd.I.CenterPoint.Lon - (w / 2) * lonPerDot;
			double lonMax = Gnd.I.CenterPoint.Lon + (w / 2) * lonPerDot;

			double tileHLat = Consts.TILE_WH * latPerDot;
			double tileWLon = Consts.TILE_WH * lonPerDot;

			long x1 = (long)(lonMin / tileWLon);
			long x2 = (long)(lonMax / tileWLon);
			long y1 = (long)(latMin / tileHLat);
			long y2 = (long)(latMax / tileHLat);

			List<Tile> oldTiles;

			if (
				Gnd.I.ActiveTiles != null &&
				CrashUtils.IsCrashed_Double_Double(Gnd.I.ActiveTiles.MeterPerMDot, Gnd.I.MeterPerMDot) &&
				CrashUtils.IsCrashed_Double_Double(Gnd.I.ActiveTiles.MeterPerLat, Gnd.I.MeterPerLat) &&
				CrashUtils.IsCrashed_Double_Double(Gnd.I.ActiveTiles.MeterPerLon, Gnd.I.MeterPerLon)
				)
				oldTiles = Gnd.I.ActiveTiles.Tiles; // 参照であること。
			else
				oldTiles = new List<Tile>();

			activeTiles.Tiles = new List<Tile>();

			for (long x = x1; x <= x2; x++)
			{
				for (long y = y1; y <= y2; y++)
				{
					int index = CommonUtils.IndexOf(oldTiles, tile => tile.X == x && tile.Y == y);

					if (index == -1)
					{
						Tile tile = new Tile()
						{
							Owner = activeTiles,
							X = x,
							Y = y,
							Bmp = null,
						};

						tile.Added();

						activeTiles.Tiles.Add(tile);
					}
					else
					{
						activeTiles.Tiles.Add(oldTiles[index]);
						oldTiles.RemoveAt(index);
					}
				}
			}

			if (Gnd.I.ActiveTiles != null)
				foreach (Tile tile in Gnd.I.ActiveTiles.Tiles)
					tile.Deleted();

			Gnd.I.ActiveTiles = activeTiles;
		}

		private void ChangeUIActiveTiles()
		{
			if (
				Gnd.I.ActiveTiles == null ||
				Gnd.I.ChangingUI == false
				)
				return;

			Gnd.I.ChangingUI = false;

			int w = this.MapPicture.Width;
			int h = this.MapPicture.Height;

			if (
				w < Consts.MP_WH_MIN ||
				h < Consts.MP_WH_MIN
				)
				return;

			Bitmap bmp = new Bitmap(w, h);

			double latPerDot = (Gnd.I.MeterPerMDot / 1000000.0) / Gnd.I.MeterPerLat;
			double lonPerDot = (Gnd.I.MeterPerMDot / 1000000.0) / Gnd.I.MeterPerLon;

			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.FillRectangle(Brushes.LightSkyBlue, 0, 0, w, h);

				Gnd.I.DrawTileWHCounter.Clear();

				foreach (Tile tile in Gnd.I.ActiveTiles.Tiles)
				{
					double x1 = (w / 2) + (tile.LonMin - Gnd.I.CenterPoint.Lon) / lonPerDot;
					double x2 = (w / 2) + (tile.LonMax - Gnd.I.CenterPoint.Lon) / lonPerDot;
					double y1 = (h / 2) + (tile.LatMin - Gnd.I.CenterPoint.Lat) / latPerDot;
					double y2 = (h / 2) + (tile.LatMax - Gnd.I.CenterPoint.Lat) / latPerDot;

					if (x1 + Consts.DRAW_TILE_WH_MAX < x2)
						continue;

					if (y1 + Consts.DRAW_TILE_WH_MAX < y2)
						continue;

					int l = DoubleTools.ToInt(x1);
					int r = DoubleTools.ToInt(x2);
					int t = DoubleTools.ToInt(h - y2);
					int b = DoubleTools.ToInt(h - y1);

					if (CrashUtils.IsCrashed_Rect_Rect(l, t, r, b, 0, 0, w, h) == false)
						continue;

					int drawTile_w = r - l;
					int drawTile_h = b - t;

					drawTile_w = Math.Max(drawTile_w, Consts.DRAW_TILE_WH_MIN);
					drawTile_h = Math.Max(drawTile_h, Consts.DRAW_TILE_WH_MIN);

					g.DrawImage(tile.Bmp, l, t, drawTile_w, drawTile_h);

					Gnd.I.DrawTileWHCounter.Add(drawTile_w + "_" + drawTile_h);
				}
			}
			this.MapPicture.Image = bmp;
		}

		private void Status_Click(object sender, EventArgs e)
		{
			// noop
		}

		private void ErrorStatus_Click(object sender, EventArgs e)
		{
			// noop
		}

		private void ChangeStatus()
		{
			if (Gnd.I.ActiveTiles == null)
				return;

			List<string> tokens = new List<string>();

			tokens.Add(string.Format("({0}, {1})", Gnd.I.CenterPoint.Lat, Gnd.I.CenterPoint.Lon));
			tokens.Add("" + Gnd.I.MeterPerMDot);
			tokens.Add("" + Gnd.I.ActiveTiles.Tiles.Count);
			tokens.Add("" + Gnd.I.SubTiles.Count);
			tokens.Add("" + Gnd.I.DrawTileWHCounter);
			tokens.Add(SyncGnd.I.UsedMemoryMB.Get().ToString("F6"));

			string text = string.Join(", ", tokens);

			if (this.Status.Text != text)
				this.Status.Text = text;

			text = Gnd.I.LastErrorStatus;

			if (this.ErrorStatus.Text != text)
				this.ErrorStatus.Text = text;
		}

		private void MapPanelMenu_Opening(object sender, CancelEventArgs e)
		{
			// noop
		}

		private void ChangeMeterPerLatLonMenuItem_Click(object sender, EventArgs e)
		{
			this.ChangeMeterPerLatLon();

			this.Zooming();
		}
	}
}
