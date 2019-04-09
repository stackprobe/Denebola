using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Charlotte.Tools;

namespace Charlotte
{
	public class ByDiskServer
	{
		public Action<HTTPServerChannel> Connected = channel => { };

		// <---- prm

		private SharedQueue Recver = new SharedQueue("{14b6b817-46ad-464f-913b-541b321fdb90}"); // shared_uuid // Client -> Server
		private SharedQueue Sender = new SharedQueue("{005320f5-00bc-424f-9cb3-220e462d7935}"); // shared_uuid // Client <- Server

		private Thread Th;
		private bool StopFlag = false;

		public void Start()
		{
			Th = new Thread(() =>
			{
				try
				{
					Monitor();
				}
				catch (Exception e)
				{
					ProcMain.WriteLog(e);
				}
			});

			Th.Start();
		}

		private void Monitor()
		{
			while (StopFlag == false)
			{
				foreach (byte[] value in Recver.DequeueAll())
				{
					try
					{
						byte[][] prms = BinTools.Split(value);
						int c = 0;

						byte[] ident = prms[c++];
						string path = Encoding.UTF8.GetString(prms[c++]);
						//byte[] body = prms[c++];

						HTTPServerChannel channel = new HTTPServerChannel();

						channel.Path = path;
						//channel.Body = body;

						Connected(channel);

						Sender.Enqueue(BinTools.SplittableJoin(new byte[][]
						{
							new byte[0], // エラーメッセージ
							ident,
							channel.ResBody_B,
						}));
					}
					catch (Exception e)
					{
						ProcMain.WriteLog(e);

						Sender.Enqueue(BinTools.SplittableJoin(new byte[][]
						{
							Encoding.UTF8.GetBytes("エラー：" + e),
						}));
					}
				}
			}
		}

		public void Stop_B()
		{
			StopFlag = true;

			Th.Join();
			Th = null;
		}
	}
}
