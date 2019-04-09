using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Charlotte.Tools;

namespace Charlotte.Utils
{
	public class HTTPServer_T : HTTPServer
	{
		public HTTPServer_T()
		{
			Interlude = () => Alive;
		}

		private ThreadEx Th;
		private bool Alive;

		public void Start()
		{
			Alive = true;
			Th = new ThreadEx(Perform);
		}

		public void Stop_B()
		{
			Alive = false;
			Th.WaitToEnd();
			Th = null;
		}
	}
}
