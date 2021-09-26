using Lab2.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Services
{
	/// <summary>
	/// Свалка данных для обмена между моделями представления
	/// </summary>
	public class Exchanger
	{
		public List<int> Vertices { get; set; }

		
		public AdjacencyList AdjacencyList { get; set; }
	}
}
