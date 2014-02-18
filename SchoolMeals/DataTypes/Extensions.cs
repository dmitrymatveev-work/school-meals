using System;

namespace SchoolMeals
{
	public static class Extensions
	{
		public static void Merge(this FIO fioOld, FIO fioNew)
		{
			fioOld.FirstName = fioNew.FirstName;
			fioOld.LastName = fioNew.LastName;
			fioOld.IsDiscount = fioNew.IsDiscount;
		}
	}
}

