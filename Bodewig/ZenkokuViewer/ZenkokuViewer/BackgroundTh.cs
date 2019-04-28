using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Charlotte.Tools;
using Charlotte.Utils;
using Charlotte.Layer;

namespace Charlotte
{
	public class BackgroundTh
	{
		private Thread Th;
		private bool StopFlag = false;

		public void Start()
		{
			Th = new Thread(() =>
			{
				int waitMillis = 0;

				while (StopFlag == false)
				{
					try
					{
						if (this.Invoke())
						{
							waitMillis = 0;
						}
						else
						{
							if (waitMillis < 100)
								waitMillis++;

							Thread.Sleep(waitMillis);
						}
					}
					catch (Exception e)
					{
						ProcMain.WriteLog(e);

						Thread.Sleep(500); // エラーによる長めの待ち。
					}
				}
			});

			Th.Start();
		}

		public void Stop_B()
		{
			StopFlag = true;

			Th.Join();
			Th = null;
		}

		private bool Invoke() // ret: ? busy
		{
			SubTile tile = null;

			Gnd.I.MainWin.Invoke((MethodInvoker)delegate
			{
				if (1 <= Gnd.I.SubTiles.Count)
				{
					tile = ArrayTools.Lightest(Gnd.I.SubTiles, t => CrashUtils.GetDistance(t.Owner.Rectangle.CenterPoint, Gnd.I.CenterPoint));
					Gnd.I.SubTiles.Remove(tile);
				}
			});

			if (tile != null)
			{
				CraftTile(tile);
				return true;
			}

			// todo ???

			return false;
		}

		private void CraftTile(SubTile tile)
		{
			ILayer[] activeLayers = null;
			GeoRectangle tileRect = new GeoRectangle();

			Gnd.I.MainWin.Invoke((MethodInvoker)delegate
			{
				activeLayers = Gnd.I.ActiveLayers.ToArray();
				tileRect = tile.Owner.Rectangle;
			});

			Bitmap bmp = new Bitmap(Consts.TILE_WH, Consts.TILE_WH);

			using (Graphics g = Graphics.FromImage(bmp))
			{
				for (int index = activeLayers.Length - 1; 0 <= index; index--)
					activeLayers[index].DrawTile(g, tileRect);
			}

			Gnd.I.MainWin.Invoke((MethodInvoker)delegate
			{
				tile.Owner.Bmp = bmp;
				Gnd.I.ChangingUI = true;
			});
		}
	}
}
