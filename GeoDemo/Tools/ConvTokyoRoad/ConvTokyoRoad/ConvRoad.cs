using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class ConvRoad
	{
		public string R_Dir;
		public string W_Dir;

		// <---- prm

		public void Invoke()
		{
			foreach (string file in Directory.GetFiles(R_Dir, "*.mrd"))
			{
				string wFile = Path.Combine(W_Dir, Path.GetFileName(file) + ".conved.txt");

				Console.WriteLine("< " + file);
				Console.WriteLine("> " + wFile);

				Invoke_File(file, wFile);
			}
		}

		private byte[] Record = new byte[256];
		private Dai2JiMesh Mesh;

		private void Invoke_File(string rFile, string wFile)
		{
			Mesh = null;

			using (FileStream reader = new FileStream(rFile, FileMode.Open, FileAccess.Read))
			using (StreamWriter writer = new StreamWriter(wFile, false, Encoding.ASCII))
			{
				for (; ; )
				{
					int readSize = reader.Read(Record, 0, 256);

					if (readSize == 0)
						break;

					if (readSize != 256)
						throw new Exception("256バイトのレコードを読み込めませんでした。" + readSize);

					int recordId = GetInt(0, 2);

					if (recordId == 11)
					{
						Mesh = new Dai2JiMesh(GetString(2, 6));
					}
					else if (recordId == 31)
					{
						int nodeNo = GetInt(2, 5);
						int sX = GetInt(7, 5);
						int sY = GetInt(12, 5);
						int nodeKindCode = GetInt(17, 1);
						int rMeshCode = GetInt(18, 6);
						int rNodeNo = GetInt(24, 5);
						int linkNum = GetInt(29, 1);
						int[] lNodeNos = new int[linkNum];

						for (int index = 0; index < linkNum; index++)
							lNodeNos[index] = GetInt(30 + index * 5, 5);

						double lat = Mesh.GetLat(sY / 10000.0);
						double lon = Mesh.GetLon(sX / 10000.0);

						// ロードここまで

						writer.WriteLine("N");
						writer.WriteLine(GetNodeCode(Mesh, nodeNo));
						writer.WriteLine(lat.ToString("F9") + " " + lon.ToString("F9"));

						if (rMeshCode != 0)
							writer.WriteLine(GetNodeCode(rMeshCode, rNodeNo));

						foreach (int lNodeNo in lNodeNos)
							writer.WriteLine(GetNodeCode(Mesh, lNodeNo));

						writer.WriteLine("/");
					}
				}
			}
		}

		private string GetNodeCode(Dai2JiMesh mesh, int nodeNo)
		{
			return GetNodeCode(int.Parse(mesh.Code), nodeNo);
		}

		private string GetNodeCode(int meshCode, int nodeNo)
		{
			if (meshCode < 0 || 999999 < meshCode) throw null; // 2bs
			if (nodeNo < 0 || 99999 < nodeNo) throw null; // 2bs

			return meshCode.ToString("D6") + ":" + nodeNo.ToString("D5");
		}

		private int GetInt(int offset, int size)
		{
			return int.Parse(GetString(offset, size));
		}

		private string GetString(int offset, int size)
		{
			return Encoding.ASCII.GetString(GetBytes(offset, size));
		}

		private byte[] GetBytes(int offset, int size)
		{
			return BinTools.GetSubBytes(Record, offset, size);
		}
	}
}
