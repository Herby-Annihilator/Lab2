using Lab2.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Data
{
	public class SeveralVerticesFoundException : Exception
	{
		public List<int> Vertcies { get; set; }
		public SeveralVerticesFoundException(string message) : base(message) { }
		public EditingMode EditingMode { get; set; }
	}
}
