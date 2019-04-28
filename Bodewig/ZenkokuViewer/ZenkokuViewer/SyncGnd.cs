using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte
{
	public class SyncGnd
	{
		private static object SYNCROOT = new object();
		private static SyncGnd _i = null;
		private static bool _i_inited = false;

		public static SyncGnd I
		{
			get
			{
				if (_i_inited == false)
				{
					lock (SYNCROOT)
					{
						if (_i_inited == false)
						{
							_i = new SyncGnd();
							_i_inited = true;
						}
					}
				}
				return _i;
			}
		}

		public SyncValue<double> UsedMemoryMB = new SyncValue<double>(-1.0);
	}
}
