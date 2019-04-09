using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Charlotte.Tools;
using Charlotte.Utils;

namespace Charlotte
{
	class Program
	{
		public const string APP_IDENT = "{71d75f21-caef-4cb7-9482-f9e8290518ab}";
		public const string APP_TITLE = "Server";

		static void Main(string[] args)
		{
			ProcMain.CUIMain(new Program().Main2, APP_IDENT, APP_TITLE);

#if DEBUG
			CommonUtils.Print("Press ENTER.");
			Console.ReadLine();
#endif
		}

		private void Main2(ArgsReader ar)
		{
			//new Test0001().Test01();
			//new Test0001().Test02();
			Main3();
		}

		private void Main3()
		{
			using (Mutex procMtx = MutexTools.CreateGlobal("{e6b15b99-c7e4-4f7b-aff0-9a3022f00aa7}"))
			{
				if (procMtx.WaitOne(0))
				{
					try
					{
						Main3_NoProcMtx();
					}
					finally
					{
						procMtx.ReleaseMutex();
					}
				}
			}
		}

		private void Main3_NoProcMtx()
		{
			DateTime 地図データのロード開始時刻 = DateTime.Now;

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

			Gnd.I.ConfigChanged();

			DateTime 地図データのロード終了時刻 = DateTime.Now;
			CommonUtils.Print("地図データのロード完了 (" + ((地図データのロード終了時刻 - 地図データのロード開始時刻).TotalMilliseconds / 1000.0) + " 秒)");

			HTTPServer_T hs = new HTTPServer_T();
			hs.PortNo = Gnd.I.Config.PortNo;
			hs.HTTPConnected = Connected;
			hs.Start();
			CommonUtils.Print("hs started");

			ByDiskServer bds = new ByDiskServer();
			bds.Connected = Connected;
			bds.Start();
			CommonUtils.Print("bds started");

			CommonUtils.Print("Press ENTER to stop the server...");
			Console.ReadLine();
			CommonUtils.Print("Stop the server");

			hs.Stop_B();
			CommonUtils.Print("hs stopped");

			bds.Stop_B();
			CommonUtils.Print("bds stopped");
		}

		private object Connected_SYNCROOT = new object();

		private void Connected(HTTPServerChannel channel)
		{
			lock (Connected_SYNCROOT)
			{
				Connected_NoSync(channel);
			}
		}

		private void Connected_NoSync(HTTPServerChannel channel)
		{
			CommonUtils.Print("リクエストのパス：" + channel.Path);

			string[] pathTokens = channel.Path.Split('/');
			string[] prms = pathTokens[pathTokens.Length - 1].Split(':');
			int c = 0;

			string command = prms[c++];

			if (command == "route")
			{
				MapPoint startPoint;
				MapPoint endPoint;

				{
					double x = double.Parse(prms[c++]);
					double y = double.Parse(prms[c++]);

					startPoint = new MapPoint(x, y);
				}

				{
					double x = double.Parse(prms[c++]);
					double y = double.Parse(prms[c++]);

					endPoint = new MapPoint(x, y);
				}

				Router router = new Router()
				{
					StartPoint = startPoint,
					EndPoint = endPoint,
				};

				router.Invoke();

				List<string> dest = new List<string>();

				dest.Add(router.RouteDistance.ToString("F3"));
				dest.AddRange(router.RouteNodes.Select(node => node.Point.ToString()));

				channel.ResContentType = "text/plain; charset=US-ASCII";
				channel.ResBody_B = Encoding.ASCII.GetBytes(string.Join(":", dest));

				CommonUtils.Print("ルートの長さ：" + router.RouteDistance);
				CommonUtils.Print("ノードの個数：" + router.RouteNodes.Count);
			}
			else if (command == "multi-route")
			{
				MultiRouter mr = new MultiRouter();

				while (c < prms.Length)
				{
					double x = double.Parse(prms[c++]);
					double y = double.Parse(prms[c++]);

					mr.PlotPoints.Add(new MapPoint(x, y));
				}

				mr.Invoke();

				List<string> dest = new List<string>();

				double totalRouteDistance = 0.0;
				int totalRouteNodeCount = 0;

				foreach (Router router in mr.Routers)
				{
					dest.Add("" + router.MltRt_StartPlotIndex);
					dest.Add("" + router.MltRt_EndPlotIndex);
					dest.Add(router.StartPoint.ToString());
					dest.Add(router.EndPoint.ToString());
					dest.Add(router.RouteDistance.ToString("F3"));
					dest.Add("" + router.RouteNodes.Count);
					dest.AddRange(router.RouteNodes.Select(node => node.Point.ToString()));

					CommonUtils.Print("ルートの長さ：" + router.RouteDistance);
					CommonUtils.Print("ノードの個数：" + router.RouteNodes.Count);

					totalRouteDistance += router.RouteDistance;
					totalRouteNodeCount += router.RouteNodes.Count;
				}
				channel.ResContentType = "text/plain; charset=US-ASCII";
				channel.ResBody_B = Encoding.ASCII.GetBytes(string.Join(":", dest));

				CommonUtils.Print("ルートの全長：" + totalRouteDistance);
				CommonUtils.Print("ノードの総数：" + totalRouteNodeCount);
			}
			else if (command == "address-to-xy")
			{
				string addressPart = prms[c++];

				CommonUtils.Print("住所：" + addressPart);

				if (addressPart == "")
					throw new Exception("空の住所パターンです。");

#if true
				AddressStore.AddressInfo[] ais = Gnd.I.Map.AddressStore.Addresses.Where(ai => ai.Address.Replace(" ", "").Contains(addressPart)).ToArray();
#else // old
				AddressStore.AddressInfo[] ais = Gnd.I.Map.AddressStore.Addresses.Where(ai => ai.Address.Contains(addressPart)).ToArray();
#endif

				if (1 <= ais.Length)
				{
					double x = 0.0;
					double y = 0.0;

					foreach (AddressStore.AddressInfo ai in ais)
					{
						x += ai.Point.X;
						y += ai.Point.Y;
					}
					x /= ais.Length;
					y /= ais.Length;

					MapPoint point = new MapPoint(x, y);

					channel.ResContentType = "text/plain; charset=US-ASCII";
					channel.ResBody_B = Encoding.ASCII.GetBytes(point.ToString());

					CommonUtils.Print("位置：" + point);
				}
				else
				{
					channel.ResContentType = "text/plain";
					channel.ResBody_B = new byte[0];

					CommonUtils.Print("位置不明");
				}
			}
			else if (command == "xy-to-address")
			{
				double x = double.Parse(prms[c++]);
				double y = double.Parse(prms[c++]);

				MapPoint point = new MapPoint(x, y);
				GeoPoint gPoint = new GeoPoint(point);

				CommonUtils.Print("位置：" + point + " (" + gPoint + ")");

				AddressStore.AddressInfo nearestAi = null;
				double nearestD = double.MaxValue;

				foreach (AddressStore.AddressInfo ai in Gnd.I.Map.AddressStore.Addresses)
				{
					double d = CrashUtils.GetDistance(ai.Point, point);

					if (d < nearestD)
					{
						nearestAi = ai;
						nearestD = d;
					}
				}
				channel.ResContentType = "text/plain; charset=UTF-8";
				channel.ResBody_B = Encoding.UTF8.GetBytes(nearestAi.Address + ":東経 " + gPoint.Lon.ToString("F9") + ":北緯 " + gPoint.Lat.ToString("F9"));

				CommonUtils.Print("住所：" + nearestAi.Address);
			}
			else if (command == "xy-to-latlon")
			{
				double x = double.Parse(prms[c++]);
				double y = double.Parse(prms[c++]);

				MapPoint point = new MapPoint(x, y);
				GeoPoint gPoint = new GeoPoint(point);

				channel.ResContentType = "text/plain; charset=US-ASCII";
				channel.ResBody_B = Encoding.ASCII.GetBytes(gPoint.ToString());

				CommonUtils.Print("緯度経度：" + gPoint);
			}
			else if (command == "tile")
			{
				double l = double.Parse(prms[c++]);
				double t = double.Parse(prms[c++]);
				double r = double.Parse(prms[c++]);
				double b = double.Parse(prms[c++]);

				if (
					l < -1000000.0 ||
					t < -1000000.0 ||
					r < l + 1.0 ||
					b < t + 1.0 ||
					1000000.0 < r ||
					1000000.0 < b
					)
					throw new Exception("地図上の領域に問題があります。");

				int bmp_w = int.Parse(prms[c++]);
				int bmp_h = int.Parse(prms[c++]);

				if (
					bmp_w < 10 || 10000 < bmp_w ||
					bmp_h < 10 || 10000 < bmp_h
					)
					throw new Exception("地図画像のサイズに問題があります。");

				string layerOn = prms[c++];
				string roadOn = prms[c++];

				using (Bitmap bmp = new Bitmap(bmp_w, bmp_h))
				{
					using (Graphics grph = Graphics.FromImage(bmp))
					{
						grph.FillRectangle(Brushes.White, 0, 0, bmp_w, bmp_h);

						for (int index = 0; index < 20; index++)
						{
							if (layerOn[index] == '1')
							{
								Color color;

								// XXX 適当な色
								{
									int color_r = index / 9;
									int color_g = (index / 3) % 3;
									int color_b = index % 3;

									color_r = new int[] { 0, 127, 255 }[color_r];
									color_g = new int[] { 0, 127, 255 }[color_g];
									color_b = new int[] { 0, 127, 255 }[color_b];

									color = Color.FromArgb(color_r, color_g, color_b);
								}

								Layer layer = Gnd.I.Map.Layers[index];

								Pen pen = new Pen(color, 1f);
								Brush brush = new SolidBrush(color);

								Where_MultiThread_Action<MapLine>(
									layer.LineMgm,
									mg => CrashUtils.IsCrashed_Rect_Rect(l, t, r, b, mg.L, mg.T, mg.R, mg.B),
									line => CrashUtils.IsCrashed_Rect_Line(l, t, r, b, line.A.X, line.A.Y, line.B.X, line.B.Y),
									line =>
									{
										double x1 = (line.A.X - l) * bmp_w / (r - l);
										double x2 = (line.B.X - l) * bmp_w / (r - l);
										double y1 = (line.A.Y - t) * bmp_h / (b - t);
										double y2 = (line.B.Y - t) * bmp_h / (b - t);

										grph.DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2);
									}
									);

								Where_MultiThread_Action<MapPoint>(
									layer.PointMgm,
									mg => CrashUtils.IsCrashed_Rect_Rect(l, t, r, b, mg.L, mg.T, mg.R, mg.B),
									point => CrashUtils.IsCrashed_Rect_Point(l, t, r, b, point.X, point.Y),
									point =>
									{
										double x = (point.X - l) * bmp_w / (r - l);
										double y = (point.Y - t) * bmp_h / (b - t);

										int POINT_WH = 5;

										grph.FillRectangle(brush, (float)(x - POINT_WH / 2.0), (float)(y - POINT_WH / 2.0), (float)POINT_WH, (float)POINT_WH);
									}
									);
							}
						}

						if (roadOn[0] == '1')
						{
							Pen pen = new Pen(Color.FromArgb(128, Color.Orange), 2f);

							Where_MultiThread_Action<Road.Branch>(
								Gnd.I.Map.Road.BranchMgm,
								mg => CrashUtils.IsCrashed_Rect_Rect(l, t, r, b, mg.L, mg.T, mg.R, mg.B),
								branch => CrashUtils.IsCrashed_Rect_Line(l, t, r, b, branch.A.Point.X, branch.A.Point.Y, branch.B.Point.X, branch.B.Point.Y),
								branch =>
								{
									double x1 = (branch.A.Point.X - l) * bmp_w / (r - l);
									double x2 = (branch.B.Point.X - l) * bmp_w / (r - l);
									double y1 = (branch.A.Point.Y - t) * bmp_h / (b - t);
									double y2 = (branch.B.Point.Y - t) * bmp_h / (b - t);

									grph.DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2);
								}
								);
						}
					}

					using (MemoryStream mem = new MemoryStream())
					{
						bmp.Save(mem, ImageFormat.Png);

						channel.ResContentType = "image/png";
						channel.ResBody_B = mem.GetBuffer();
					}
				}

				CommonUtils.Print("タイル画像サイズ：" + channel.ResBody_B.Length);
			}
			CommonUtils.Print("OK!");
		}

		private static void Where_MultiThread_Action<T>(MeshedGroupManager<T> mgm, Func<MeshedGroup<T>, bool> mgAcceptor, Func<T, bool> acceptor, Action<T> accepted)
		{
			MeshedGroup<T>[] mgs = mgm.GetMeshedGroups(mgAcceptor);
			T[][] valTbl = new T[4][];

			using (MultiThreadEx mte = new MultiThreadEx())
			{
				mte.Add(() => valTbl[0] = WhereRange<T>(mgs, 0, 4, acceptor));
				mte.Add(() => valTbl[1] = WhereRange<T>(mgs, 1, 4, acceptor));
				mte.Add(() => valTbl[2] = WhereRange<T>(mgs, 2, 4, acceptor));
				mte.Add(() => valTbl[3] = WhereRange<T>(mgs, 3, 4, acceptor));

				mte.RelayThrow();
			}

			foreach (T[] values in valTbl)
				foreach (T value in values)
					accepted(value);
		}

		private static T[] WhereRange<T>(MeshedGroup<T>[] mgs, int startIndex, int step, Func<T, bool> acceptor)
		{
			List<T> dest = new List<T>();

			for (int index = startIndex; index < mgs.Length; index += step)
				foreach (T item in mgs[index].Items)
					if (acceptor(item))
						dest.Add(item);

			return dest.ToArray();
		}

		private static T[] WhereRange<T>(T[] src, int startIndex, int endIndex, Func<T, bool> predicate)
		{
			List<T> dest = new List<T>();

			for (int index = startIndex; index < endIndex; index++)
				if (predicate(src[index]))
					dest.Add(src[index]);

			return dest.ToArray();
		}
	}
}
