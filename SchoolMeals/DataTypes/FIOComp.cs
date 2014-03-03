using System;
using System.Collections.Generic;

namespace SchoolMeals
{
	public class FIOComp : IComparer<FIO>
	{
		public int Compare(FIO fio1, FIO fio2)
		{
			return (fio1.LastName + " " + fio1.FirstName).CompareTo (fio2.LastName + " " + fio2.FirstName);
		}
	}
}

