using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Utils
{
	public class KeyCounter
	{
		private Dictionary<string, int> Counters = DictionaryTools.Create<int>();

		public void Clear()
		{
			Counters.Clear();
		}

		public void Add(string key)
		{
			if (Counters.ContainsKey(key))
			{
				Counters[key]++;
			}
			else
			{
				Counters.Add(key, 1);
			}
		}

		public override string ToString()
		{
			string[] keys = Counters.Keys.ToArray();

			Array.Sort(keys, StringTools.Comp);

			List<string> dest = new List<string>();

			foreach (string key in keys)
				dest.Add(key + "=" + Counters[key]);

			return string.Join(", ", dest);
		}
	}
}
