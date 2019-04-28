using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Utils;

namespace Charlotte.Layer.MapLayer
{
	public class Area
	{
		private string Dir;

		public Area(string dir)
		{
			this.Dir = dir;
		}

		private string GetConvedFile()
		{
			return Path.Combine(this.Dir, "Area.conved.txt");
		}

		private string GetOthersConvedFile()
		{
			return Path.Combine(this.Dir, "Others.conved.txt");
		}

		private string GetAreaFile()
		{
			return Path.Combine(this.Dir, "Area.txt");
		}

		private bool? Leaf = null;

		private bool IsLeaf()
		{
			if (this.Leaf == null)
				this.Leaf = File.Exists(this.GetConvedFile());

			return this.Leaf.Value;
		}

		private GeoRectangle[] AreaRectangles = null;

		private GeoRectangle[] GetAreaRectangles()
		{
			if (this.AreaRectangles == null)
			{
				List<GeoRectangle> dest = new List<GeoRectangle>();

				using (StreamReader reader = new StreamReader(GetAreaFile(), Encoding.ASCII))
				{
					for (; ; )
					{
						string line = reader.ReadLine();

						if (line == null)
							break;

						if (line != "A")
							throw new Exception("不明なデータ識別子です。" + line);

						double latMin = double.Parse(reader.ReadLine());
						double latMax = double.Parse(reader.ReadLine());
						double lonMin = double.Parse(reader.ReadLine());
						double lonMax = double.Parse(reader.ReadLine());

						if (reader.ReadLine() != "/")
							throw new Exception("データが破損しています。" + line);

						dest.Add(new GeoRectangle(latMin, latMax, lonMin, lonMax));
					}
				}
				this.AreaRectangles = dest.ToArray();
			}
			return this.AreaRectangles;
		}

		private Area SW = null;
		private Area SE = null;
		private Area NW = null;
		private Area NE = null;

		public Area GetSW()
		{
			if (this.SW == null)
				this.SW = new Area(Path.Combine(this.Dir, "SW"));

			return this.SW;
		}

		public Area GetSE()
		{
			if (this.SE == null)
				this.SE = new Area(Path.Combine(this.Dir, "SE"));

			return this.SE;
		}

		public Area GetNW()
		{
			if (this.NW == null)
				this.NW = new Area(Path.Combine(this.Dir, "NW"));

			return this.NW;
		}

		public Area GetNE()
		{
			if (this.NE == null)
				this.NE = new Area(Path.Combine(this.Dir, "NE"));

			return this.NE;
		}

		public void Search(GeoRectangle gRect, Action<string> found)
		{
			if (this.IsLeaf())
			{
				found(GetConvedFile());
			}
			else
			{
				found(GetOthersConvedFile());

				GeoRectangle[] areaRects = GetAreaRectangles();

				if (CrashUtils.IsCrashed_Rect_Rect(gRect, areaRects[1]))
				{
					this.GetSW().Search(gRect, found);
				}
				if (CrashUtils.IsCrashed_Rect_Rect(gRect, areaRects[2]))
				{
					this.GetSE().Search(gRect, found);
				}
				if (CrashUtils.IsCrashed_Rect_Rect(gRect, areaRects[3]))
				{
					this.GetNW().Search(gRect, found);
				}
				if (CrashUtils.IsCrashed_Rect_Rect(gRect, areaRects[4]))
				{
					this.GetNE().Search(gRect, found);
				}
			}
		}
	}
}
