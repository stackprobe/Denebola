using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Utils;
using Charlotte.Tools;

namespace Charlotte
{
	public class MultiRouter
	{
		public List<MapPoint> PlotPoints = new List<MapPoint>();

		// <---- prm

		public Router[] Routers;

		public void Invoke()
		{
			if (PlotPoints.Count < 3)
				throw new Exception("少なくとも3つプロットして下さい。");

			if (PlotPoints.Count <= 8) // 総当たりはプロット数8まで
			{
				ExhaustiveSearch();
			}
			//else if (PlotPoints.Count <= 40)
			//{
			//InsertionSearch(); // TODO
			//}
			else
			{
				NearestNeighborSearch();
			}
		}

		private class NNS_Entry
		{
			public int NextPlotIndex;
			public double StraightDistance;
		}

		private void NearestNeighborSearch()
		{
			int routerNum = PlotPoints.Count; // 閉路上のルート数はプロット数と同じ。
			Routers = new Router[routerNum];
			int currPlotIndex = 0;

			for (int routerIndex = 0; routerIndex < routerNum; routerIndex++)
			{
				List<NNS_Entry> entries = new List<NNS_Entry>();

				for (int nextPlotIndex = 0; nextPlotIndex < routerNum; nextPlotIndex++)
				{
					if (currPlotIndex == nextPlotIndex) // 現在地と同じプロットはng
						continue;

					if (routerIndex + 1 < routerNum) // ? 途中のルート
					{
						int index;

						for (index = 0; index < routerIndex; index++)
							if (Routers[index].MltRt_StartPlotIndex == nextPlotIndex) // 既に立ち寄ったプロットはng
								break;

						if (index < routerIndex)
							continue;
					}
					else // ? 最後のルート
					{
						if (nextPlotIndex != 0) // A(出発点)以外はng
							continue;
					}

					entries.Add(new NNS_Entry()
					{
						NextPlotIndex = nextPlotIndex,
						StraightDistance = CrashUtils.GetDistance(PlotPoints[currPlotIndex], PlotPoints[nextPlotIndex]),
					});
				}

				entries.Sort((a, b) => DoubleTools.Comp(a.StraightDistance, b.StraightDistance));

				int count = Math.Min(3, entries.Count);

				Router nextRouter = null;

				for (int index = 0; index < count; index++)
				{
					Router router = new Router()
					{
						StartPoint = PlotPoints[currPlotIndex],
						EndPoint = PlotPoints[entries[index].NextPlotIndex],

						MltRt_StartPlotIndex = currPlotIndex,
						MltRt_EndPlotIndex = entries[index].NextPlotIndex,
					};

					router.Invoke();

					if (nextRouter == null || router.RouteDistance < nextRouter.RouteDistance)
						nextRouter = router;
				}

				Routers[routerIndex] = nextRouter;
				currPlotIndex = nextRouter.MltRt_EndPlotIndex;
			}
		}

		private Router[][] RouterTable; // [From][To]

		private void ExhaustiveSearch()
		{
			RouterTable = new Router[PlotPoints.Count][];

			for (int startPlotIndex = 0; startPlotIndex < PlotPoints.Count; startPlotIndex++)
			{
				RouterTable[startPlotIndex] = new Router[PlotPoints.Count];

				for (int endPlotIndex = 0; endPlotIndex < PlotPoints.Count; endPlotIndex++)
				{
					if (
						RouterTable[endPlotIndex] != null &&
						RouterTable[endPlotIndex][startPlotIndex] != null
						)
					{
						RouterTable[startPlotIndex][endPlotIndex] = RouterTable[endPlotIndex][startPlotIndex].GetReverse();
					}
					else
					{
						Router router = new Router()
						{
							StartPoint = PlotPoints[startPlotIndex],
							EndPoint = PlotPoints[endPlotIndex],

							MltRt_StartPlotIndex = startPlotIndex,
							MltRt_EndPlotIndex = endPlotIndex,
						};

						router.Invoke();

						RouterTable[startPlotIndex][endPlotIndex] = router;
					}
				}
			}

			Routers = new Router[PlotPoints.Count];
			ShortestRouters = new Router[PlotPoints.Count];
			ShortestDistance = double.MaxValue;

			ES_Search();

			Routers = ShortestRouters;

			RouterTable = null;
		}

		private Router[] ShortestRouters;
		private double ShortestDistance;

		private void ES_Search()
		{
			int index = 0;
			bool ahead = true;

			for (; ; )
			{
				int startPlotIndex = index == 0 ? 0 : Routers[index - 1].MltRt_EndPlotIndex;

				if (ahead)
				{
					if (ES_Check(index) == false)
					{
						ahead = false;
					}
					else if (PlotPoints.Count <= index)
					{
						ES_Found();
						ahead = false;
					}
					else
					{
						Routers[index] = RouterTable[startPlotIndex][0];
					}
				}
				else
				{
					if (Routers[index].MltRt_EndPlotIndex + 1 < PlotPoints.Count)
					{
						Routers[index] = RouterTable[startPlotIndex][Routers[index].MltRt_EndPlotIndex + 1];
						ahead = true;
					}
					else
					{
						// noop
					}
				}

				if (ahead)
				{
					index++;
				}
				else
				{
					index--;

					if (index < 0)
						break;
				}
			}
		}

		private bool ES_Check(int count)
		{
			if (count == 0)
				return true;

			Router router = Routers[count - 1];

			if (count < PlotPoints.Count) // ? 途中のルート
			{
				for (int index = 0; index < count; index++)
					if (Routers[index].MltRt_StartPlotIndex == router.MltRt_EndPlotIndex) // 既に立ち寄ったプロットはng
						return false;
			}
			else // ? 最後のルート
			{
				if (router.MltRt_EndPlotIndex != 0) // A(出発点)以外はng
					return false;
			}
			return true;
		}

		private void ES_Found()
		{
			double d = 0.0;

			foreach (Router router in Routers)
				d += router.RouteDistance;

			if (d < ShortestDistance)
			{
				Array.Copy(Routers, ShortestRouters, PlotPoints.Count);
				ShortestDistance = d;
			}
		}
	}
}
