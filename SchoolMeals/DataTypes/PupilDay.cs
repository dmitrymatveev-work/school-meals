using System;

namespace SchoolMeals
{
	[Serializable]
	public class PupilDay
	{
		public bool Presence { get; set; }
		public bool IsDiscount { get; set; }
		public decimal Payment { get; set; }
		public string Comment { get; set; }
	}
}

