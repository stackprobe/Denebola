using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utils;

namespace Charlotte
{
	public class Router
	{
		public MapPoint StartPoint;
		public MapPoint EndPoint;

		// <---- prm

		public Road.Node StartNode;
		public Road.Node EndNode;

		/// <summary>
		/// StartNode から EndNode まで。StartNode と EndNode を含む。
		/// StartNode == EndNode の場合 --> { StartNode }
		/// </summary>
		public List<Road.Node> RouteNodes;

		public double RouteDistance = double.MaxValue;

		// ---- MultiRouter 用 ----

		public int MltRt_StartPlotIndex;
		public int MltRt_EndPlotIndex;

		// ----

		public Router GetReverse()
		{
			Router ret = new Router()
			{
				StartPoint = this.EndPoint,
				EndPoint = this.StartPoint,

				StartNode = this.EndNode,
				EndNode = this.StartNode,

				RouteNodes = new List<Road.Node>(this.RouteNodes),

				RouteDistance = this.RouteDistance,

				MltRt_StartPlotIndex = this.MltRt_EndPlotIndex,
				MltRt_EndPlotIndex = this.MltRt_StartPlotIndex,
			};

			ret.RouteNodes.Reverse();

			return ret;
		}

		public void Invoke()
		{
			Road.Node startNodePrevNode = new Road.Node();

			StartNode = GetNearestNode(StartPoint);
			EndNode = GetNearestNode(EndPoint);

			if (StartNode == EndNode)
			{
				RouteNodes = new List<Road.Node>(new Road.Node[] { StartNode });
				return;
			}

			foreach (Road.Node node in Gnd.I.Map.Road.Nodes)
			{
				node.Rt_PrevNode = null;
				node.Rt_Distance = double.MaxValue;
			}

			StartNode.Rt_PrevNode = startNodePrevNode;
			StartNode.Rt_Distance = 0.0;

			Queue<Road.Node> nodes = new Queue<Road.Node>();

			nodes.Enqueue(StartNode);

			while (1 <= nodes.Count)
			{
				Road.Node node = nodes.Dequeue();

				foreach (Road.Node link in node.Links)
				{
					double d = node.Rt_Distance + CrashUtils.GetDistance(node.Point, link.Point);

					if (d < link.Rt_Distance && d < RouteDistance)
					{
						link.Rt_PrevNode = node;
						link.Rt_Distance = d;

						if (link == EndNode)
						{
							RouteDistance = d;
						}
						else
						{
							nodes.Enqueue(link);
						}
					}
				}
			}

			if (EndNode.Rt_PrevNode == null)
				throw new Exception("指定された2地点は接続していません。");

			RouteNodes = new List<Road.Node>();

			{
				Road.Node node = EndNode;

				while (node != startNodePrevNode)
				{
					RouteNodes.Add(node);
					node = node.Rt_PrevNode;
				}
			}

			RouteNodes.Reverse();
		}

		private Road.Node GetNearestNode(MapPoint point)
		{
			Road.Node retNode = null;
			double retD = double.MaxValue;

			foreach (Road.Node node in Gnd.I.Map.Road.Nodes)
			{
				double d = CrashUtils.GetDistance(node.Point, point);

				if (d < retD)
				{
					retNode = node;
					retD = d;
				}
			}
			return retNode;
		}
	}
}
