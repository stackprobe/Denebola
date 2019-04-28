using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Utils;

namespace Charlotte
{
	public class MeshedGroupManager<ItemType>
	{
		private const int TABLE_WH = 16;

		private MeshedGroup<ItemType>[][] Table = new MeshedGroup<ItemType>[TABLE_WH][];
		private MeshedGroup<ItemType> OthersMg = new MeshedGroup<ItemType>();
		private Func<ItemType, double[]> GetLTRB;

		public MeshedGroupManager(Func<ItemType, double[]> getLTRB)
		{
			for (int x = 0; x < TABLE_WH; x++)
			{
				Table[x] = new MeshedGroup<ItemType>[TABLE_WH];

				for (int y = 0; y < TABLE_WH; y++)
				{
					Table[x][y] = new MeshedGroup<ItemType>();
				}
			}
			this.GetLTRB = getLTRB;
		}

		public void SetItems(ItemType[] items)
		{
			double l;
			double t;
			double r;
			double b;

			if (1 <= items.Length)
			{
				double[] ltrb = GetLTRB(items[0]);

				l = ltrb[0];
				t = ltrb[1];
				r = ltrb[2];
				b = ltrb[3];
			}
			else
			{
				// 適当な範囲で良い。

				l = 0.0;
				t = 0.0;
				r = 1.0;
				b = 1.0;
			}

			for (int index = 1; index < items.Length; index++)
			{
				double[] ltrb = GetLTRB(items[index]);

				l = Math.Min(l, ltrb[0]);
				t = Math.Min(t, ltrb[1]);
				r = Math.Max(r, ltrb[2]);
				b = Math.Max(b, ltrb[3]);
			}
			for (int x = 0; x < TABLE_WH; x++)
			{
				for (int y = 0; y < TABLE_WH; y++)
				{
					MeshedGroup<ItemType> mg = Table[x][y];

					mg.L = l + (x + 0) * (r - l) / TABLE_WH;
					mg.T = t + (y + 0) * (b - t) / TABLE_WH;
					mg.R = l + (x + 1) * (r - l) / TABLE_WH;
					mg.B = t + (y + 1) * (b - t) / TABLE_WH;
				}
			}
			foreach (ItemType item in items)
			{
				double[] ltrb = GetLTRB(item);

				for (int x = 0; x < TABLE_WH; x++)
				{
					for (int y = 0; y < TABLE_WH; y++)
					{
						MeshedGroup<ItemType> mg = Table[x][y];

						double MARGIN = 0.001; // 1 mm

						// ? ltrb は mg の内側にある。
						if (
							mg.L + MARGIN < ltrb[0] && ltrb[2] < mg.R - MARGIN &&
							mg.T + MARGIN < ltrb[1] && ltrb[3] < mg.B - MARGIN
							)
						{
							mg.Wk_Items.Add(item);

							goto ITEM_ADDED;
						}
					}
				}
				OthersMg.Wk_Items.Add(item);

			ITEM_ADDED: ;
			}
			for (int x = 0; x < TABLE_WH; x++)
			{
				for (int y = 0; y < TABLE_WH; y++)
				{
					MeshedGroup<ItemType> mg = Table[x][y];

					mg.Items = mg.Wk_Items.ToArray();
					mg.Wk_Items = null;
				}
			}
			OthersMg.Items = OthersMg.Wk_Items.ToArray();
			OthersMg.Wk_Items = null;

			GC.Collect();
		}

		public MeshedGroup<ItemType>[] GetMeshedGroups(Func<MeshedGroup<ItemType>, bool> acceptor)
		{
			List<MeshedGroup<ItemType>> dest = new List<MeshedGroup<ItemType>>();

			for (int x = 0; x < TABLE_WH; x++)
			{
				for (int y = 0; y < TABLE_WH; y++)
				{
					MeshedGroup<ItemType> mg = Table[x][y];

					if (acceptor(mg))
						dest.Add(mg);
				}
			}
			dest.Add(OthersMg);
			return dest.ToArray();
		}

		public double Get枠内ヒット率()
		{
			int count = 0;

			for (int x = 0; x < TABLE_WH; x++)
			{
				for (int y = 0; y < TABLE_WH; y++)
				{
					MeshedGroup<ItemType> mg = Table[x][y];

					count += mg.Items.Length;
				}
			}
			return count == 0 ?
				0.0 :
				count / (double)(count + OthersMg.Items.Length);
		}
	}
}
