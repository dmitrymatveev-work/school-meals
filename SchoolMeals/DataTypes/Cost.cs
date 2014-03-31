using System;

namespace SchoolMeals
{
	[Serializable]
	public class Cost
	{
		public decimal Normal { get; set; }
		public decimal WithDiscount { get; set; }
	}
}

