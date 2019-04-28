using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte.Utils
{
	public class StringKeyCache<V>
	{
		private Dictionary<string, V> Values;
		private Func<string, V> CreateValue;
		private Queue<string> Keys = new Queue<string>();

		public StringKeyCache(bool ignoreCase, Func<string, V> createValue)
		{
			this.Values = ignoreCase ? DictionaryTools.CreateIgnoreCase<V>() : DictionaryTools.Create<V>();
			this.CreateValue = createValue;
		}

		public void Clear()
		{
			this.Values.Clear();
			this.Keys.Clear();
		}

		public V Get(string key)
		{
			if (this.Values.ContainsKey(key) == false)
			{
				this.Values.Add(key, this.CreateValue(key));
				this.Keys.Enqueue(key);
			}
			return this.Values[key];
		}
	}
}
