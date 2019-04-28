using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Charlotte.Tools;
using Charlotte.Utils;

namespace Charlotte.Layer.MapLayer
{
	public class TileDrawer
	{
		public Graphics TileGraphics;
		public GeoRectangle TileRect;
		public ConvedFile[] ConvedFiles;

		public Pen PenColorPen;
		public Brush PenColorBrush;
		public Brush BrushColorBrush;

		// <---- prm

		private double TileRate_X;
		private double TileRate_Y;

		private ImageRectangle TileImageRect;

		public void Invoke()
		{
			TileRate_X = Consts.TILE_WH / (TileRect.LonMax - TileRect.LonMin);
			TileRate_Y = Consts.TILE_WH / (TileRect.LatMax - TileRect.LatMin);

			{
				int MARGIN = 10;

				TileImageRect = new ImageRectangle()
				{
					L = 0 - MARGIN,
					T = 0 - MARGIN,
					R = Consts.TILE_WH + MARGIN,
					B = Consts.TILE_WH + MARGIN,
				};
			}

			using (MultiThreadTaskInvoker mtti = new MultiThreadTaskInvoker())
			using (MultiThreadTaskInvoker mttiDraw = new MultiThreadTaskInvoker() { ThreadCountMax = 1 })
			{
				foreach (ConvedFile convedFile in ConvedFiles)
				{
					ConvedFile f_convedFile = convedFile;

					mtti.AddTask(() => DrawConvedFile(f_convedFile, mttiDraw));
				}
				mtti.RelayThrow();
				mttiDraw.RelayThrow();
			}
		}

		private struct PolygonInfo
		{
			public PointF[] Exterior;
			public PointF[][] Interiors;
		}

		private void DrawConvedFile(ConvedFile convedFile, MultiThreadTaskInvoker mttiDraw)
		{
			// Points
			{
				List<PointF> dest = new List<PointF>();

				foreach (GeoPoint point in convedFile.Points)
				{
					PointF pt;

					if (
						GeoPointToPointF(point, out pt) &&
						CrashUtils.IsCrashed_Rect_Point(TileImageRect, pt)
						)
					{
						dest.Add(pt);
					}
				}

				mttiDraw.AddTask(() =>
				{
					foreach (PointF pt in dest)
					{
						TileGraphics.FillEllipse(PenColorBrush, pt.X - 5, pt.Y - 5, 10, 10);
					}
				});
			}

			// Curves
			{
				List<PointF[]> dest = new List<PointF[]>();

				foreach (GeoCurve curve in convedFile.Curves)
				{
					PointF[] pts = new PointF[curve.Points.Length];
					ImageRectangle rect = ImageRectangle.INIT_VALUE;

					if (
						GeoPointsToPointFs(curve.Points, pts, ref rect) &&
						CrashUtils.IsCrashed_Rect_Rect(TileImageRect, rect)
						)
					{
						dest.Add(pts);
					}
				}

				mttiDraw.AddTask(() =>
				{
					foreach (PointF[] pts in dest)
					{
						TileGraphics.DrawLines(PenColorPen, pts);
					}
				});
			}

			// Surface
			{
				List<PointF[]> exteriorPtTbl = new List<PointF[]>();
				List<PointF[][]> interiorPtCuboid = new List<PointF[][]>();

				foreach (GeoSurface surface in convedFile.Surfaces)
				{
					PointF[] exteriorPts = new PointF[surface.Exterior.Points.Length];
					PointF[][] interiorPtTbl = new PointF[surface.Interiors.Length][];
					ImageRectangle rect = ImageRectangle.INIT_VALUE;

					if (
						GeoPointsToPointFs(surface.Exterior.Points, exteriorPts, ref rect) &&
						GeoPointTblToPointFTbl(surface.Interiors, interiorPtTbl, ref rect) &&
						CrashUtils.IsCrashed_Rect_Rect(TileImageRect, rect)
						)
					{
						exteriorPtTbl.Add(exteriorPts);
						interiorPtCuboid.Add(interiorPtTbl);
					}
				}

				mttiDraw.AddTask(() =>
				{
					for (int index = 0; index < exteriorPtTbl.Count; index++)
					{
						PointF[] exteriorPts = exteriorPtTbl[index];
						PointF[][] interiorPtTbl = interiorPtCuboid[index];

						TileGraphics.FillPolygon(BrushColorBrush, exteriorPts);
						TileGraphics.DrawPolygon(PenColorPen, exteriorPts);

						foreach (PointF[] interiorPts in interiorPtTbl)
							TileGraphics.DrawPolygon(PenColorPen, interiorPts);
					}
				});
			}
		}

		private bool GeoPointToPointF(GeoPoint gPoint, out PointF point)
		{
			double x = (gPoint.Lon - TileRect.LonMin) * TileRate_X;
			double y = (gPoint.Lat - TileRect.LatMin) * TileRate_Y;

			y = Consts.TILE_WH - y;

			point = new PointF((float)x, (float)y);

			// FIXME 有効範囲適当
			return
				-10000.0 < x && x < 10000.0 &&
				-10000.0 < y && y < 10000.0;
		}

		private bool GeoPointsToPointFs(GeoPoint[] src, PointF[] dest, ref ImageRectangle rect)
		{
			for (int index = 0; index < src.Length; index++)
			{
				if (GeoPointToPointF(src[index], out dest[index]) == false)
					return false;

				ImageRectangle.Plot(ref rect, dest[index]);
			}
			return true;
		}

		private bool GeoPointTblToPointFTbl(GeoCurve[] src, PointF[][] dest, ref ImageRectangle rect)
		{
			for (int index = 0; index < src.Length; index++)
			{
				dest[index] = new PointF[src[index].Points.Length];

				if (GeoPointsToPointFs(src[index].Points, dest[index], ref rect) == false)
					return false;
			}
			return true;
		}
	}
}
