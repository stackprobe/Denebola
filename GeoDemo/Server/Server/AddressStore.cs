using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class AddressStore
	{
		public List<AddressInfo> Wk_Addresses = new List<AddressInfo>();
		public AddressInfo[] Addresses;

		public class AddressInfo
		{
			public string Address; // 都道府県名 + " " + 市区町村名 + " " + 大字丁目名 + " " + 小字通称名 + " " + 地番
			public MapPoint Point;
		}
	}
}
