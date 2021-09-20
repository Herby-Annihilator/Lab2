using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Data
{
	public class Work
	{
		public int FirstEventID { get; set; }
		public int SecondEventID { get; set; }
		public int Length { get; set; }

		public Work(int firstEventID, int secondEventID, int length)
		{
			FirstEventID = firstEventID;
			SecondEventID = secondEventID;
			Length = length;
		}

		public Work Clone() => new Work(FirstEventID, SecondEventID, Length);

		public override string ToString() => $"{FirstEventID} → {SecondEventID} = {Length}";
	}
}
