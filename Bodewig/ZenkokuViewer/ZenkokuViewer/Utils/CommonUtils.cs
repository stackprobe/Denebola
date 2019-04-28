using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Utils
{
	public class CommonUtils
	{
		public static int IndexOf<T>(List<T> list, Func<T, bool> predicate, int defval = -1)
		{
			for (int index = 0; index < list.Count; index++)
				if (predicate(list[index]))
					return index;

			return defval;
		}
	}
}
