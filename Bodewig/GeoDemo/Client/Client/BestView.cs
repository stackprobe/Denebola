using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.Windows.Forms;

namespace Charlotte
{
	public class BestView
	{
		private double XMin = IntTools.IMAX;
		private double YMin = IntTools.IMAX;
		private double XMax = -IntTools.IMAX;
		private double YMax = -IntTools.IMAX;

		public void AddPoint(MapPoint point)
		{
			XMin = Math.Min(XMin, point.X);
			YMin = Math.Min(YMin, point.Y);
			XMax = Math.Max(XMax, point.X);
			YMax = Math.Max(YMax, point.Y);
		}

		private int MapPanel_W = -1;
		private int MapPanel_H = -1;

		public void SetMapPanel(Panel mapPanel)
		{
			MapPanel_W = mapPanel.Width;
			MapPanel_H = mapPanel.Height;
		}

		public double CenterX;
		public double CenterY;
		public int MeterPerMDot;

		public void Invoke()
		{
			double WH_MIN = 1.0;

			if (XMax - WH_MIN < XMin) throw new Exception("X_狭すぎ");
			if (YMax - WH_MIN < YMin) throw new Exception("Y_狭すぎ");

			int MP_WH_MIN = 100;

			if (MapPanel_W < MP_WH_MIN) throw new Exception("MP_X_狭すぎ");
			if (MapPanel_H < MP_WH_MIN) throw new Exception("MP_Y_狭すぎ");

			CenterX = (XMin + XMax) / 2.0;
			CenterY = (YMin + YMax) / 2.0;

			double w = XMax - XMin;
			double h = YMax - YMin;

			w *= 1.1; // += margin
			h *= 1.1; // += margin

			double meterPerDotX = w / MapPanel_W;
			double meterPerDotY = h / MapPanel_H;

			double meterPDot = Math.Max(meterPerDotX, meterPerDotY);

			MeterPerMDot = DoubleTools.ToInt(meterPDot * 1000000.0);

			MeterPerMDot = IntTools.ToRange(MeterPerMDot, Consts.MPMD_MIN, Consts.MPMD_MAX);
		}
	}
}
