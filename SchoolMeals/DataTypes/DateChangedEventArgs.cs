using System;

namespace SchoolMeals
{
	public class DateChangedEventArgs : EventArgs
	{
		private readonly DateTime _dateTime;

		public DateChangedEventArgs (DateTime dateTime)
		{
			this._dateTime = dateTime;
		}

		public DateTime DateTime { get { return this._dateTime;} }
	}
}
