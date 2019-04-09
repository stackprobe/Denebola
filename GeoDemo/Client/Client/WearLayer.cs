using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Utils;
using Charlotte.Tools;
using System.Drawing.Imaging;

namespace Charlotte
{
	public class WearLayer
	{
		public Image GetWearedTilePic(Image bg, Gnd.Tile tile)
		{
			return GetWearedTilePic(
				bg,
				(tile.L + 0) * Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0),
				(tile.T + 0) * Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0),
				(tile.L + 1) * Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0),
				(tile.T + 1) * Consts.TILE_WH * (tile.MeterPerMDot / 1000000.0),
				tile.MeterPerMDot
				);
		}

		public Image GetWearedTilePic(Image bg, double l, double t, double r, double b, double merterPerMDot)
		{
			Bitmap bmp = new Bitmap(Consts.TILE_WH, Consts.TILE_WH);

			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.DrawImage(bg, 0, 0);

				using (Bitmap ggBmp = new Bitmap(Consts.TILE_WH, Consts.TILE_WH))
				{
					using (Graphics gg = Graphics.FromImage(ggBmp))
					{
						foreach (AroundCircle ac in this.AroundCircles)
						{
							double x = (ac.Point.X - l) * Consts.TILE_WH / (r - l);
							double y = (ac.Point.Y - t) * Consts.TILE_WH / (b - t);
							double circle_r = ac.R / (merterPerMDot / 1000000.0);

							double margin = circle_r;

							if (CrashUtils.IsCrashed_Rect_Point(0 - margin, 0 - margin, Consts.TILE_WH + margin, Consts.TILE_WH + margin, x, y))
							{
								gg.FillEllipse(Brushes.Yellow, (float)(x - circle_r), (float)(y - circle_r), (float)(circle_r * 2.0), (float)(circle_r * 2.0));

								{
									circle_r *= 0.7; // 0.7 == (1 / ルート2) - margin

									double x1 = x - circle_r;
									double y1 = y - circle_r;
									double x2 = x + circle_r;
									double y2 = y + circle_r;

									if (x1 < 0 && y1 < 0 && Consts.TILE_WH < x2 && Consts.TILE_WH < y2) // ? 塗りつぶしてしまった。-> これ以上描画する必要は無い。
									{
										break;
									}
								}
							}
						}

						{
							Action<Route, Color> r_drawRoute = (route, routeColor) =>
							{
								Action<MapPoint, MapPoint> r_drawLine = (p1, p2) =>
								{
									double x1 = (p1.X - l) * Consts.TILE_WH / (r - l);
									double y1 = (p1.Y - t) * Consts.TILE_WH / (b - t);
									double x2 = (p2.X - l) * Consts.TILE_WH / (r - l);
									double y2 = (p2.Y - t) * Consts.TILE_WH / (b - t);

									double margin = 10.0;
									float width = 8F;

									if (CrashUtils.IsCrashed_Rect_Line(0 - margin, 0 - margin, Consts.TILE_WH + margin, Consts.TILE_WH + margin, x1, y1, x2, y2))
									{
										gg.DrawLine(new Pen(routeColor, width), (float)x1, (float)y1, (float)x2, (float)y2);
										gg.FillEllipse(new SolidBrush(routeColor), (float)x1 - width / 2, (float)y1 - width / 2, width, width);
									}
								};

								r_drawLine(route.StartPoint, route.RoutePoints[0]);

								for (int index = 1; index < route.RoutePoints.Length; index++)
									r_drawLine(route.RoutePoints[index - 1], route.RoutePoints[index]);

								r_drawLine(route.RoutePoints[route.RoutePoints.Length - 1], route.EndPoint);
							};

							foreach (Route route in this.Routes)
							{
								r_drawRoute(route, Gnd.I.Config.RouteColor); // FIXME ハイライト2回描画している。
							}

							if (0 <= this.HighlightRouteIndex && this.HighlightRouteIndex < this.Routes.Count)
							{
								r_drawRoute(this.Routes[this.HighlightRouteIndex], Color.DarkBlue);
							}
						}
					}

					double alpha = 0.6;

					ColorMatrix cm = new ColorMatrix();
					cm.Matrix00 = 1F;
					cm.Matrix11 = 1F;
					cm.Matrix22 = 1F;
					cm.Matrix33 = (float)alpha;
					cm.Matrix44 = 1F;

					ImageAttributes ia = new ImageAttributes();
					ia.SetColorMatrix(cm);

					g.DrawImage(ggBmp, new Rectangle(0, 0, Consts.TILE_WH, Consts.TILE_WH), 0, 0, Consts.TILE_WH, Consts.TILE_WH, GraphicsUnit.Pixel, ia);
				}

				for (int index = 0; index < this.Points.Count; index++)
				{
					MapPoint point = this.Points[index];

					double x = (point.X - l) * Consts.TILE_WH / (r - l);
					double y = (point.Y - t) * Consts.TILE_WH / (b - t);
					double circle_r = 15.0;

					Brush brush = new SolidBrush(Color.Red);
					Font font = new Font(Consts.MONOSPC_FONT_NAME, 12F, FontStyle.Regular);

					double margin = circle_r;

					if (CrashUtils.IsCrashed_Rect_Point(0 - margin, 0 - margin, Consts.TILE_WH + margin, Consts.TILE_WH + margin, x, x))
					{
						g.FillEllipse(brush, (float)(x - circle_r), (float)(y - circle_r), (float)(circle_r * 2.0), (float)(circle_r * 2.0));

						string name = CommonUtils.PlotIndexToName(index);

						// FIXME 適当な位置

						x -= 3.0 + 3.0 * name.Length;
						y -= 7.0;

						g.DrawString(name, font, Brushes.White, (float)x, (float)y);
					}
				}

				if (Gnd.I.Config.ShowUser)
				{
					for (int index = 0; index < Gnd.I.UserMgr.Users.Count; index++)
					{
						UserInfo user = Gnd.I.UserMgr.Users[index];
						MapPoint point = user.Point;

						double x = (point.X - l) * Consts.TILE_WH / (r - l);
						double y = (point.Y - t) * Consts.TILE_WH / (b - t);
						double circle_r = 10.0;

						//Brush brush = new SolidBrush(Gnd.I.Config.UserColor);
						//Brush brush = new SolidBrush(Color.DarkRed);
						//Brush brush = new SolidBrush(Color.Magenta);
						//Brush brush = new SolidBrush(Color.YellowGreen);
						Font font = new Font(Consts.MONOSPC_FONT_NAME, 8F, FontStyle.Regular);

						double margin = circle_r;

						if (CrashUtils.IsCrashed_Rect_Point(0 - margin, 0 - margin, Consts.TILE_WH + margin, Consts.TILE_WH + margin, x, x))
						{
							Brush brush;
							string name;

							if (user.Hit)
							{
								brush = new SolidBrush(Gnd.I.Config.HitUserColor);
								name = "HI";
							}
							else
							{
								brush = new SolidBrush(Gnd.I.Config.UserColor);
								name = "CL";
							}
							g.FillEllipse(brush, (float)(x - circle_r), (float)(y - circle_r), (float)(circle_r * 2.0), (float)(circle_r * 2.0));

							// FIXME 適当な位置

							x -= 6.0;
							y -= 4.0;

							g.DrawString(name, font, Brushes.White, (float)x, (float)y);
						}
					}
				}
			}
			return bmp;
		}

		private List<MapPoint> Points = new List<MapPoint>();
		private List<Route> Routes = new List<Route>();
		private List<AroundCircle> AroundCircles = new List<AroundCircle>();

		public int HighlightRouteIndex = -1; // -1 ～ *

		public string[] GetStatusLines()
		{
			List<string> dest = new List<string>();

			dest.Add("Plot:");

			for (int index = 0; index < this.Points.Count; index++)
			{
				if (20 <= index)
				{
					dest.Add("　　...");
					break;
				}
				dest.Add("　　" + this.Points[index]);
			}

			dest.Add("Route:");

			for (int index = 0; index < this.Routes.Count; index++)
			{
				if (20 <= index)
				{
					dest.Add("　　...");
					break;
				}
				Route route = this.Routes[index];

				dest.Add("　　" + route.StartPointName + " > " + route.EndPointName + " " + route.RouteDistance + "m (" + route.RoutePoints.Length + ")" + (index == this.HighlightRouteIndex ? " *" : ""));
			}
			if (2 <= this.Routes.Count)
			{
				double totalRouteDistance = 0.0;
				int totalRoutePointCount = 0;

				foreach (Route route in this.Routes)
				{
					totalRouteDistance += route.RouteDistance;
					totalRoutePointCount += route.RoutePoints.Length;
				}
				dest.Add("　　Total: " + totalRouteDistance + "m (" + totalRoutePointCount + ")");
			}

			dest.Add("Around: " + this.AroundCircles.Count);

			return dest.ToArray();
		}

		public void Clear()
		{
			this.Points.Clear();
			this.ClearRoutes();
		}

		public void Clear_LastOne()
		{
			if (1 <= this.Points.Count)
			{
				this.Points.RemoveAt(this.Points.Count - 1);
				this.ClearRoutes();
			}
		}

		public void ClearRoutes()
		{
			this.Routes.Clear();
			this.ClearAroundCircles();

			// extra ---->

			this.HighlightRouteIndex = -1;
		}

		public void ClearAroundCircles()
		{
			this.AroundCircles.Clear();

			// extra ---->

			this.AroundCircles_Changed();
		}

		public void AroundCircles_Changed()
		{
			foreach (UserInfo user in Gnd.I.UserMgr.Users)
			{
				bool hit = false;

				foreach (AroundCircle ac in this.AroundCircles)
				{
					if (CrashUtils.GetDistance(user.Point, ac.Point) <= ac.R)
					{
						hit = true;
						break;
					}
				}
				user.Hit = hit;
			}
		}

		private bool HasSamePoint(MapPoint point)
		{
			foreach (MapPoint p in this.Points)
				if (CrashUtils.IsCrashed_Point_Point(p, point))
					return true;

			return false;
		}

		public void AddPoint(MapPoint point)
		{
			if (100 < this.Points.Count) // ? 多過ぎる。
			{
				ProcMain.WriteLog("プロット点過多クリア");

				this.Clear();
			}
			if (HasSamePoint(point))
			{
				ProcMain.WriteLog("同じ地点をプロットしようとしました。" + point);

				return;
			}
			this.Points.Add(point);
		}

		public void AddRoute(Route route)
		{
			if (100 < this.Routes.Count) // ? 多過ぎる。
			{
				ProcMain.WriteLog("ルート過多クリア");

				this.ClearRoutes();
			}
			this.Routes.Add(route);

			{
				int count = 0;

				foreach (Route r in this.Routes)
					count += r.RoutePoints.Length;

				if (30000 < count) // ? 多過ぎる。
				{
					ProcMain.WriteLog("ルート経由点過多クリア");

					this.ClearRoutes();
				}
			}
		}

		public void AddAroundCircle(AroundCircle ar)
		{
			if (10000 < this.AroundCircles.Count)
			{
				ProcMain.WriteLog("AroundCircle過多クリア");

				this.ClearAroundCircles();
			}
			this.AroundCircles.Add(ar);
		}

		public MapPoint[] GetPoints()
		{
			return this.Points.ToArray();
		}

		public Route[] GetRoutes()
		{
			return this.Routes.ToArray();
		}

		public AroundCircle[] GetAroundCircles()
		{
			return this.AroundCircles.ToArray();
		}
	}
}
