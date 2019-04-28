using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte
{
	public class MeshedGroup<ItemType>
	{
		public double L;
		public double T;
		public double R;
		public double B;

		public List<ItemType> Wk_Items = new List<ItemType>();
		public ItemType[] Items;
	}
}
