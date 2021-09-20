using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Data
{
	public class CyclesFoundException : Exception
	{
		public List<Work> CyclingWorks { get; set; }
		public List<int> CyclingVertecies { get; set; }

		public CyclesFoundException(string message, List<Work> works, List<int> vertecies) : base(message)
		{
			CyclingVertecies = vertecies;
			CyclingWorks = works;
		}

		public CyclesFoundException(List<Work> works, List<int> vertecies)
		{
			CyclingVertecies = vertecies;
			CyclingWorks = works;
		}

		public override string ToString()
		{
			List<int> cycle = FindCycle();
			string cycl = "";
			for (int i = 0; i < cycle.Count; i++)
			{
				cycl += $"{cycle[i]} ";
			}
			string result = "Найден цикл: " + cycl;
			return result;
		}

		private List<int> FindCycle()
		{
			List<int> result = new List<int>();
			int startVertex = CyclingWorks[0].FirstEventID;
			result.Add(startVertex);
			int currentVertex = CyclingWorks[0].SecondEventID;
			result.Add(currentVertex);
			while (startVertex != currentVertex)
			{
				for (int i = 1; i < CyclingWorks.Count; i++)
				{
					if (CyclingWorks[i].FirstEventID == currentVertex)
					{
						currentVertex = CyclingWorks[i].SecondEventID;
						break;
					}						
				}
				result.Add(currentVertex);
			}
			return result;
		}
	}
}
