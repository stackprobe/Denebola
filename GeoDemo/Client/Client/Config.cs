using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public class Config
	{
		public string ServerUrlPrefix = "http://localhost:59999/";
		public bool TileCrafter_ByDisk = true;
		public bool[] VisibleLayerFlags = new bool[Consts.LAYER_NUM];
		public bool TileCrafter_PriorityToCenter = false;
		public bool TileCrafter_Road = true;
		public bool ShowCenterAddress = false;
		public Color RouteColor = Color.Aqua;
		public bool ShowUser = true;
		public Color UserColor = Color.Magenta;
		public Color HitUserColor = Color.DarkRed;
		public string GoogleMapUrlFormat = "https://www.google.co.jp/maps/dir/'$(START_LAT),$(START_LON)'/'$(END_LAT),$(END_LON)'";

		public Config()
		{
			this.SetLayerOn(Consts.LAYER_ON_PRESET_LITE);
		}

		public void SetLayerOn(string layerOn)
		{
			for (int index = 0; index < Consts.LAYER_NUM; index++)
				VisibleLayerFlags[index] = layerOn[index] == '1';
		}

		public string GetLayerOn()
		{
			StringBuilder buff = new StringBuilder();

			foreach (bool flag in Gnd.I.Config.VisibleLayerFlags)
				buff.Append(flag ? '1' : '0');

			return buff.ToString();
		}

		public void LoadFile(string file)
		{
			string VAL_TRUE = "true";

			XmlNode root = XmlNode.LoadFile(file);

			ServerUrlPrefix = root.Get("ServerUrlPrefix").Value;
			TileCrafter_ByDisk = root.Get("TileCrafter_ByDisk").Value == VAL_TRUE;
			SetLayerOn(root.Get("LayerOn").Value);
			TileCrafter_PriorityToCenter = root.Get("TileCrafter_PriorityToCenter").Value == VAL_TRUE;
			TileCrafter_Road = root.Get("TileCrafter_Road").Value == VAL_TRUE;
			ShowCenterAddress = root.Get("ShowCenterAddress").Value == VAL_TRUE;
			RouteColor = ToColor(root.Get("RouteColor"));
			ShowUser = root.Get("ShowUser").Value == VAL_TRUE;
			UserColor = ToColor(root.Get("UserColor"));
			HitUserColor = ToColor(root.Get("HitUserColor"));
			GoogleMapUrlFormat = root.Get("GoogleMapUrlFormat").Value;
		}

		private Color ToColor(XmlNode node)
		{
			return Color.FromArgb(
				255,
				int.Parse(node.Get("R").Value),
				int.Parse(node.Get("G").Value),
				int.Parse(node.Get("B").Value)
				);
		}

		public void ValidationCheck()
		{
			if (string.IsNullOrWhiteSpace(ServerUrlPrefix)) throw new InvalidDataException();
			// TileCrafter_ByDisk
			// VisibleLayerFlags
			// TileCrafter_PriorityToCenter
			// TileCrafter_Road
			// ShowCenterAddress
			// RouteColor
			// ShowUser
			// UserColor
			// HitUserColor
			if (string.IsNullOrWhiteSpace(GoogleMapUrlFormat)) throw new InvalidDataException();
		}
	}
}
