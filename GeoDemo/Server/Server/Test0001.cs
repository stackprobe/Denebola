using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utils;

namespace Charlotte
{
	public class Test0001
	{
		public void Test01()
		{
			Gnd.I.ConfigChanged();

			Router router = new Router()
			{
				//StartPoint = new MapPoint(-6473, 2312), // 新宿駅中心 533945 の 左下
				//EndPoint = new MapPoint(4013, -6201), // 新宿駅中心 533945 の 右上

				StartPoint = new MapPoint(-1500, -1500),
				EndPoint = new MapPoint(1500, 1500),
			};

			DateTime startTm = DateTime.Now;
			router.Invoke();
			DateTime endTm = DateTime.Now;

			foreach (Road.Node node in router.RouteNodes)
				CommonUtils.Print(node.Point.ToString());

			CommonUtils.Print("経由ノード数：" + router.RouteNodes.Count);
			CommonUtils.Print("道のり：" + router.RouteDistance);

			CommonUtils.Print("ルート検索_処理時間：" + ((endTm - startTm).TotalMilliseconds / 1000.0));
		}

		public void Test02()
		{
			Gnd.I.ConfigChanged();

			MultiRouter mr = new MultiRouter();

#if true // プロット数 8
			for (int x = 0; x < 2; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					mr.PlotPoints.Add(new MapPoint(x * 1000.0, y * 1000.0));
				}
			}
#else // プロット数 25
			for (int x = -2; x <= 2; x++)
			{
				for (int y = -2; y <= 2; y++)
				{
					mr.PlotPoints.Add(new MapPoint(x * 1000.0, y * 1000.0));
				}
			}
#endif

			DateTime startTm = DateTime.Now;
			mr.Invoke();
			DateTime endTm = DateTime.Now;

			foreach (Router router in mr.Routers)
			{
				CommonUtils.Print(router.MltRt_StartPlotIndex + " -> " + router.MltRt_EndPlotIndex + " " + router.RouteDistance + "m (" + router.RouteNodes.Count + ")");
			}
			CommonUtils.Print("ルート検索_処理時間：" + ((endTm - startTm).TotalMilliseconds / 1000.0));
		}
	}
}
