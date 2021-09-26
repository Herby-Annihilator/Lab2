using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Data
{
	public class AdjacencyList
	{
		public List<int> Vertices { get; set; }
		public Dictionary<int, List<Work>> Edges { get; set; }
	}
}
