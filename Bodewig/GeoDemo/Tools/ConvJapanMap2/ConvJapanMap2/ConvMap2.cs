using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class ConvMap2
	{
		public string R_Dir;
		public string W_Dir;

		// <---- prm

		public void Invoke()
		{
			string[] dirs = Directory.GetDirectories(R_Dir);

			foreach (string dir in dirs)
			{
				Divide2x2Layer(
					Path.Combine(dir, "All.conved.txt"),
					Path.Combine(W_Dir, Path.GetFileName(dir)),
					20.0,
					46.1, // 緯線上のアイテムを拾えるように 46 + 0.1
					122.0,
					154.1, // 経線上のアイテムを拾えるように 154 + 0.1
					0,
					() => { }
					);
			}
		}

		private void Divide2x2Layer(string rFile, string wDir, double latMin_0, double latMax_2, double lonMin_0, double lonMax_2, int depth, Action beforeNext)
		{
			Console.WriteLine("< " + rFile);
			Console.WriteLine("> " + wDir);

			double latMid_1 = (latMin_0 + latMax_2) / 2.0;
			double lonMid_1 = (lonMin_0 + lonMax_2) / 2.0;

			AreaInfo sw = new AreaInfo()
			{
				WDir = Path.Combine(wDir, "SW"),
				LatMin = latMin_0,
				LatMax = latMid_1,
				LonMin = lonMin_0,
				LonMax = lonMid_1,
			};

			AreaInfo se = new AreaInfo()
			{
				WDir = Path.Combine(wDir, "SE"),
				LatMin = latMin_0,
				LatMax = latMid_1,
				LonMin = lonMid_1,
				LonMax = lonMax_2,
			};

			AreaInfo nw = new AreaInfo()
			{
				WDir = Path.Combine(wDir, "NW"),
				LatMin = latMid_1,
				LatMax = latMax_2,
				LonMin = lonMin_0,
				LonMax = lonMid_1,
			};

			AreaInfo ne = new AreaInfo()
			{
				WDir = Path.Combine(wDir, "NE"),
				LatMin = latMid_1,
				LatMax = latMax_2,
				LonMin = lonMid_1,
				LonMax = lonMax_2,
			};

			sw.Init();
			se.Init();
			nw.Init();
			ne.Init();

			using (StreamReader reader = new StreamReader(rFile, Encoding.ASCII))
			using (sw.Writer = sw.Open())
			using (se.Writer = se.Open())
			using (nw.Writer = nw.Open())
			using (ne.Writer = ne.Open())
			using (StreamWriter others_writer = new StreamWriter(Path.Combine(wDir, "Others.conved.txt"), false, Encoding.ASCII))
			{
				for (; ; )
				{
					ItemInfo item = ReadItem(reader);

					if (item == null)
						break;

					if (
						sw.WriteItem_IfInside_IsNotWrote(item) &&
						se.WriteItem_IfInside_IsNotWrote(item) &&
						nw.WriteItem_IfInside_IsNotWrote(item) &&
						ne.WriteItem_IfInside_IsNotWrote(item)
						)
						WriteLines(others_writer, item.Lines);
				}
			}

			using (StreamWriter writer = new StreamWriter(Path.Combine(wDir, "Area.txt")))
			{
				WriteArea(writer, latMin_0, latMax_2, lonMin_0, lonMax_2);
				WriteArea(writer, sw);
				WriteArea(writer, se);
				WriteArea(writer, nw);
				WriteArea(writer, ne);
			}

			beforeNext();

			Next(sw, depth);
			Next(se, depth);
			Next(nw, depth);
			Next(ne, depth);
		}

		private void Next(AreaInfo area, int depth)
		{
			if (depth < 20 && IsLineCountOver(area.WFile, 10000))
			{
				Divide2x2Layer(area.WFile, area.WDir, area.LatMin, area.LatMax, area.LonMin, area.LonMax, depth + 1, () => FileTools.Delete(area.WFile));
			}
		}

		private class AreaInfo
		{
			public string WDir;
			public double LatMin;
			public double LatMax;
			public double LonMin;
			public double LonMax;

			// <---- prm

			public string WFile;
			public StreamWriter Writer;

			public void Init()
			{
				FileTools.CreateDir(WDir);
				WFile = Path.Combine(WDir, "Area.conved.txt");
			}

			public StreamWriter Open()
			{
				return new StreamWriter(WFile, false, Encoding.ASCII);
			}

			public bool WriteItem_IfInside_IsNotWrote(ItemInfo item)
			{
				if (
					LatMin + Consts.LAT_LON_MARGIN < item.LatMin && item.LatMax < LatMax - Consts.LAT_LON_MARGIN &&
					LonMin + Consts.LAT_LON_MARGIN < item.LonMin && item.LonMax < LonMax - Consts.LAT_LON_MARGIN
					)
				{
					WriteLines(Writer, item.Lines);
					return false;
				}
				return true;
			}
		}

		private ItemInfo ReadItem(StreamReader reader)
		{
			string line = reader.ReadLine();

			if (line == null)
				return null;

			if (line.Length != 1)
				throw new Exception("不正なレコード識別子です。");

			if (line == "/")
				throw new Exception("レコード開始前にレコードの終端を読み込みました。");

			List<string> dest = new List<string>();
			bool readPoint = false;
			int depth = 1;

			dest.Add(line);

			ItemInfo item = new ItemInfo();

			do
			{
				line = reader.ReadLine();

				if (line == null)
					throw new IOException("レコードの途中でファイルの終端に到達しました。");

				dest.Add(line);

				if (line.Length != 1)
				{
					string[] tokens = line.Split(' ');

					double lat = double.Parse(tokens[0]);
					double lon = double.Parse(tokens[1]);

					item.PointRead(lat, lon);

					readPoint = true;
				}
				else if (line == "/")
				{
					depth--;
				}
				else
				{
					depth++;
				}
			}
			while (1 <= depth);

			if (readPoint == false)
				throw new Exception("レコードに点がありません。");

			item.Lines = dest.ToArray();
			return item;
		}

		private class ItemInfo
		{
			public string[] Lines;
			public double LatMin = double.MaxValue;
			public double LatMax = double.MinValue;
			public double LonMin = double.MaxValue;
			public double LonMax = double.MinValue;

			public void PointRead(double lat, double lon)
			{
				LatMin = Math.Min(LatMin, lat);
				LatMax = Math.Max(LatMax, lat);
				LonMin = Math.Min(LonMin, lon);
				LonMax = Math.Max(LonMax, lon);
			}
		}

		private static bool IsLineCountOver(string file, int threshold)
		{
			using (StreamReader reader = new StreamReader(file, Encoding.ASCII))
			{
				for (int count = 0; ; )
				{
					string line = reader.ReadLine();

					if (line == null)
						return false;

					count++;

					if (threshold < count)
						return true;
				}
			}
		}

		private static void WriteLines(StreamWriter writer, string[] lines)
		{
			foreach (string line in lines)
			{
				writer.WriteLine(line);
			}
		}

		private static void WriteArea(StreamWriter writer, AreaInfo area)
		{
			WriteArea(writer, area.LatMin, area.LatMax, area.LonMin, area.LonMax);
		}

		private static void WriteArea(StreamWriter writer, double latMin, double latMax, double lonMin, double lonMax)
		{
			writer.WriteLine("A");
			writer.WriteLine(latMin.ToString("F9"));
			writer.WriteLine(latMax.ToString("F9"));
			writer.WriteLine(lonMin.ToString("F9"));
			writer.WriteLine(lonMax.ToString("F9"));
			writer.WriteLine("/");
		}
	}
}
