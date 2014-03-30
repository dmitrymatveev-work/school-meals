using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SchoolMeals
{
	public class OnDateChangedListener : Java.Lang.Object, DatePicker.IOnDateChangedListener
	{
		public event EventHandler<DateChangedEventArgs> DateChanged;

		public void OnDateChanged(DatePicker view, int Year, int Month, int dayOfMonth)
		{
			EventHandler<DateChangedEventArgs> temp = DateChanged;
			if(temp != null)
				temp(view, new DateChangedEventArgs(view.DateTime));
		}
	}
}

