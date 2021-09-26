using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Data
{
	public class AdjacencyList
	{
		public List<int> Vertices { get; set; } = new List<int>();
		public Dictionary<int, List<Work>> Edges { get; set; } = new Dictionary<int, List<Work>>();
	}
}
