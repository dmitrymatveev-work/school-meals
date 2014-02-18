using System;

namespace SchoolMeals
{
	public class FIO
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsDiscount { get; set; }


		#region [ Equals ]
		public override bool Equals (object obj)
		{
			var fio = obj as FIO;
			if (fio == null)
				return false;

			return this.FirstName.Equals(fio.FirstName) && this.LastName.Equals(fio.LastName);
		}

		public bool Equals(FIO fio)
		{
			if (fio == null)
				return false;

			return this.FirstName.Equals(fio.FirstName) && this.LastName.Equals(fio.LastName);
		}
		#endregion

		#region [ GetHashCode ]
		public override int GetHashCode ()
		{
			return this.FirstName.GetHashCode() ^ this.LastName.GetHashCode();
		}
		#endregion

		#region [ Operator equal ]
		public static bool operator == (FIO fio1, FIO fio2)
		{
			if (object.ReferenceEquals (fio1, fio2))
				return true;

			if (((object)fio1 == null) || ((object)fio2 == null))
				return false;

			return fio1.Equals(fio2);
		}

		public static bool operator != (FIO fio1, FIO fio2)
		{
			return !(fio1 == fio2);
		}
		#endregion
	}
}

