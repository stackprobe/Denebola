using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Charlotte.Tools;
using Charlotte.Utils;

namespace Charlotte
{
	public partial class MainWin : Form
	{
		public MainWin()
		{
			InitializeComponent();
		}

		private static bool WroteLog;

		private void MainWin_Load(object sender, EventArgs e)
		{
			ProcMain.WriteLog = message =>
			{
				try
				{
					using (new MSection("{2cb2103a-d1c1-489c-9cae-7b8116cfcb86}"))
					using (StreamWriter writer = new StreamWriter(ProcMain.SelfFile + ".log", WroteLog, Encoding.UTF8))
					{
						writer.WriteLine(DateTime.Now.ToString("[yyyy/MM/dd hh:mm:ss.fff] ") + JString.ToJString("" + message, true, true, true, true));
						WroteLog = true;
					}
				}
				catch
				{ }
			};
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			ProcMain.WriteLog("GeoDemo_Client Start");

			{
				string file = "Config.xml";

				if (File.Exists(file))
				{
					Config config = new Config();

					config.LoadFile(file);
					config.ValidationCheck();

					Gnd.I.Config = config;
				}
			}

			ProcMain.WriteLog("TileCrafter Starting...");

			Gnd.I.TileCrafter = new TileCrafter();
			Gnd.I.TileCrafter.Start();

			ProcMain.WriteLog("TileCrafter Started");

			Gnd.I.ByDiskClient = new ByDiskClient();

			ProcMain.WriteLog("ByDiskClient Started");

			for (int index = 0; index < Consts.LAYER_NUM; index++) // レイヤ_メニュー
			{
				int f_index = index;

				this.MapPanelLayerMenu.DropDownItems.Add("[" + index.ToString("D2") + "] " + Consts.LAYER_NAMES[index]);

				ToolStripMenuItem item = (ToolStripMenuItem)this.MapPanelLayerMenu.DropDownItems[index];

				item.Checked = Gnd.I.Config.VisibleLayerFlags[index];
				item.Click += (s, ev) =>
				{
					Gnd.I.Config.VisibleLayerFlags[f_index] =
						Gnd.I.Config.VisibleLayerFlags[f_index] == false;

					this.UIRefresh();
				};
			}

			Gnd.I.UserMgr.LoadFile();

			// ==== ここから MapPanel ====

			this.MouseWheel += this.MapPanelMouseWheel;

			this.MapPanel.KeyPress += this.MapPanelKeyPress;

			// ==== MapPanel ここまで ====

			this.UIRefresh();

			// ----

			this.MTEnabled = true;
		}

		private void タイルデータをローカルディスク経由で取得するToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.TileCrafter_ByDisk =
				Gnd.I.Config.TileCrafter_ByDisk == false;

			this.UIRefresh();
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.MTEnabled = false;

			// ----

			if (Gnd.I.AddressDlg != null)
			{
				Gnd.I.AddressDlg.Close();
				Gnd.I.AddressDlg = null;
			}

			ProcMain.WriteLog("TileCrafter Ending...");

			Gnd.I.TileCrafter.Stop();
			Gnd.I.TileCrafter = null;

			ProcMain.WriteLog("TileCrafter Ended");

			Gnd.I.ByDiskClient.Dispose();
			Gnd.I.ByDiskClient = null;

			ProcMain.WriteLog("GeoDemo_Client End");
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

			// -- 9000

			// ----

			this.Close();
		}

		private bool MTEnabled;
		private bool MTBusy;
		private long MTCount;

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			if (this.MTEnabled == false || this.MTBusy)
				return;

			this.MTBusy = true;

			try
			{
				if (Gnd.I.Delta != 0)
				{
					int vDlt = DoubleTools.ToInt(Gnd.I.MeterPerMDot * (Gnd.I.Delta / 1000.0));

					if (vDlt == 0)
						vDlt = Gnd.I.Delta < 0 ? -1 : 1;

					Gnd.I.MeterPerMDot -= vDlt;
					Gnd.I.MeterPerMDot = IntTools.ToRange(Gnd.I.MeterPerMDot, Consts.MPMD_MIN, Consts.MPMD_MAX);

					Gnd.I.Delta = 0;
				}
				this.Tiling();
				//this.RefreshStatus(); // moved

				if (Gnd.I.MapSlided)
				{
					Gnd.I.MapSlided = false;
					Gnd.I.TileStore.XYToAddress.SetPoint(Gnd.I.CenterXY);
				}

				{
					string address = Gnd.I.TileStore.XYToAddress.TakeAddress();

					if (address != null)
					{
						Gnd.I.CenterAddress = address;
					}
				}

				this.RefreshStatus();

				{
					MapPoint? point = Gnd.I.TileStore.AddressToXY.TakePoint();

					if (point != null)
					{
						this.ChangeCenterXY(point.Value);
					}
				}

				Gnd.I.TileStore.RouteJob.RegularInvoke();
				Gnd.I.TileStore.GoogleMapJob.RegularInvoke();
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

		private void MapPanel_Paint(object sender, PaintEventArgs e)
		{
			// noop
		}

		private void MapPanel_MouseDown(object sender, MouseEventArgs e)
		{
			Gnd.I.DownPoint = e.Location;
			Gnd.I.DownCenterXY = Gnd.I.CenterXY;
		}

		private void MapPanel_MouseUp(object sender, MouseEventArgs e)
		{
			if (
				Gnd.I.DownPoint != null &&
				Gnd.I.DownPoint.Value.X == e.Location.X &&
				Gnd.I.DownPoint.Value.Y == e.Location.Y &&
				e.Button == MouseButtons.Left
				)
			{
				int mX = e.Location.X - (this.MapPanel.Width / 2);
				int mY = e.Location.Y - (this.MapPanel.Height / 2);

				Gnd.I.ClickedPoint = new MapPoint(
					Gnd.I.CenterXY.X + mX * (Gnd.I.MeterPerMDot / 1000000.0),
					Gnd.I.CenterXY.Y + mY * (Gnd.I.MeterPerMDot / 1000000.0)
					);

				// -- マップの上を左クリックした。

				{
					MapPoint? nearestHitUserPoint = GetNearestHitUserPoint(Gnd.I.ClickedPoint.Value);

					if (nearestHitUserPoint != null)
					{
						Gnd.I.WearLayer.AddPoint(nearestHitUserPoint.Value);
					}
					else
					{
						Gnd.I.WearLayer.AddPoint(Gnd.I.ClickedPoint.Value);
					}
				}

				this.UIRefresh_WearLayer();
			}
			Gnd.I.DownPoint = null;
		}

		private MapPoint? GetNearestHitUserPoint(MapPoint point)
		{
			MapPoint? ret = null;
			double ret_d = double.MaxValue;

			foreach (MapPoint userPoint in Gnd.I.UserMgr.Users.Where(user => user.Hit).Select(user => user.Point))
			{
				double d = CrashUtils.GetDistance(userPoint, point);

				if (d < ret_d)
				{
					ret = userPoint;
					ret_d = d;
				}
			}
			return ret;
		}

		private void MapPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (Gnd.I.DownPoint != null)
			{
				int mX = e.Location.X - Gnd.I.DownPoint.Value.X;
				int mY = e.Location.Y - Gnd.I.DownPoint.Value.Y;

				this.ChangeCenterXY(new MapPoint(
					Gnd.I.DownCenterXY.X - mX * (Gnd.I.MeterPerMDot / 1000000.0),
					Gnd.I.DownCenterXY.Y - mY * (Gnd.I.MeterPerMDot / 1000000.0)
					));
			}

			// FIXME テキストボックスにフォーカスが当たったままだと、ホイールが効かなくなる模様。Win7のみだろうか？
			if (this.MapPanel.Focused == false)
				this.MapPanel.Focus();
		}

		private void ChangeCenterXY(MapPoint point)
		{
			Gnd.I.CenterXY = point;

			// Range
			{
				double x = Gnd.I.CenterXY.X;
				double y = Gnd.I.CenterXY.Y;

				x = DoubleTools.ToRange(x, Consts.X_MIN, Consts.X_MAX);
				y = DoubleTools.ToRange(y, Consts.Y_MIN, Consts.Y_MAX);

				Gnd.I.CenterXY.X = x;
				Gnd.I.CenterXY.Y = y;
			}

			Gnd.I.MapSlided = true;
		}

		private void MapPanelMouseWheel(object sender, MouseEventArgs e)
		{
			Gnd.I.Delta += e.Delta;
		}

		private void MapPanelKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13) // enter
			{
				// noop
				e.Handled = true;
			}
			else if (e.KeyChar == (char)43) // '+'
			{
				Gnd.I.WearLayer.HighlightRouteIndex = Math.Min(Gnd.I.WearLayer.HighlightRouteIndex + 1, Gnd.I.WearLayer.GetRoutes().Length - 1);
				this.UIRefresh_WearLayer();
				e.Handled = true;
			}
			else if (e.KeyChar == (char)45) // '-'
			{
				Gnd.I.WearLayer.HighlightRouteIndex = Math.Max(Gnd.I.WearLayer.HighlightRouteIndex - 1, -1);
				this.UIRefresh_WearLayer();
				e.Handled = true;
			}
			else if (e.KeyChar == (char)42) // '*'
			{
				try
				{
					BestView bv = new BestView();

					if (0 <= Gnd.I.WearLayer.HighlightRouteIndex)
					{
						Route route = Gnd.I.WearLayer.GetRoutes()[Gnd.I.WearLayer.HighlightRouteIndex];

						bv.AddPoint(route.StartPoint);
						bv.AddPoint(route.EndPoint);

						foreach (MapPoint point in route.RoutePoints)
						{
							bv.AddPoint(point);
						}
					}
					else if (1 <= Gnd.I.WearLayer.GetAroundCircles().Length)
					{
						foreach (AroundCircle ac in Gnd.I.WearLayer.GetAroundCircles())
						{
							MapPoint nw = new MapPoint(ac.Point.X - ac.R, ac.Point.Y - ac.R);
							MapPoint se = new MapPoint(ac.Point.X + ac.R, ac.Point.Y + ac.R);

							bv.AddPoint(nw);
							bv.AddPoint(se);
						}
					}
					else if (1 <= Gnd.I.WearLayer.GetRoutes().Length)
					{
						foreach (Route route in Gnd.I.WearLayer.GetRoutes())
						{
							bv.AddPoint(route.StartPoint);
							bv.AddPoint(route.EndPoint);

							foreach (MapPoint point in route.RoutePoints)
							{
								bv.AddPoint(point);
							}
						}
					}
					else if (1 <= Gnd.I.WearLayer.GetPoints().Length)
					{
						foreach (MapPoint point in Gnd.I.WearLayer.GetPoints())
						{
							bv.AddPoint(point);
						}
					}
					else
					{
						throw new Exception("ズーム基準無し");
					}

					bv.SetMapPanel(this.MapPanel);

					bv.Invoke();

					Gnd.I.CenterXY.X = bv.CenterX;
					Gnd.I.CenterXY.Y = bv.CenterY;
					Gnd.I.MeterPerMDot = bv.MeterPerMDot;
					Gnd.I.LastMeterPerMDot = bv.MeterPerMDot; // 直ちに画面更新するため、ZoomingCounter待ちをさせないため。

					Gnd.I.MapSlided = true;
				}
				catch (Exception ex)
				{
					ProcMain.WriteLog(ex);
				}
				e.Handled = true;
			}
		}

		private void Tiling()
		{
			// ---- タイル更新と、その抑止 ----

			if (Gnd.I.LastMeterPerMDot != Gnd.I.MeterPerMDot)
			{
				Gnd.I.LastMeterPerMDot = Gnd.I.MeterPerMDot;
				Gnd.I.ZoomingCounter = Consts.ZOOMING_COUNTER_MAX;
			}
			else if (1 <= Gnd.I.ZoomingCounter)
			{
				Gnd.I.ZoomingCounter--;
			}
			else
			{
				this.UpdateActiveTiles();
			}

			// ---- タイルの再配置 ----

			if (Gnd.I.ActiveTiles != null)
			{
				this.ChangeUIActiveTiles();
			}

			// ----

			GC.Collect();
		}

		private void UpdateActiveTiles()
		{
			// ? 中心座標＆ズーム＆ウィンドウサイズ変わっていない。-> 更新不要
			if (
				Gnd.I.ActiveTiles != null &&
				CrashUtils.IsCrashed_Point_Point(
					Gnd.I.ActiveTiles.CenterXY.X,
					Gnd.I.ActiveTiles.CenterXY.Y,
					Gnd.I.CenterXY.X,
					Gnd.I.CenterXY.Y
					) &&
				Gnd.I.ActiveTiles.MeterPerMDot == Gnd.I.MeterPerMDot &&
				Gnd.I.ActiveTiles.MapPanelW == this.MapPanel.Width &&
				Gnd.I.ActiveTiles.MapPanelH == this.MapPanel.Height
				)
				return;

			double x1 = Gnd.I.CenterXY.X - (this.MapPanel.Width / 2) * (Gnd.I.MeterPerMDot / 1000000.0); // MapPanel 左
			double x2 = Gnd.I.CenterXY.X + (this.MapPanel.Width / 2) * (Gnd.I.MeterPerMDot / 1000000.0); // MapPanel 右
			double y1 = Gnd.I.CenterXY.Y - (this.MapPanel.Height / 2) * (Gnd.I.MeterPerMDot / 1000000.0); // MapPanel 上
			double y2 = Gnd.I.CenterXY.Y + (this.MapPanel.Height / 2) * (Gnd.I.MeterPerMDot / 1000000.0); // MapPanel 下

			int l = GetTileLT(x1, Gnd.I.MeterPerMDot);
			int r = GetTileLT(x2, Gnd.I.MeterPerMDot);
			int t = GetTileLT(y1, Gnd.I.MeterPerMDot);
			int b = GetTileLT(y2, Gnd.I.MeterPerMDot);
			int w = (r - l) + 1;
			int h = (b - t) + 1;

			if (w < 1) throw null; // test
			if (h < 1) throw null; // test

			Gnd.ActiveTileTable activeTilesNew = new Gnd.ActiveTileTable();

			activeTilesNew.CenterXY = Gnd.I.CenterXY;
			activeTilesNew.MeterPerMDot = Gnd.I.MeterPerMDot;
			activeTilesNew.MapPanelW = this.MapPanel.Width;
			activeTilesNew.MapPanelH = this.MapPanel.Height;

			activeTilesNew.Table = new Gnd.Tile[w][];
			activeTilesNew.L = l;
			activeTilesNew.T = t;
			activeTilesNew.W = w;
			activeTilesNew.H = h;

			for (int x = 0; x < w; x++)
				activeTilesNew.Table[x] = new Gnd.Tile[h];

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					int tileL = l + x;
					int tileT = t + y;

					Gnd.Tile tile = TakeTile(tileL, tileT, Gnd.I.MeterPerMDot, Gnd.I.ActiveTiles);

					if (tile == null)
					{
						tile = CreateTile(tileL, tileT, Gnd.I.MeterPerMDot);
						activeTilesNew.AddedTiles.Add(tile);
					}
					activeTilesNew.Table[x][y] = tile;
				}
			}
			if (Gnd.I.ActiveTiles != null)
				for (int x = 0; x < Gnd.I.ActiveTiles.W; x++)
					for (int y = 0; y < Gnd.I.ActiveTiles.H; y++)
						if (Gnd.I.ActiveTiles.Table[x][y] != null)
							activeTilesNew.DeletedTiles.Add(Gnd.I.ActiveTiles.Table[x][y]);

			Gnd.I.ActiveTiles = activeTilesNew;
		}

		private Gnd.Tile CreateTile(int tileL, int tileT, int tileMeterPerMDot)
		{
			Gnd.Tile tile = new Gnd.Tile();

			tile.Pic = new PictureBox();
			//tile.Pic.Image = CreateTilePic(tileL, tileT, tileMeterPerMDot); // old
			tile.Pic.SizeMode = PictureBoxSizeMode.StretchImage;

			tile.MeterPerMDot = tileMeterPerMDot;
			tile.L = tileL;
			tile.T = tileT;

			tile.Bg = CreateTilePic(tileL, tileT, tileMeterPerMDot);
			tile.Pic.Image = Gnd.I.WearLayer.GetWearedTilePic(tile.Bg, tile);

			// イベント

			tile.Pic.MouseDown += (sender, e) =>
			{
				MouseEventArgs mea = new MouseEventArgs(
					e.Button,
					0,
					tile.Pic.Left + e.Location.X,
					tile.Pic.Top + e.Location.Y,
					0
					);

				MapPanel_MouseDown(null, mea);
			};

			tile.Pic.MouseUp += (sender, e) =>
			{
				MouseEventArgs mea = new MouseEventArgs(
					e.Button,
					0,
					tile.Pic.Left + e.Location.X,
					tile.Pic.Top + e.Location.Y,
					0
					);

				MapPanel_MouseUp(null, mea);
			};

			tile.Pic.MouseMove += (sender, e) =>
			{
				MouseEventArgs mea = new MouseEventArgs(
					e.Button,
					0,
					tile.Pic.Left + e.Location.X,
					tile.Pic.Top + e.Location.Y,
					0
					);

				MapPanel_MouseMove(null, mea);
			};

			// ホイールのイベントは拾ってくれる。

			return tile;
		}

		private Image CreateTilePic(int tileL, int tileT, int tileMeterPerMDot) // ダミーパネル
		{
			double x1 = (tileL + 0) * (Consts.TILE_WH * (tileMeterPerMDot / 1000000.0));
			double x2 = (tileL + 1) * (Consts.TILE_WH * (tileMeterPerMDot / 1000000.0));
			double y1 = (tileT + 0) * (Consts.TILE_WH * (tileMeterPerMDot / 1000000.0));
			double y2 = (tileT + 1) * (Consts.TILE_WH * (tileMeterPerMDot / 1000000.0));

			Bitmap bmp = new Bitmap(Consts.TILE_WH, Consts.TILE_WH);
			Pen pen = new Pen(Color.Gray, 1f);

			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.Clear(Color.White);
				g.DrawLine(pen, 0f, 0f, Consts.TILE_WH - 1f, 0f);
				g.DrawLine(pen, 0f, 0f, 0f, Consts.TILE_WH - 1f);
				g.DrawLine(pen, Consts.TILE_WH - 1f, 0f, Consts.TILE_WH - 1f, Consts.TILE_WH - 1f);
				g.DrawLine(pen, 0f, Consts.TILE_WH - 1f, Consts.TILE_WH - 1f, Consts.TILE_WH - 1f);
				g.DrawString(
					"(" + x1 + ", " + y1 + ")\r\n(" + x2 + ", " + y2 + ")",
					new Font("メイリオ", 10f, FontStyle.Regular),
					Brushes.Blue,
					1f,
					1f
					);
			}
			return bmp;
		}

		private Gnd.Tile TakeTile(int tileL, int tileT, int tileMeterPerMDot, Gnd.ActiveTileTable src)
		{
			if (
				src != null &&
				src.MeterPerMDot == tileMeterPerMDot &&
				src.L <= tileL && tileL < src.L + src.W &&
				src.T <= tileT && tileT < src.T + src.H
				)
			{
				int x = tileL - src.L;
				int y = tileT - src.T;

				Gnd.Tile tile = src.Table[x][y];

				src.Table[x][y] = null;

				if (tile.L != tileL) throw null; // test
				if (tile.T != tileT) throw null; // test

				return tile;
			}
			return null;
		}

		private int GetTileLT(double p, int meterPerMDot)
		{
			bool neg = false;

			if (p < 0.0)
			{
				p = -p;
				neg = true;
			}
			double meterTileWH = meterPerMDot * (Consts.TILE_WH / 1000000.0);
			int index = (int)(p / meterTileWH);

			if (index < 0) throw null; // test

			if (neg)
				index = -1 - index;

			return index;
		}

		private void ChangeUIActiveTiles()
		{
			foreach (Gnd.Tile tile in Gnd.I.ActiveTiles.DeletedTiles)
			{
				this.MapPanel.Controls.Remove(tile.Pic);
				this.TileRemoved(tile);
			}
			Gnd.I.ActiveTiles.DeletedTiles.Clear();

			// ---- 再配置 ----

			double x1 = Gnd.I.CenterXY.X - (this.MapPanel.Width / 2) * (Gnd.I.MeterPerMDot / 1000000.0); // MapPanel 左
			double x2 = Gnd.I.CenterXY.X + (this.MapPanel.Width / 2) * (Gnd.I.MeterPerMDot / 1000000.0); // MapPanel 右
			double y1 = Gnd.I.CenterXY.Y - (this.MapPanel.Height / 2) * (Gnd.I.MeterPerMDot / 1000000.0); // MapPanel 上
			double y2 = Gnd.I.CenterXY.Y + (this.MapPanel.Height / 2) * (Gnd.I.MeterPerMDot / 1000000.0); // MapPanel 下

			for (int x = 0; x < Gnd.I.ActiveTiles.W; x++)
			{
				for (int y = 0; y < Gnd.I.ActiveTiles.H; y++)
				{
					Gnd.Tile tile = Gnd.I.ActiveTiles.Table[x][y];

					double tx1 = (tile.L + 0) * (Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0));
					double tx2 = (tile.L + 1) * (Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0));
					double ty1 = (tile.T + 0) * (Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0));
					double ty2 = (tile.T + 1) * (Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0));

					double dL = (tx1 - x1) * this.MapPanel.Width / (x2 - x1);
					double dR = (tx2 - x1) * this.MapPanel.Width / (x2 - x1);
					double dT = (ty1 - y1) * this.MapPanel.Height / (y2 - y1);
					double dB = (ty2 - y1) * this.MapPanel.Height / (y2 - y1);
					double dW = dR - dL;
					double dH = dB - dT;

					bool fault = false;

					if (dW < 10.0) // ? 幅が狭すぎる。
					{
						dW = 10.0;
					}
					else if (this.MapPanel.Width * 2.0 < dW) // ? 幅が広すぎる。
					{
						fault = true;
					}
					if (dH < 10.0) // ? 高さが狭すぎる。
					{
						dH = 10.0;
					}
					else if (this.MapPanel.Height * 2.0 < dH) // ? 高さが広すぎる。
					{
						fault = true;
					}

					int[] ltwh;

					if (fault)
					{
						ltwh = new int[] { -300, -300, 200, 200 };
					}
					else
					{
						int iL = DoubleTools.ToInt(dL);
						int iR = DoubleTools.ToInt(dR);
						int iT = DoubleTools.ToInt(dT);
						int iB = DoubleTools.ToInt(dB);
						int iW = iR - iL;
						int iH = iB - iT;

						if (iW < 1) throw null; // test
						if (iH < 1) throw null; // test

						ltwh = new int[] { iL, iT, iW, iH };
					}

					if (
						tile.Pic_LTWH != null &&
						tile.Pic_LTWH[0] == ltwh[0] &&
						tile.Pic_LTWH[1] == ltwh[1] &&
						tile.Pic_LTWH[2] == ltwh[2] &&
						tile.Pic_LTWH[3] == ltwh[3]
						)
					{
						// noop -- 位置が変わっていない。
					}
					else
					{
						tile.Pic.Left = ltwh[0];
						tile.Pic.Top = ltwh[1];
						tile.Pic.Width = ltwh[2];
						tile.Pic.Height = ltwh[3];

						tile.Pic_LTWH = ltwh;
					}
				}
			}

			// 再配置ここまで

			foreach (Gnd.Tile tile in Gnd.I.ActiveTiles.AddedTiles)
			{
				this.MapPanel.Controls.Add(tile.Pic);
				this.TileAdded(tile);
			}
			Gnd.I.ActiveTiles.AddedTiles.Clear();

			for (int x = 0; x < Gnd.I.ActiveTiles.W; x++)
			{
				for (int y = 0; y < Gnd.I.ActiveTiles.H; y++)
				{
					Gnd.Tile tile = Gnd.I.ActiveTiles.Table[x][y];

					this.TileInterlude(tile);
				}
			}
		}

		private void UIRefresh_WearLayer()
		{
			if (Gnd.I.ActiveTiles != null)
			{
				for (int x = 0; x < Gnd.I.ActiveTiles.W; x++)
				{
					for (int y = 0; y < Gnd.I.ActiveTiles.H; y++)
					{
						Gnd.Tile tile = Gnd.I.ActiveTiles.Table[x][y];

						tile.Pic.Image = Gnd.I.WearLayer.GetWearedTilePic(tile.Bg, tile);
					}
				}
			}
		}

		private void UIRefresh(bool quickMode = false)
		{
			for (int index = 0; index < Consts.LAYER_NUM; index++)
			{
				ToolStripMenuItem item = (ToolStripMenuItem)this.MapPanelLayerMenu.DropDownItems[index];

				item.Checked = Gnd.I.Config.VisibleLayerFlags[index];
			}
			this.タイルデータをローカルディスク経由で取得するToolStripMenuItem.Checked = Gnd.I.Config.TileCrafter_ByDisk;
			this.中心からタイリングするToolStripMenuItem.Checked = Gnd.I.Config.TileCrafter_PriorityToCenter;
			this.道路を表示するToolStripMenuItem.Checked = Gnd.I.Config.TileCrafter_Road;
			this.住所を表示するToolStripMenuItem.Checked = Gnd.I.Config.ShowCenterAddress;
			this.ルートの色MenuItem_Aqua.Checked = CommonUtils.IsSame(Gnd.I.Config.RouteColor, Color.Aqua);
			this.ルートの色MenuItem_OrangeRed.Checked = CommonUtils.IsSame(Gnd.I.Config.RouteColor, Color.OrangeRed);
			this.顧客を表示するToolStripMenuItem.Checked = Gnd.I.Config.ShowUser;
			this.顧客の色MenuItem_DarkRed.Checked = CommonUtils.IsSame(Gnd.I.Config.UserColor, Color.DarkRed);
			this.顧客の色MenuItem_Magenta.Checked = CommonUtils.IsSame(Gnd.I.Config.UserColor, Color.Magenta);
			this.顧客の色MenuItem_YellowGreen.Checked = CommonUtils.IsSame(Gnd.I.Config.UserColor, Color.YellowGreen);
			this.ヒットした顧客の色MenuItem_DarkRed.Checked = CommonUtils.IsSame(Gnd.I.Config.HitUserColor, Color.DarkRed);
			this.ヒットした顧客の色MenuItem_Magenta.Checked = CommonUtils.IsSame(Gnd.I.Config.HitUserColor, Color.Magenta);
			this.ヒットした顧客の色MenuItem_YellowGreen.Checked = CommonUtils.IsSame(Gnd.I.Config.HitUserColor, Color.YellowGreen);

			if (quickMode)
			{
				this.UIRefresh_WearLayer();
			}
			else
			{
				this.ReregisterAllTile();
				//this.UIRefresh_WearLayer(); // タイルの再読込により更新されるので不要。
			}
		}

		private void ReregisterAllTile()
		{
			if (Gnd.I.ActiveTiles == null)
				return;

			Gnd.I.TileStore.RemoveAllTSTile();

			for (int x = 0; x < Gnd.I.ActiveTiles.W; x++)
			{
				for (int y = 0; y < Gnd.I.ActiveTiles.H; y++)
				{
					Gnd.Tile tile = Gnd.I.ActiveTiles.Table[x][y];

					this.TileRegister(tile);
				}
			}
		}

		// ---- Tile* ----

		private void TileInterlude(Gnd.Tile tile)
		{
			if (tile.TSTile != null)
			{
				Image img = tile.TSTile.GetTilePic();

				if (img != null)
				{
					tile.Bg = img;
					tile.Pic.Image = Gnd.I.WearLayer.GetWearedTilePic(img, tile);

					Gnd.I.TileStore.RemoveTSTile(tile.TSTile);
					tile.TSTile = null;
				}
			}
		}

		private void TileAdded(Gnd.Tile tile)
		{
			TileRegister(tile);
		}

		private void TileRegister(Gnd.Tile tile)
		{
			tile.TSTile = new TileStore.TSTile()
			{
				L = (tile.L + 0) * (Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0)),
				T = (tile.T + 0) * (Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0)),
				R = (tile.L + 1) * (Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0)),
				B = (tile.T + 1) * (Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0)),
				BmpW = Consts.TILE_WH,
				BmpH = Consts.TILE_WH,
				//LayerOn = "10001000100010001000", // 暫定
				//LayerOn = "10101010101010101010", // 暫定
				//LayerOn = "11111111111111111111", // 暫定
				LayerOn = Gnd.I.Config.GetLayerOn(),
				Road = Gnd.I.Config.TileCrafter_Road,
			};

			Gnd.I.TileStore.AddTSTile(tile.TSTile);
		}

		private void TileRemoved(Gnd.Tile tile)
		{
			if (tile.TSTile != null)
			{
				Gnd.I.TileStore.RemoveTSTile(tile.TSTile);
				tile.TSTile = null;
			}
		}

		// ---- Tile* ここまで

		private void Status_TextChanged(object sender, EventArgs e)
		{
			// noop
		}

		private void RefreshStatus()
		{
			List<string> dest = new List<string>();

			dest.Add(CommonUtils.ToString(Gnd.I.CenterXY));
			dest.Add("" + Gnd.I.MeterPerMDot);
			dest.Add(CommonUtils.ToString(Gnd.I.DownPoint));
			dest.Add(CommonUtils.ToString(Gnd.I.ClickedPoint));
			dest.Add(Gnd.I.Config.GetLayerOn());
			dest.Add("" + Gnd.I.Config.TileCrafter_Road);
			dest.Add(this.MapPanel.Controls.Count + " (" + Gnd.I.TileStore.GetTSTileCount() + ")");
			dest.AddRange(Gnd.I.CenterAddress.Split(':'));
			//dest.Add(Gnd.I.CenterAddress); // old
			dest.Add("顧客数 " + Gnd.I.UserMgr.Users.Count);
			dest.AddRange(Gnd.I.WearLayer.GetStatusLines());

			string text = string.Join("\r\n", dest);

			if (this.Status.Text != text)
			{
				this.Status.Text = text;
				this.Status.SelectionStart = text.Length;
			}
		}

		private void Status_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)1) // ctrl_a
			{
				this.Status.SelectAll();
				e.Handled = true;
			}
		}

		private void MapPanelMenu_Opening(object sender, CancelEventArgs e)
		{
			// noop
		}

		private void NoopMenuItem_Click(object sender, EventArgs e)
		{
			// noop
		}

		private void フルToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.SetLayerOn(Consts.LAYER_ON_PRESET_FULL);
			this.UIRefresh();
		}

		private void 軽量ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.SetLayerOn(Consts.LAYER_ON_PRESET_LITE);
			this.UIRefresh();
		}

		private void 無しToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.SetLayerOn(Consts.LAYER_ON_PRESET_NONE);
			this.UIRefresh();
		}

		private void 中心からタイリングするToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.TileCrafter_PriorityToCenter =
				Gnd.I.Config.TileCrafter_PriorityToCenter == false;

			this.UIRefresh();
		}

		private void 道路を表示するToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.TileCrafter_Road =
				Gnd.I.Config.TileCrafter_Road == false;

			this.UIRefresh();
		}

		private void 住所を表示するToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.ShowCenterAddress =
				Gnd.I.Config.ShowCenterAddress == false;

			this.UIRefresh();
		}

		private void 住所DlgToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Gnd.I.AddressDlg != null)
				return;

			Gnd.I.AddressDlg = new AddressDlg();
			Gnd.I.AddressDlg.Show();
		}

		private void ルートの色MenuItem_Aqua_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.RouteColor = Color.Aqua;
			this.UIRefresh(true);
		}

		private void ルートの色MenuItem_OrangeRed_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.RouteColor = Color.OrangeRed;
			this.UIRefresh(true);
		}

		private void 顧客を表示するToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.ShowUser =
				Gnd.I.Config.ShowUser == false;

			this.UIRefresh(true);
		}

		private void 顧客の色MenuItem_DarkRed_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.UserColor = Color.DarkRed;
			this.UIRefresh(true);
		}

		private void 顧客の色MenuItem_Magenta_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.UserColor = Color.Magenta;
			this.UIRefresh(true);
		}

		private void 顧客の色MenuItem_YellowGreen_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.UserColor = Color.YellowGreen;
			this.UIRefresh(true);
		}

		private void ヒットした顧客の色MenuItem_DarkRed_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.HitUserColor = Color.DarkRed;
			this.UIRefresh(true);
		}

		private void ヒットした顧客の色MenuItem_Magenta_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.HitUserColor = Color.Magenta;
			this.UIRefresh(true);
		}

		private void ヒットした顧客の色MenuItem_YellowGreen_Click(object sender, EventArgs e)
		{
			Gnd.I.Config.HitUserColor = Color.YellowGreen;
			this.UIRefresh(true);
		}

		private void リフレッシュToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.MapSlided = true;

			this.UIRefresh();
		}

		private void リフレッシュクイックToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.UIRefresh_WearLayer();
		}

		private void プロットクリアToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.WearLayer.Clear();

			this.UIRefresh_WearLayer();
		}

		private void プロットクリアOneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.WearLayer.Clear_LastOne();

			this.UIRefresh_WearLayer();
		}

		private void ルート検索ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				MapPoint[] points = Gnd.I.WearLayer.GetPoints();

				if (points.Length < 2)
					throw new Exception("2箇所以上プロットして下さい。");

				if (points.Length == 2)
				{
					Route resRoute = null;

					Gnd.I.TileStore.RouteJob.SetJob(
						() =>
						{
							HTTPClient hc = new HTTPClient(Gnd.I.Config.ServerUrlPrefix + string.Join(":",
								"route",
								points[0].X.ToString("F6"),
								points[0].Y.ToString("F6"),
								points[1].X.ToString("F6"),
								points[1].Y.ToString("F6")
								));

							hc.Get();

							string[] rets = Encoding.ASCII.GetString(hc.ResBody).Split(':');
							int c = 0;

							Route route = new Route();

							route.StartPointName = CommonUtils.PlotIndexToName(0);
							route.EndPointName = CommonUtils.PlotIndexToName(1);
							route.StartPoint = points[0];
							route.EndPoint = points[1];
							route.RouteDistance = double.Parse(rets[c++]);

							{
								List<MapPoint> rtPts = new List<MapPoint>();

								while (c < rets.Length)
								{
									string[] tokens = rets[c++].Split(' ');

									double x = double.Parse(tokens[0]);
									double y = double.Parse(tokens[1]);

									rtPts.Add(new MapPoint(x, y));
								}
								route.RoutePoints = rtPts.ToArray();
							}

							resRoute = route;
						},
						() =>
						{
							if (resRoute != null)
							{
								Gnd.I.WearLayer.ClearRoutes();
								Gnd.I.WearLayer.AddRoute(resRoute);

								this.UIRefresh_WearLayer();
							}
						}
						);
				}
				else
				{
					Route[] resRoutes = null;

					Gnd.I.TileStore.RouteJob.SetJob(
						() =>
						{
							List<string> prms = new List<string>();

							prms.Add("multi-route");

							foreach (MapPoint point in points)
							{
								prms.Add(point.X.ToString("F6"));
								prms.Add(point.Y.ToString("F6"));
							}
							HTTPClient hc = new HTTPClient(Gnd.I.Config.ServerUrlPrefix + string.Join(":", prms));

							hc.Get();

							string[] rets = Encoding.ASCII.GetString(hc.ResBody).Split(':');
							int c = 0;

							List<Route> dest = new List<Route>();

							while (c < rets.Length)
							{
								Route route = new Route();

								route.StartPointName = CommonUtils.PlotIndexToName(int.Parse(rets[c++]));
								route.EndPointName = CommonUtils.PlotIndexToName(int.Parse(rets[c++]));

								{
									string[] tokens = rets[c++].Split(' ');

									double x = double.Parse(tokens[0]);
									double y = double.Parse(tokens[1]);

									route.StartPoint = new MapPoint(x, y);
								}

								{
									string[] tokens = rets[c++].Split(' ');

									double x = double.Parse(tokens[0]);
									double y = double.Parse(tokens[1]);

									route.EndPoint = new MapPoint(x, y);
								}

								route.RouteDistance = double.Parse(rets[c++]);

								{
									int rtPtNum = int.Parse(rets[c++]);
									MapPoint[] rtPts = new MapPoint[rtPtNum];

									for (int i = 0; i < rtPtNum; i++)
									{
										string[] tokens = rets[c++].Split(' ');

										double x = double.Parse(tokens[0]);
										double y = double.Parse(tokens[1]);

										rtPts[i] = new MapPoint(x, y);
									}
									route.RoutePoints = rtPts;
								}

								dest.Add(route);
							}
							resRoutes = dest.ToArray();
						},
						() =>
						{
							if (resRoutes != null)
							{
								Gnd.I.WearLayer.ClearRoutes();

								foreach (Route route in resRoutes)
									Gnd.I.WearLayer.AddRoute(route);

								this.UIRefresh_WearLayer();
							}
						}
						);
				}
			}
			catch (Exception ex)
			{
				ProcMain.WriteLog(ex);

				CommonUtils.ShowMessage("ルート検索_失敗", ex);
			}
		}

		private void ルートクリアToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.WearLayer.ClearRoutes();

			this.UIRefresh_WearLayer();
		}

		private void プロットを顧客情報として抽出する_抽出MenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				string file = ProcMain.SelfFile + ".Tempプロット_顧客情報.txt";
				//string file = @"C:\temp\プロット_顧客情報.txt"; // old

				using (StreamWriter writer = new StreamWriter(file, false, Encoding.UTF8))
				{
					foreach (MapPoint point in Gnd.I.WearLayer.GetPoints())
					{
						writer.WriteLine("\t<User>");
						writer.WriteLine("\t\t<Name>顧客" + SecurityTools.MakePassword_9().Substring(0, 10) + "</Name>");
						writer.WriteLine("\t\t<Point X=\"" + point.X.ToString("F6") + "\" Y=\"" + point.Y.ToString("F6") + "\"></Point>");
						writer.WriteLine("\t\t<Mail></Mail>");
						writer.WriteLine("\t</User>");
					}
				}

				ProcessTools.Batch(new string[] { "START \"\" \"" + file + "\"" });
			}
			catch (Exception ex)
			{
				CommonUtils.ShowMessage("抽出_失敗", ex);
			}
		}

		private void aroundToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BeforeDialog();

			try
			{
				if (Gnd.I.WearLayer.GetRoutes().Length == 0)
					throw new Exception("先にルート検索を行って下さい。");

				using (AroundDlg f = new AroundDlg())
				{
					f.ShowDialog();

					if (f.OkPressed)
					{
						double circle_r = double.Parse(f.Ret_CircleR);

						if (circle_r < 100.0)
							throw new Exception("100メートル以上にして下さい。");

						if (10000.0 < circle_r)
							throw new Exception("10000メートル以下にして下さい。");

						Gnd.I.WearLayer.ClearAroundCircles();

						Gnd.I.WearLayer.AddAroundCircle(new AroundCircle()
						{
							Point = Gnd.I.WearLayer.GetRoutes()[0].RoutePoints[0],
							R = circle_r,
						});

						foreach (Route route in Gnd.I.WearLayer.GetRoutes())
						{
							for (int index = 1; index < route.RoutePoints.Length; index++)
							{
								MapPoint startPoint = route.RoutePoints[index - 1];
								MapPoint endPoint = route.RoutePoints[index];

								double d = CrashUtils.GetDistance(startPoint, endPoint);
								double interval = circle_r / 10.0;
								int n = (int)(d / interval) + 1;

								for (int c = 1; c <= n; c++)
								{
									double rate = (double)c / n;

									double x = startPoint.X + rate * (endPoint.X - startPoint.X);
									double y = startPoint.Y + rate * (endPoint.Y - startPoint.Y);

									Gnd.I.WearLayer.AddAroundCircle(new AroundCircle()
									{
										Point = new MapPoint(x, y),
										R = circle_r,
									});
								}
							}
						}

						using (BusyDlg ff = new BusyDlg())
						{
							ff.Routine = () =>
							{
								Gnd.I.WearLayer.AroundCircles_Changed();
							};

							ff.ShowDialog();
						}
					}
				}
			}
			catch (Exception ex)
			{
				CommonUtils.ShowMessage("Around_失敗", ex);
			}

			this.AfterDialog();
			this.UIRefresh_WearLayer();
		}

		private void メール送信ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// TODO
		}

		private void Aroundヒット_プロットToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (UserInfo user in Gnd.I.UserMgr.Users)
				if (user.Hit)
					Gnd.I.WearLayer.AddPoint(user.Point);

			this.UIRefresh_WearLayer();
		}

		private void aroundクリアToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Gnd.I.WearLayer.ClearAroundCircles();
			this.UIRefresh_WearLayer();
		}

		private void GoogleMapUrlMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				GetGooleMapUrl_Routine(url =>
				{
					string file = ProcMain.SelfFile + ".Temp_GooleMapUrl.txt";

					File.WriteAllText(file, url, Encoding.ASCII);

					ProcessTools.Batch(new string[] { "START \"\" \"" + file + "\"" });
				});
			}
			catch (Exception ex)
			{
				CommonUtils.ShowMessage("GoogleMapUrl_失敗", ex);
			}
		}

		private void GoogleMap表示MenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				GetGooleMapUrl_Routine(url =>
				{
					ProcessTools.Batch(new string[] { "START \"\" \"" + url + "\"" });
				});
			}
			catch (Exception ex)
			{
				CommonUtils.ShowMessage("GoogleMap表示_失敗", ex);
			}
		}

		private void GetGooleMapUrl_Routine(Action<string> rtn)
		{
			Route route = GetSelectedRoute();

			double[] startLatLon = null;
			double[] endLatLon = null;

			Gnd.I.TileStore.GoogleMapJob.SetJob(
				() =>
				{
					startLatLon = MapPointToLatLon(route.StartPoint);
					endLatLon = MapPointToLatLon(route.EndPoint);
				},
				() =>
				{
					if (startLatLon != null && endLatLon != null)
					{
						string url = Gnd.I.Config.GoogleMapUrlFormat;

						url = url.Replace("$(START_LAT)", startLatLon[0].ToString("F9"));
						url = url.Replace("$(START_LON)", startLatLon[1].ToString("F9"));
						url = url.Replace("$(END_LAT)", endLatLon[0].ToString("F9"));
						url = url.Replace("$(END_LON)", endLatLon[1].ToString("F9"));

						rtn(url);
					}
				}
				);
		}

		private Route GetSelectedRoute()
		{
			Route[] routes = Gnd.I.WearLayer.GetRoutes();

			if (routes.Length < 1)
				throw new Exception("ルート検索を行って下さい。");

			if (routes.Length == 1)
				return routes[0];

			int index = Gnd.I.WearLayer.HighlightRouteIndex;

			if (index < 0 || routes.Length <= index)
				throw new Exception("ルート選択(ハイライト表示)して下さい。");

			return routes[index];
		}

		private double[] MapPointToLatLon(MapPoint point)
		{
			HTTPClient hc = new HTTPClient(Gnd.I.Config.ServerUrlPrefix + string.Join(":",
				"xy-to-latlon",
				point.X.ToString("F6"),
				point.Y.ToString("F6")
				));

			hc.Get();

			string[] rets = Encoding.ASCII.GetString(hc.ResBody).Split(' ');
			int c = 0;

			double lat = double.Parse(rets[c++]);
			double lon = double.Parse(rets[c++]);

			return new double[] { lat, lon };
		}
	}
}
