using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte.Layer
{
	public interface ILayer
	{
		void DrawTile(Graphics g, GeoRectangle gRect); // 注意) BackgroundTh から呼ばれる。
	}
}
