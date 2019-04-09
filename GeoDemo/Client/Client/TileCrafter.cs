using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Drawing;
using Charlotte.Tools;
using Charlotte.Utils;

namespace Charlotte
{
	public class TileCrafter
	{
		private Thread Th = null;
		private bool StopFlag = false;

		public void Start()
		{
			this.Th = new Thread(() =>
			{
				try
				{
					this.MainTh();
				}
				catch (Exception e)
				{
					ProcMain.WriteLog(e);
				}
			});

			this.Th.Start();
		}

		public void Stop()
		{
			this.StopFlag = true;

			this.Th.Join();
			this.Th = null;
		}

		private long NoNoTilePicTSTileCount;

		private void MainTh()
		{
			// XXX このスレッド内で Gnd.I.Config を見てしまっている。今の所問題無いけど、実装上マズい。@ 2018.10.12

			while (this.StopFlag == false)
			{
				{
					TileStore.TSTile[] tiles = Gnd.I.TileStore.GetNoTilePicTSTiles();

					if (1 <= tiles.Length)
					{
						this.NoNoTilePicTSTileCount = 0L;

						try
						{
							if (Gnd.I.Config.TileCrafter_ByDisk)
							{
								TileStore.TSTile tile = NextTSTile(tiles);

								tile.SetTilePic(ResBodyToTilePic(Gnd.I.ByDiskClient.Invoke(GetPath(tile))));
							}
							else
							{
								TileStore.TSTile tile = NextTSTile(tiles);

								HTTPClient hc = new HTTPClient(Gnd.I.Config.ServerUrlPrefix + GetPath(tile));

								ProcMain.WriteLog("tile hc.Get() Start");
								hc.Get();
								ProcMain.WriteLog("tile hc.Get() End");

								tile.SetTilePic(ResBodyToTilePic(hc.ResBody));
							}
						}
						catch (Exception e)
						{
							ProcMain.WriteLog(e);

							Thread.Sleep(500); // catnap -- エラー発生による
						}

						continue;
					}
					this.NoNoTilePicTSTileCount++;
				}

				Gnd.I.TileStore.RouteJob.RegularInvokeOnBackgroundTh();
				Gnd.I.TileStore.GoogleMapJob.RegularInvokeOnBackgroundTh();

				if (this.NoNoTilePicTSTileCount < 10L) // ? < 0.5 sec
				{
					Thread.Sleep(50);
					continue;
				}

				{
					string address = Gnd.I.TileStore.AddressToXY.TakeAddress();

					if (address != null)
					{
						try
						{
							HTTPClient hc = new HTTPClient(Gnd.I.Config.ServerUrlPrefix + string.Join(":",
								"address-to-xy",
								address
								));

							ProcMain.WriteLog("address-to-xy hc.Get() Start");
							hc.Get();
							ProcMain.WriteLog("address-to-xy hc.Get() End");

							string[] tokens = Encoding.ASCII.GetString(hc.ResBody).Split(' ');

							double x = double.Parse(tokens[0]);
							double y = double.Parse(tokens[1]);

							MapPoint point = new MapPoint(x, y);

							Gnd.I.TileStore.AddressToXY.SetPoint(point);
						}
						catch (Exception e)
						{
							ProcMain.WriteLog(e);
						}

						continue;
					}
				}

				{
					MapPoint? point = Gnd.I.TileStore.XYToAddress.TakePoint();

					if (point != null)
					{
						if (Gnd.I.Config.ShowCenterAddress == false)
						{
							Gnd.I.TileStore.XYToAddress.SetAddress("住所表示は無効になっています。");
							continue;
						}

						try
						{
							HTTPClient hc = new HTTPClient(Gnd.I.Config.ServerUrlPrefix + string.Join(":",
								"xy-to-address",
								point.Value.X.ToString("F6"),
								point.Value.Y.ToString("F6")
								));

							ProcMain.WriteLog("xy-to-address hc.Get() Start");
							hc.Get();
							ProcMain.WriteLog("xy-to-address hc.Get() End");

							string address = Encoding.UTF8.GetString(hc.ResBody);

							if (address == "")
								throw new Exception("空の住所を取得しました。");

							Gnd.I.TileStore.XYToAddress.SetAddress(address);
						}
						catch (Exception e)
						{
							ProcMain.WriteLog(e);

							Gnd.I.TileStore.XYToAddress.SetAddress("住所取得失敗");
						}

						continue;
					}
				}

				Thread.Sleep(100); // catnap
			}
		}

		private TileStore.TSTile NextTSTile(TileStore.TSTile[] tiles)
		{
			TileStore.TSTile ret = tiles[0];

			if (Gnd.I.Config.TileCrafter_PriorityToCenter)
			{
				double ret_d = GetDistance_Center(ret);

				for (int index = 1; index < tiles.Length; index++)
				{
					TileStore.TSTile tile = tiles[index];
					double d = GetDistance_Center(tile);

					if (d < ret_d)
					{
						ret = tile;
						ret_d = d;
					}
				}
			}
			return ret;
		}

		private double GetDistance_Center(TileStore.TSTile tile)
		{
			double tileX = (tile.L + tile.R) / 2.0;
			double tileY = (tile.T + tile.B) / 2.0;

			return CrashUtils.GetDistance(Gnd.I.CenterXY.X, Gnd.I.CenterXY.Y, tileX, tileY);
		}

		private string GetPath(TileStore.TSTile tile)
		{
			return string.Join(":",
				"tile",
				"" + tile.L,
				"" + tile.T,
				"" + tile.R,
				"" + tile.B,
				"" + tile.BmpW,
				"" + tile.BmpH,
				tile.LayerOn,
				"" + (tile.Road ? 1 : 0)
				);
		}

		private Bitmap ResBodyToTilePic(byte[] resBody)
		{
			if (resBody == null)
				throw new ArgumentNullException("resBody is null");

			using (MemoryStream mem = new MemoryStream(resBody))
			{
				return new Bitmap(mem);
			}
		}
	}
}
