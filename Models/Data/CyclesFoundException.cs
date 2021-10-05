using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Data
{
	public class CyclesFoundException : Exception
	{
		public List<Work> CyclingWorks { get; set; }
		public List<int> CyclingVertecies { get; set; }

		public CyclesFoundException(string message, List<Work> works, List<int> vertecies) 
			: base(message)
		{
			CyclingVertecies = vertecies;
			CyclingWorks = works;
		}

		public CyclesFoundException(List<Work> works, List<int> vertecies)
		{
			CyclingVertecies = vertecies;
			CyclingWorks = works;
		}

		private List<string> _cycles = new List<string>();

		public override string ToString()
		{
			_cycles.Clear();
			CyclingWorks.Sort((first, second) =>
			{
				if (first.FirstEventID > second.FirstEventID) return 1;
				if (first.FirstEventID < second.FirstEventID) return -1;
				if (first.FirstEventID == second.FirstEventID)
				{
					if (first.SecondEventID > second.SecondEventID) return 1;
					if (first.SecondEventID < second.SecondEventID) return -1;
				}
				return 0;
			}
			);
			int startVertex;
			int endVertex;
			for (int i = 0; i < CyclingWorks.Count; i = GetNextWorkIndex(i))
			{
				startVertex = CyclingWorks[i].FirstEventID;
				for (int j = 0; j < CyclingWorks.Count; j++)
				{
					if (CyclingWorks[j].SecondEventID == startVertex)
					{
						endVertex = CyclingWorks[j].FirstEventID;
						FindCycles(CyclingWorks, i, startVertex, endVertex, new List<int>());
					}
				}
			}
			string result = "Найдены циклы:\r\n";
			foreach (var item in _cycles)
			{
				result += item + "\r\n";
			}
			return result;
		}

		private int GetNextWorkIndex(int currentIndex)
		{
			int result = currentIndex;
			for ( ; result < CyclingWorks.Count; result++)
			{
				if (CyclingWorks[currentIndex].FirstEventID != CyclingWorks[result].FirstEventID)
					return result;
			}
			return result;
		}


		private void FindCycles(List<Work> table, int blockStartIndex, int currentVertex, int endVertex, List<int> currentPath)
		{
			currentPath.Add(currentVertex);
			if (currentVertex == endVertex)
			{
				string path = "";
				foreach (int vertex in currentPath)
				{
					path += vertex.ToString() + " ";
				}
				path += currentPath[0].ToString();
				_cycles.Add(path);
				currentPath.Remove(currentVertex);
				return;
			}
			int blockEndIndex = blockStartIndex + 1;
			for (; blockEndIndex < table.Count; blockEndIndex++)   // найти конец блока работ
			{
				if (table[blockStartIndex].FirstEventID != table[blockEndIndex].FirstEventID)
				{
					break;
				}
			}
			int nextBlockIndex;
			for (int i = blockStartIndex; i < blockEndIndex && i < table.Count; i++)  // обходим блок
			{
				for (nextBlockIndex = i; nextBlockIndex < table.Count; nextBlockIndex++)
				{
					if (table[nextBlockIndex].FirstEventID == table[i].SecondEventID)
						break;
				}
				FindCycles(table, nextBlockIndex, table[i].SecondEventID, endVertex, currentPath);
			}
			currentPath.Remove(currentVertex);
			return;
		}
	}
}
