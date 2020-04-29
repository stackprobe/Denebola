using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class UserInfoManager
	{
		public List<UserInfo> Users = new List<UserInfo>();

		public void AddUser(UserInfo user)
		{
			if (3000 < Users.Count)
				throw new Exception("顧客過多エラー");

			Users.Add(user);
		}

		public void LoadFile()
		{
			string file = "Users.xml";

			if (File.Exists(file) == false)
				file = @"..\..\..\..\res\Client\Users.xml";

			XmlNode root = XmlNode.LoadFile(file);

			foreach (XmlNode node in root.Children)
			{
				UserInfo user = new UserInfo();

				user.Name = node.Get("Name").Value;

				{
					double x = double.Parse(node.Get("Point/X").Value);
					double y = double.Parse(node.Get("Point/Y").Value);

					user.Point = new MapPoint(x, y);
				}

				user.Mail = node.Get("Mail").Value;

				// Validate
				{
					user.Name = JString.AsLine(user.Name);
					user.Point.X = DoubleTools.ToRange(user.Point.X, Consts.X_MIN, Consts.X_MAX);
					user.Point.Y = DoubleTools.ToRange(user.Point.Y, Consts.Y_MIN, Consts.Y_MAX);
					user.Mail = JString.AsLine(user.Mail);
				}

				AddUser(user);
			}
		}
	}
}
