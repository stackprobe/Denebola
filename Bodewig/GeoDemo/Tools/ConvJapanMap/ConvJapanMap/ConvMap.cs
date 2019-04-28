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
			foreach (string n in Consts.LayerNames)
				FileTools.CreateDir(Path.Combine(W_Dir, n));

			foreach (string file in Directory.GetFiles(R_Dir, "FG-GML-*.xml", SearchOption.AllDirectories))
			{
				Console.WriteLine("file: " + file);

				LayerName = Path.GetFileName(file).Split('-')[3];

				Console.WriteLine("LayerName: " + LayerName);

				if (ArrayTools.Contains(Consts.LayerNames, n => StringTools.EqualsIgnoreCase(n, LayerName)) == false)
					throw new Exception("不明なレイヤ名");

				Root = XmlNode.LoadFile(file);

				Writer = new StreamWriter(Path.Combine(W_Dir, LayerName, "All.conved.txt"), true, Encoding.ASCII);
				try
				{
					LoadLayer();
				}
				finally
				{
					Writer.Dispose();
					Writer = null;
				}

				Console.WriteLine("OK!");

				LayerName = null;
				Root = null;

				GC.Collect();
			}
		}

		private string LayerName;
		private XmlNode Root;
		private StreamWriter Writer;

		private void LoadLayer()
		{
			foreach (XmlNode layerRoot in Root.Collect(LayerName))
			{
				foreach (XmlNode surface in layerRoot.Collect("area/Surface/patches/PolygonPatch"))
				{
					Writer.WriteLine("S");

					XmlNode exterior = surface.Get("exterior");
					XmlNode[] interiors = surface.Collect("interior");

					Writer.WriteLine("E");
					WritePointList(exterior.Get("Ring/curveMember/Curve/segments/LineStringSegment/posList"));
					Writer.WriteLine("/");

					foreach (XmlNode interior in interiors)
					{
						Writer.WriteLine("I");
						WritePointList(exterior.Get("Ring/curveMember/Curve/segments/LineStringSegment/posList"));
						Writer.WriteLine("/");
					}
					Writer.WriteLine("/");
				}
				foreach (XmlNode curve in layerRoot.Collect("loc/Curve/segments/LineStringSegment/posList"))
				{
					Writer.WriteLine("C");
					WritePointList(curve);
					Writer.WriteLine("/");
				}
				foreach (XmlNode point in layerRoot.Collect("pos/Point/pos"))
				{
					Writer.WriteLine("P");
					WritePointList(point);
					Writer.WriteLine("/");
				}
			}
		}

		private void WritePointList(XmlNode node)
		{
			foreach (string f_line in node.Value.Replace("\r", "").Split('\n'))
			{
				string line = f_line;
				line = line.Trim();
				string[] tokens = line.Split(' ');

				if (tokens.Length != 2)
					throw new Exception("不正なポイント");

				double lat = double.Parse(tokens[0]);
				double lon = double.Parse(tokens[1]);

				if (lat < Consts.LAT_MIN || Consts.LAT_MAX < lat)
					throw new Exception("不正な緯度");

				if (lon < Consts.LON_MIN || Consts.LON_MAX < lon)
					throw new Exception("不正な緯度");

				Writer.WriteLine(line);
				//Writer.WriteLine(lat.ToString("F9") + " " + lon.ToString("F9"));
			}
		}
	}
}
