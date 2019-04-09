using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;

namespace Charlotte
{
	public class Road
	{
		public class Node
		{
			public string Code;
			public MapPoint Point;
			public List<string> Wk_LinkCodes = new List<string>();
			public Node[] Links;

			public static int Comp(Node a, Node b)
			{
				return StringTools.Comp(a.Code, b.Code);
			}

			public Node Rt_PrevNode;
			public double Rt_Distance;
		}

		public Dictionary<string, Node> Wk_Nodes = DictionaryTools.Create<Node>();
		public Node[] Nodes;

		public class Branch
		{
			public Node A;
			public Node B;
		}

		public List<Branch> Wk_Branches = new List<Branch>();
		public Branch[] Branches;
		public MeshedGroupManager<Branch> BranchMgm;
	}
}
