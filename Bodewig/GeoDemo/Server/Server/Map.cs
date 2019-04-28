using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;
using Charlotte.Utils;

namespace Charlotte
{
	public class Map
	{
		public Layer[] Layers = new Layer[Consts.LAYER_NUM];
		public Road Road;
		public AddressStore AddressStore;

		public Map()
		{
			for (int index = 0; index < Consts.LAYER_NUM; index++)
				Layers[index] = new Layer();

			foreach (string file in Directory.GetFiles(Gnd.I.Config.FG_GML_RootDir, "FG-GML-*.xml.conved.txt", SearchOption.AllDirectories))
			{
				if (IsInside_MapFile(file))
				{
					CommonUtils.Print("Loading... " + file);

					CurrTrgFile = file;
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

					CurrTrgFile = null;

					CommonUtils.Print("Loaded");

					GC.Collect();
				}
			}
			CommonUtils.Print("レイヤ読み込み完了！");

			for (int index = 0; index < Consts.LAYER_NUM; index++)
			{
				CommonUtils.Print("レイヤ展開中... " + (index + 1) + " / " + Consts.LAYER_NUM);

				Layers[index].Lines = Layers[index].Wk_Lines.ToArray();
				Layers[index].Wk_Lines = null;

				GC.Collect();

				Layers[index].Points = Layers[index].Wk_Points.ToArray();
				Layers[index].Wk_Points = null;

				GC.Collect();

				Layers[index].LineMgm = new MeshedGroupManager<MapLine>(item => new double[]
				{
					Math.Min(item.A.X, item.B.X),
					Math.Min(item.A.Y, item.B.Y),
					Math.Max(item.A.X, item.B.X),
					Math.Max(item.A.Y, item.B.Y),
				});

				Layers[index].LineMgm.SetItems(Layers[index].Lines);
				Layers[index].Lines = null;

				GC.Collect();

				Layers[index].PointMgm = new MeshedGroupManager<MapPoint>(item => new double[] { item.X, item.Y, item.X, item.Y });
				Layers[index].PointMgm.SetItems(Layers[index].Points);
				Layers[index].Points = null;

				GC.Collect();

				CommonUtils.Print("枠内ヒット率_L: " + Layers[index].LineMgm.Get枠内ヒット率());
				CommonUtils.Print("枠内ヒット率_P: " + Layers[index].PointMgm.Get枠内ヒット率());
			}
			CommonUtils.Print("レイヤ読み込み(展開)完了！");

			// レイヤ ここまで

			Road = new Road();

			foreach (string file in Directory.GetFiles(Gnd.I.Config.MRD_RootDir, "*.mrd.conved.txt"))
			{
				//if (IsInside_RoadFile(file)) // XXX -- MRDの西偏が考慮されていないので、とりあえず全部読み込んでしまう。
				{
					using (StreamReader reader = new StreamReader(file, Encoding.ASCII))
					{
						for (; ; )
						{
							string line = reader.ReadLine();

							if (line == null)
								break;

							if (line != "N")
								throw new Exception("不明なレコード識別子です。" + line);

							Road.Node node = new Road.Node();

							{
								string code = reader.ReadLine();

								if (IsFairRoadNodeCode(code) == false)
									throw new Exception("不正な道路ノードのコードです。" + code);

								node.Code = code;
							}

							GeoPoint gPoint;

							{
								string[] tokens = reader.ReadLine().Split(' ');

								double lat = double.Parse(tokens[0]);
								double lon = double.Parse(tokens[1]);

								lat += Gnd.I.Config.MRD_CORRECT_LAT;
								lon += Gnd.I.Config.MRD_CORRECT_LON;

								gPoint = new GeoPoint(lat, lon);
							}

							for (; ; )
							{
								string code = reader.ReadLine();

								if (code == "/")
									break;

								if (IsFairRoadNodeCode(code) == false)
									throw new Exception("不正な道路ノードのリンクコードです。" + code);

								node.Wk_LinkCodes.Add(code);
							}

							if (IsInside(gPoint))
							{
								node.Point = new MapPoint(gPoint);

								Road.Wk_Nodes.Add(node.Code, node);
							}
						}
					}
				}
			}
			foreach (Road.Node node in Road.Wk_Nodes.Values)
			{
				for (int index = 0; index < node.Wk_LinkCodes.Count; index++)
				{
					string linkCode = node.Wk_LinkCodes[index];

					if (Road.Wk_Nodes.ContainsKey(linkCode) == false)
					{
						node.Wk_LinkCodes.RemoveAt(index);
						index--;
					}
				}
				node.Links = new Road.Node[node.Wk_LinkCodes.Count];

				for (int index = 0; index < node.Wk_LinkCodes.Count; index++)
				{
					node.Links[index] = Road.Wk_Nodes[node.Wk_LinkCodes[index]];
				}
				node.Wk_LinkCodes = null;
			}
			GC.Collect();

			Road.Nodes = Road.Wk_Nodes.Values.ToArray();
			Road.Wk_Nodes = null;

			GC.Collect();

			CommonUtils.Print("道路読み込み完了！");

			// 道路 ここまで

			foreach (Road.Node node in Road.Nodes)
			{
				foreach (Road.Node link in node.Links)
				{
					if (StringTools.Comp(node.Code, link.Code) < 0) // ? node < link
					{
						Road.Wk_Branches.Add(new Road.Branch()
						{
							A = node,
							B = link,
						});
					}
				}
			}
			Road.Branches = Road.Wk_Branches.ToArray();
			Road.Wk_Branches = null;

			GC.Collect();

			CommonUtils.Print("道路(ブランチ)読み込み完了！");

			Road.BranchMgm = new MeshedGroupManager<Road.Branch>(item => new double[]
			{
				Math.Min(item.A.Point.X,item.B.Point.X),
				Math.Min(item.A.Point.Y,item.B.Point.Y),
				Math.Max(item.A.Point.X,item.B.Point.X),
				Math.Max(item.A.Point.Y,item.B.Point.Y),
			});

			Road.BranchMgm.SetItems(Road.Branches);
			Road.Branches = null;

			GC.Collect();

			CommonUtils.Print("道路(ブランチ)読み込み(展開)完了！");

			// 道路のブランチ ここまで

			AddressStore = new AddressStore();

			{
				int latMin = (int)Gnd.I.Config.GPoint_SE.Lat;
				int latMax = (int)Gnd.I.Config.GPoint_NW.Lat;
				int lonMin = (int)Gnd.I.Config.GPoint_NW.Lon;
				int lonMax = (int)Gnd.I.Config.GPoint_SE.Lon;

				foreach (string file in Directory.GetFiles(Gnd.I.Config.Address_RootDir, "*.conved.txt"))
				{
					string lFile = Path.GetFileName(file);

					int lat = int.Parse(lFile.Substring(0, 2));
					int lon = int.Parse(lFile.Substring(2, 3));

					if (
						latMin <= lat && lat <= latMax &&
						lonMin <= lon && lon <= lonMax
						)
					{
						using (StreamReader reader = new StreamReader(file, StringTools.ENCODING_SJIS))
						{
							for (; ; )
							{
								string line = reader.ReadLine();

								if (line == null)
									break;

								if (line != "A")
									throw new Exception("不明なレコード識別子です。" + line);

								string 都道府県名 = reader.ReadLine();
								string 市区町村名 = reader.ReadLine();
								string 大字丁目名 = reader.ReadLine();
								string 小字通称名 = reader.ReadLine();
								string 地番 = reader.ReadLine();
								string sLat = reader.ReadLine();
								string sLon = reader.ReadLine();

								if (reader.ReadLine() != "/")
									throw new Exception("レコードが壊れています。");

								AddressStore.AddressInfo ai = new AddressStore.AddressInfo();

								ai.Address = string.Join(" ", 都道府県名, 市区町村名, 大字丁目名, 小字通称名, 地番);
								ai.Point = new MapPoint(new GeoPoint(
									double.Parse(sLat),
									double.Parse(sLon)
									));

								AddressStore.Wk_Addresses.Add(ai);
							}
						}
					}
				}
			}

			AddressStore.Addresses = AddressStore.Wk_Addresses.ToArray();
			AddressStore.Wk_Addresses = null;

			GC.Collect();

			CommonUtils.Print("住所読み込み完了！");

			// 住所 ここまで
		}

		private bool IsInside_MapFile(string file) // ex. ...\FG-GML-533827-AdmArea-20171001-0001.xml.conved.txt
		{
			return IsInside_Mesh(StringTools.GetEnclosed(Path.GetFileName(file), "FG-GML-", "-").Inner);
		}

		private bool IsInside_RoadFile(string file) // ex. ...\533924.mrd.conved.txt
		{
			return IsInside_Mesh(Path.GetFileName(file).Substring(0, 6));
		}

		private bool IsInside_Mesh(string code)
		{
			Dai2JiMesh mesh = new Dai2JiMesh(code);

			double latMin = Gnd.I.Config.GPoint_SE.Lat;
			double latMax = Gnd.I.Config.GPoint_NW.Lat;
			double lonMin = Gnd.I.Config.GPoint_NW.Lon;
			double lonMax = Gnd.I.Config.GPoint_SE.Lon;

			return CrashUtils.IsCrashed_Rect_Rect(
				mesh.LonMin,
				mesh.LatMin,
				mesh.LonMax,
				mesh.LatMax,
				lonMin,
				latMin,
				lonMax,
				latMax
				);
		}

		private string CurrTrgFile;

		private void LoadLayer(string subRootName, int index)
		{
			if (StringTools.ContainsIgnoreCase(Path.GetFileName(CurrTrgFile), string.Format("-{0}-", subRootName)))
			{
				using (StreamReader reader = new StreamReader(CurrTrgFile, Encoding.UTF8))
				{
					for (; ; )
					{
						string line = reader.ReadLine();

						if (line == null)
							break;

						if (line == "M")
						{
							List<string> lines = new List<string>();

							for (; ; )
							{
								line = reader.ReadLine();

								if (line == "/")
									break;

								lines.Add(line);
							}
							if (lines.Count < 2) throw null; // 2bs

							LoadPolygon(Layers[index], lines);
						}
						else if (line == "S")
						{
							LoadPoint(Layers[index], reader.ReadLine());
						}
						else
						{
							throw null; // 2bs
						}
					}
				}
			}
		}

		private void LoadPolygon(Layer layer, List<string> lines)
		{
			GeoPoint lastGPoint = null;

			foreach (string line in lines)
			{
				GeoPoint gPoint = CreatePoint(line);

				if (lastGPoint != null)
				{
					GeoLine gLine = new GeoLine(lastGPoint, gPoint);

					if (IsInside(gLine.A) || IsInside(gLine.B))
					{
						layer.Wk_Lines.Add(new MapLine(gLine));
					}
				}
				lastGPoint = gPoint;
			}
		}

		private void LoadPoint(Layer layer, string value)
		{
			GeoPoint gPoint = CreatePoint(value);

			if (IsInside(gPoint))
			{
				layer.Wk_Points.Add(new MapPoint(gPoint));
			}
		}

		private GeoPoint CreatePoint(string line)
		{
			string[] tokens = line.Split(' ');

			return new GeoPoint(
				 double.Parse(tokens[0]),
				 double.Parse(tokens[1])
				 );
		}

		private bool IsInside(GeoPoint gPoint)
		{
			return
				Gnd.I.Config.GPoint_NW.Lon <= gPoint.Lon && gPoint.Lon <= Gnd.I.Config.GPoint_SE.Lon &&
				Gnd.I.Config.GPoint_SE.Lat <= gPoint.Lat && gPoint.Lat <= Gnd.I.Config.GPoint_NW.Lat;
		}

		private bool IsFairRoadNodeCode(string code)
		{
			return StringTools.ReplaceChars(code, StringTools.DECIMAL, '9') == "999999:99999";
		}
	}
}
