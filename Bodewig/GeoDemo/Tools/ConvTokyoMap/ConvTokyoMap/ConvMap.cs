using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class ConvMap
	{
		public string R_Dir;
		public string W_Dir;

		// <---- prm

		public void Invoke()
		{
			foreach (string file in Directory.GetFiles(R_Dir, "FG-GML-*.xml", SearchOption.AllDirectories))
			{
				string wFile = FileTools.ChangeRoot(file, R_Dir, W_Dir) + ".conved.txt";
				string wDir = Path.GetDirectoryName(wFile);

				Console.WriteLine("< " + file);
				Console.WriteLine("> " + wFile);

				Directory.CreateDirectory(wDir); // 存在すれば何もしない。

				Writer = new StreamWriter(wFile, false, Encoding.ASCII);
				try
				{
					Root = XmlNode.LoadFile(file);
					int c = 0;

					LoadLayer("AdmArea", c++);
					LoadLayer("AdmBdry", c++);
					LoadLayer("AdmPt", c++);
					LoadLayer("BldA", c++);
					LoadLayer("BldL", c++);
					LoadLayer("Cntr", c++);
					LoadLayer("CommBdry", c++);
					LoadLayer("CommPt", c++);
					LoadLayer("Cstline", c++);
					LoadLayer("ElevPt", c++);
					LoadLayer("GCP", c++);
					LoadLayer("RailCL", c++);
					LoadLayer("RdCompt", c++);
					LoadLayer("RdEdg", c++);
					LoadLayer("SBAPt", c++);
					LoadLayer("SBBdry", c++);
					LoadLayer("WA", c++);
					LoadLayer("WL", c++);
					LoadLayer("WStrA", c++);
					LoadLayer("WStrL", c++);

					Root = null;
				}
				finally
				{
					Writer.Dispose();
					Writer = null;
				}

				Console.WriteLine("Done");

				GC.Collect();
			}
		}

		private XmlNode Root;
		private StreamWriter Writer;

		private void LoadLayer(string subRootName, int layerIndex)
		{
			foreach (XmlNode subRoot in Root.Collect(subRootName))
			{
				foreach (XmlNode node in subRoot.Collect("area/Surface/patches/PolygonPatch/exterior/Ring/curveMember/Curve/segments/LineStringSegment/posList"))
				{
					LoadPolygon(node);
				}
				foreach (XmlNode node in subRoot.Collect("area/Surface/patches/PolygonPatch/interior/Ring/curveMember/Curve/segments/LineStringSegment/posList"))
				{
					LoadPolygon(node);
				}
				foreach (XmlNode node in subRoot.Collect("loc/Curve/segments/LineStringSegment/posList"))
				{
					LoadPolygon(node);
				}
				foreach (XmlNode node in subRoot.Collect("pos/Point/pos"))
				{
					LoadPoint(node);
				}
			}
		}

		private void LoadPolygon(XmlNode node)
		{
			string value = node.Value;
			value = value.Replace("\r", "");
			string[] lines = value.Split('\n');

			if (lines.Length < 2)
				throw new Exception("ポリゴンのくせに複数の座標がありません。" + lines.Length);

			Writer.WriteLine("M");

			foreach (string line in lines)
			{
				CheckGeoPoint_Line(line);

				Writer.WriteLine(line);
			}
			Writer.WriteLine("/");
		}

		private void LoadPoint(XmlNode node)
		{
			string line = node.Value;

			CheckGeoPoint_Line(line);

			Writer.WriteLine("S");
			Writer.WriteLine(line);
		}

		private void CheckGeoPoint_Line(string line)
		{
			string[] tokens = line.Split(' ');

			if (tokens.Length != 2)
				throw new Exception("不正な行(座標)です。" + line);

			double lat = double.Parse(tokens[0]);
			double lon = double.Parse(tokens[1]);

			if (lat < 0.0 || 90.0 < lat)
				throw new Exception("不正な緯度(北緯)です。" + lat);

			if (lon < 0.0 || 180.0 < lon)
				throw new Exception("不正な経度(東経)です。" + lon);
		}
	}
}
