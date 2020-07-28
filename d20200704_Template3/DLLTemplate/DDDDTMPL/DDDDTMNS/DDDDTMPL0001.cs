using Charlotte.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charlotte.DDDDTMNS
{
	public class DDDDTMPL0001 // -- 0001
	{
		public string Echo(string message)
		{
			using (WorkingDir wd = new WorkingDir())
			{
				string file = wd.MakePath();

				File.WriteAllText(file, message, Encoding.UTF8);

				message = File.ReadAllText(file, Encoding.UTF8);
			}
			return message;
		}
	}
}
