using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Data
{
	public class NoVerticesFoundException : Exception
	{
		public NoVerticesFoundException(string message) : base(message) { }
	}
}
