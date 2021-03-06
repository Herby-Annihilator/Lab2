using Lab2.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Data
{
	public class NoVerticesFoundException : Exception
	{
		public List<int> Vertcies { get; } = new List<int>();
		public NoVerticesFoundException(string message) : base(message) { }
		public EditingMode EditingMode { get; set; }
	}
}
