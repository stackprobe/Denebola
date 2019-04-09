using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Tools;

namespace Charlotte
{
	/// <summary>
	/// 複数スレッドから触られる。
	/// </summary>
	public class TileStore
	{
		private static object SYNCROOT = new object();

		public class TSTile
		{
			public double L;
			public double T;
			public double R;
			public double B;
			public double BmpW;
			public double BmpH;
			public string LayerOn;
			public bool Road;

			// <---- 要求

			private Bitmap TilePic;

			public Bitmap GetTilePic()
			{
				lock (SYNCROOT)
				{
					return TilePic;
				}
			}

			public void SetTilePic(Bitmap img)
			{
				lock (SYNCROOT)
				{
					TilePic = img;
				}
			}

			// <---- 応答
		}

		private List<TSTile> TSTiles = new List<TSTile>();

		public void AddTSTile(TSTile tile)
		{
			lock (SYNCROOT)
			{
				TSTiles.Add(tile);
			}
		}

		public void RemoveTSTile(TSTile tile)
		{
			lock (SYNCROOT)
			{
				TSTiles.Remove(tile);
			}
		}

		public void RemoveAllTSTile()
		{
			lock (SYNCROOT)
			{
				TSTiles.Clear();
			}
		}

		public int GetTSTileCount()
		{
			lock (SYNCROOT)
			{
				return TSTiles.Count;
			}
		}

		public TSTile[] GetNoTilePicTSTiles()
		{
			List<TSTile> dest = new List<TSTile>();

			lock (SYNCROOT)
			{
				foreach (TileStore.TSTile tile in Gnd.I.TileStore.TSTiles)
					if (tile.GetTilePic() == null)
						dest.Add(tile);
			}
			return dest.ToArray();
		}

		// ★★★ Tile ここまで

		public class AddressXY
		{
			private string Address = null;
			private MapPoint? Point = null;

			public void SetAddress(string address)
			{
				lock (SYNCROOT)
				{
					Address = address;
				}
			}

			public string TakeAddress()
			{
				lock (SYNCROOT)
				{
					string ret = Address;
					Address = null;
					return ret;
				}
			}

			public void SetPoint(MapPoint point)
			{
				lock (SYNCROOT)
				{
					Point = point;
				}
			}

			public MapPoint? TakePoint()
			{
				lock (SYNCROOT)
				{
					MapPoint? ret = Point;
					Point = null;
					return ret;
				}
			}
		}

		public AddressXY AddressToXY = new AddressXY();
		public AddressXY XYToAddress = new AddressXY();

		public class Job
		{
			private Action BackgroundRtn;
			private Action PostBgFrontRtn;

			public void SetJob(Action backgroundRtn, Action postBgFrontRtn)
			{
				lock (SYNCROOT)
				{
					this.BackgroundRtn = backgroundRtn;
					this.PostBgFrontRtn = postBgFrontRtn;
				}
			}

			public void RegularInvokeOnBackgroundTh()
			{
				lock (SYNCROOT)
				{
					if (this.BackgroundRtn != null)
					{
						try
						{
							this.BackgroundRtn();
						}
						catch (Exception e)
						{
							ProcMain.WriteLog(e);
						}

						this.BackgroundRtn = null;
					}
				}
			}

			public void RegularInvoke()
			{
				lock (SYNCROOT)
				{
					if (this.BackgroundRtn == null && this.PostBgFrontRtn != null)
					{
						try
						{
							this.PostBgFrontRtn();
						}
						catch (Exception e)
						{
							ProcMain.WriteLog(e);
						}

						this.PostBgFrontRtn = null;
					}
				}
			}
		}

		public Job RouteJob = new Job();
		public Job GoogleMapJob = new Job();
	}
}
