using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte
{
	public class Tile
	{
		public ActiveTileTable Owner;
		public long X;
		public long Y;
		public Image Bmp;

		public double LatMin { get { return (Y + 0) * Consts.TILE_WH * (Owner.MeterPerMDot / 1000000.0) / Owner.MeterPerLat; } }
		public double LatMax { get { return (Y + 1) * Consts.TILE_WH * (Owner.MeterPerMDot / 1000000.0) / Owner.MeterPerLat; } }
		public double LonMin { get { return (X + 0) * Consts.TILE_WH * (Owner.MeterPerMDot / 1000000.0) / Owner.MeterPerLon; } }
		public double LonMax { get { return (X + 1) * Consts.TILE_WH * (Owner.MeterPerMDot / 1000000.0) / Owner.MeterPerLon; } }

		public void Added()
		{
			Bmp = new Bitmap(Consts.TILE_WH, Consts.TILE_WH);

			using (Graphics g = Graphics.FromImage(Bmp))
			{
				g.FillRectangle(Brushes.Moccasin, 0, 0, Consts.TILE_WH, Consts.TILE_WH);
				g.DrawLine(new Pen(Color.Orange), 0, 0, 0, Consts.TILE_WH - 1);
				g.DrawLine(new Pen(Color.Orange), 0, 0, Consts.TILE_WH - 1, 0);
				g.DrawLine(new Pen(Color.Orange), 0, Consts.TILE_WH - 1, Consts.TILE_WH - 1, Consts.TILE_WH - 1);
				g.DrawLine(new Pen(Color.Orange), Consts.TILE_WH - 1, 0, Consts.TILE_WH - 1, Consts.TILE_WH - 1);

				List<string> dest = new List<string>();

				dest.Add("" + X);
				dest.Add("" + Y);
				dest.Add("(" + LatMin.ToString("F9") + ", " + LonMin.ToString("F9") + ")");
				dest.Add("(" + LatMax.ToString("F9") + ", " + LonMax.ToString("F9") + ")");

				g.DrawString(string.Join("\n", dest), new Font("メイリオ", 10F, FontStyle.Regular), Brushes.OrangeRed, 0, 0);
			}

			Gnd.I.SubTiles.Add(new SubTile()
			{
				Owner = this,
			});
		}

		public void Deleted()
		{
			Gnd.I.SubTiles.RemoveAll(tile => tile.Owner == this);
		}

		public GeoRectangle Rectangle
		{
			get
			{
				return new GeoRectangle(LatMin, LatMax, LonMin, LonMax);
			}
		}
	}
}
