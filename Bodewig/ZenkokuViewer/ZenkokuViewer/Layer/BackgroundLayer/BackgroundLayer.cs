using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte.Layer.BackgroundLayer
{
	public class BackgroundLayer : ILayer
	{
		private Color BgColor;

		public BackgroundLayer(Color bgColor)
		{
			this.BgColor = bgColor;
		}

		public void DrawTile(Graphics g, GeoRectangle gRect)
		{
			g.FillRectangle(new SolidBrush(this.BgColor), 0, 0, Consts.TILE_WH, Consts.TILE_WH);
		}
	}
}
