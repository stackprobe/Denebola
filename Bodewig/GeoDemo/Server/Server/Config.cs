using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class Config
	{
		public int PortNo = 59999;

		public GeoPoint GOrigin = new GeoPoint(35.6666, 139.713); // 青山鈴木硝子ビルの裏手
		//public GeoPoint GOrigin = new GeoPoint(35.69, 139.7); // 新宿駅
		// 朝霞台～川崎・TDR
		//public GeoPoint GPoint_NW = new GeoPoint(35.8, 139.6); // 北西端 == XY最小値
		//public GeoPoint GPoint_SE = new GeoPoint(35.5, 139.9); // 南東端 == XY最大値
		// 新高円寺～お台場・荒川河口
		//public GeoPoint GPoint_NW = new GeoPoint(35.7, 139.65); // 北西端 == XY最小値
		//public GeoPoint GPoint_SE = new GeoPoint(35.6, 139.85); // 南東端 == XY最大値
		// 地域メッシュ 533945, 533935 の 緯度経度 0.003° 内側の範囲
		public GeoPoint GPoint_NW = new GeoPoint(35.747, 139.628); // 北西端 == XY最小値
		public GeoPoint GPoint_SE = new GeoPoint(35.586, 139.747); // 南東端 == XY最大値
		// 地域メッシュ 533945 の 緯度経度 0.003° 内側の範囲
		//public GeoPoint GPoint_NW = new GeoPoint(35.747, 139.628); // 北西端 == XY最小値
		//public GeoPoint GPoint_SE = new GeoPoint(35.669, 139.747); // 南東端 == XY最大値
		// 道路全部 -- 読み込めない。
		//public GeoPoint GPoint_NW = new GeoPoint(35.833332, 139.500001); // 北西端 == XY最小値
		//public GeoPoint GPoint_SE = new GeoPoint(35.500001, 139.749999); // 南東端 == XY最大値
		// 道路上半分
		//public GeoPoint GPoint_NW = new GeoPoint(35.833332, 139.500001); // 北西端 == XY最小値
		//public GeoPoint GPoint_SE = new GeoPoint(35.666667, 139.749999); // 南東端 == XY最大値
		// 道路下半分
		//public GeoPoint GPoint_NW = new GeoPoint(35.666665, 139.500001); // 北西端 == XY最小値
		//public GeoPoint GPoint_SE = new GeoPoint(35.500001, 139.749999); // 南東端 == XY最大値

		/// <summary>
		/// 国土地理院_基盤地図情報データ を ConvTokyoMap に掛けた後のルートディレクトリ
		/// https://fgd.gsi.go.jp/download/menu.php
		/// </summary>
		public string FG_GML_RootDir = @"C:\var2\res\東京都地図T";

		/// <summary>
		/// MAPPLE道路ネットワークデータ(.mrd) を ConvTokyoRoad に掛けた後のルートディレクトリ
		/// </summary>
		public string MRD_RootDir = @"C:\var2\res\東京道路網T";

		// MRDの西偏補正(緯度経度)
		public double MRD_CORRECT_LON = -0.00323;
		public double MRD_CORRECT_LAT = 0.00325;

		/// <summary>
		/// 国土交通省国土政策局国土情報課_街区レベル位置参照情報 を ConvAddress に掛けた後のルートディレクトリ
		/// </summary>
		public string Address_RootDir = @"C:\var2\res\全国住所緯度経度T";

		public void LoadFile(string file)
		{
			XmlNode root = XmlNode.LoadFile(file);

			PortNo = int.Parse(root.Get("PortNo").Value);
			GOrigin = ToGeoPoint(root.Get("GOrigin"));
			GPoint_NW = ToGeoPoint(root.Get("GPoint_NW"));
			GPoint_SE = ToGeoPoint(root.Get("GPoint_SE"));
			FG_GML_RootDir = root.Get("FG_GML_RootDir").Value;
			MRD_RootDir = root.Get("MRD_RootDir").Value;
			MRD_CORRECT_LON = double.Parse(root.Get("MRD_CORRECT_LON").Value);
			MRD_CORRECT_LAT = double.Parse(root.Get("MRD_CORRECT_LAT").Value);
			Address_RootDir = root.Get("Address_RootDir").Value;
		}

		private GeoPoint ToGeoPoint(XmlNode node)
		{
			double lat = double.Parse(node.Get("LAT").Value);
			double lon = double.Parse(node.Get("LON").Value);

			if (lat < 20.0 || 46.0 < lat)
				throw new InvalidDataException("不正な緯度です。" + lat); // 日本の最南北端 20°25′31″ 45°33′26″

			if (lon < 122.0 || 154.0 < lon)
				throw new InvalidDataException("不正な経度です。" + lon); // 日本の最西東端 122°56′01″ 153°59′11″

			return new GeoPoint(lat, lon);
		}

		public void ValidationCheck()
		{
			if (PortNo < 1 || 65535 < PortNo) throw new InvalidDataException();
			// GOrigin
			if (GPoint_NW.Lat - 0.001 < GPoint_SE.Lat) throw new InvalidDataException();
			if (GPoint_SE.Lon - 0.001 < GPoint_NW.Lon) throw new InvalidDataException();
			if (Directory.Exists(FG_GML_RootDir) == false) throw new InvalidDataException();
			if (Directory.Exists(MRD_RootDir) == false) throw new InvalidDataException();
			if (MRD_CORRECT_LON < -100.0 || 100.0 < MRD_CORRECT_LON) throw new InvalidDataException();
			if (MRD_CORRECT_LAT < -100.0 || 100.0 < MRD_CORRECT_LAT) throw new InvalidDataException();
			if (Directory.Exists(Address_RootDir) == false) throw new InvalidDataException();
		}
	}
}
