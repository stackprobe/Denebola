using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Charlotte.Layer.MapLayer
{
	public class ConvedFile
	{
		public GeoPoint[] Points;
		public GeoCurve[] Curves;
		public GeoSurface[] Surfaces;

		public ConvedFile(string file)
		{
			List<GeoPoint> points = new List<GeoPoint>();
			List<GeoCurve> curves = new List<GeoCurve>();
			List<GeoSurface> surfaces = new List<GeoSurface>();

			using (StreamReader reader = new StreamReader(file, Encoding.ASCII))
			{
				for (; ; )
				{
					string line = reader.ReadLine();

					if (line == null)
						break;

					if (line == "P")
					{
						points.Add(ReadToSlash(reader)[0]);
					}
					else if (line == "C")
					{
						curves.Add(new GeoCurve(ReadToSlash(reader)));
					}
					else if (line == "S")
					{
						if (reader.ReadLine() != "E")
							throw new Exception("不明なデータ識別子(E)です。" + line);

						GeoCurve exterior = new GeoCurve(ReadToSlash(reader));
						GeoCurve[] interiors;

						{
							List<GeoCurve> dest = new List<GeoCurve>();

							for (; ; )
							{
								line = reader.ReadLine();

								if (line == "/")
									break;

								if (line != "I")
									throw new Exception("不明なデータ識別(I)です。" + line);

								dest.Add(new GeoCurve(ReadToSlash(reader)));
							}
							interiors = dest.ToArray();
						}

						surfaces.Add(new GeoSurface(exterior, interiors));
					}
					else
					{
						throw new Exception("不明なデータ識別子です。" + line);
					}
				}
			}

			this.Points = points.ToArray();
			this.Curves = curves.ToArray();
			this.Surfaces = surfaces.ToArray();
		}

		private static GeoPoint[] ReadToSlash(StreamReader reader)
		{
			List<GeoPoint> dest = new List<GeoPoint>();

			for (; ; )
			{
				string line = reader.ReadLine();

				if (line == "/")
					break;

				dest.Add(LineToGeoPoint(line));
			}
			return dest.ToArray();
		}

		private static GeoPoint LineToGeoPoint(string line)
		{
			string[] tokens = line.Split(' ');

			double lat = double.Parse(tokens[0]);
			double lon = double.Parse(tokens[1]);

			return new GeoPoint(lat, lon);
		}
	}
}
