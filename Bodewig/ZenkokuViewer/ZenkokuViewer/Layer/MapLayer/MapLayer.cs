using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Tools;
using Charlotte.Utils;

namespace Charlotte.Layer.MapLayer
{
	public class MapLayer : ILayer
	{
		private string Dir;
		public MapLayer(string dir)
		{
			this.Dir = dir;
		}

		private Color PenColor = Color.Blue;
		private Color BrushColor = Color.Cyan;

		public void SetColor_Index(int index)
		{
			int r = index / 9;
			int g = (index / 3) % 3;
			int b = index % 3;

			{
				int[] lv = new int[] { 0, 100, 200 };

				PenColor = Color.FromArgb(lv[r], lv[g], lv[b]);
			}

			{
				int[] lv = new int[] { 128, 192, 255 };

				BrushColor = Color.FromArgb(lv[r], lv[g], lv[b]);
			}
		}

		private Area AreaCache = null;
		private StringKeyCache<ConvedFile> ConvedFileCache = new StringKeyCache<ConvedFile>(true, file => new ConvedFile(file));

		public void DrawTile(Graphics g, GeoRectangle tileRect)
		{
			try
			{
				if (this.AreaCache == null)
					this.AreaCache = new Area(this.Dir);

				List<string> files = new List<string>();

				this.AreaCache.Search(tileRect, file =>
				{
					if (100 < files.Count)
						throw new DrawTile_Overflow();

					files.Add(file);
				});

				// キャッシュクリア FIXME ???
				if (1000.0 < SyncGnd.I.UsedMemoryMB.Get()) // ? 1 GB <
				{
					this.AreaCache = null;
					this.ConvedFileCache.Clear();
				}

				int count = files.Count;
				ConvedFile[] convedFiles = new ConvedFile[count];

				for (int index = 0; index < count; index++)
					convedFiles[index] = this.ConvedFileCache.Get(files[index]);

				new TileDrawer()
				{
					TileGraphics = g,
					TileRect = tileRect,
					ConvedFiles = convedFiles,

					PenColorPen = new Pen(PenColor),
					PenColorBrush = new SolidBrush(PenColor),
					BrushColorBrush = new SolidBrush(BrushColor),
				}
				.Invoke();
			}
			catch (DrawTile_Overflow)
			{ }
		}

		private class DrawTile_Overflow : Exception
		{ }
	}
}
