using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte
{
	public class ByDiskClient : IDisposable
	{
		private SharedQueue Sender = new SharedQueue("{14b6b817-46ad-464f-913b-541b321fdb90}"); // shared_uuid // Client -> Server
		private SharedQueue Recver = new SharedQueue("{005320f5-00bc-424f-9cb3-220e462d7935}"); // shared_uuid // Client <- Server

		public byte[] Invoke(string path)
		{
			byte[] ident = SecurityTools.CRandom.GetBytes(16);

			Sender.Enqueue(BinTools.SplittableJoin(
				ident,
				Encoding.UTF8.GetBytes(path)
				));

			DateTime startedTime = DateTime.Now;

			while ((DateTime.Now - startedTime).TotalSeconds < 5)
			{
				foreach (byte[] value in Recver.DequeueAll())
				{
					byte[][] rets = BinTools.Split(value);
					int c = 0;

					string errorMessage = Encoding.UTF8.GetString(rets[c++]);

					if (errorMessage != "")
						throw new Exception("errorMessage: " + errorMessage);

					byte[] resIdent = rets[c++];
					byte[] resBody = rets[c++];

					if (BinTools.Comp(resIdent, ident) == 0)
					{
						return resBody;
					}
				}
			}
			Sender.Clear(); // サーバー停止中かも？不要なリクエストが溜まらないようにクリアする。
			return null;
		}

		public void Dispose()
		{
			if (Sender != null)
			{
				Sender.Dispose();
				Recver.Dispose();

				Sender = null;
			}
		}
	}
}
