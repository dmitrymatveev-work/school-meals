using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SchoolMeals
{
	[Activity (Label = "ReportActivity")]			
	public class ReportActivity : Activity
	{
		private DateTime _startDate;
		private DateTime _endDate;
		private Button _pressedButton;

		protected override void OnCreate (Bundle bundle)
		{
			var class1 = ClassItem.Instance;

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Report);

			var selectStartDate = FindViewById<Button>(Resource.Id.selectStartDate);
			var selectEndDate = FindViewById<Button>(Resource.Id.selectEndDate);
			var formReport = FindViewById<Button>(Resource.Id.formReport);

			this._startDate = DateTime.Today;
			this._endDate = DateTime.Today;

			selectStartDate.Click += delegate {
				this._pressedButton = selectStartDate;
				ShowDialog(0);
			};
			selectEndDate.Click += delegate {
				this._pressedButton = selectEndDate;
				ShowDialog(1);
			};

			formReport.Click += delegate {
				if(this._startDate != null && this._endDate != null)
					class1.FormReport(this._startDate, this._endDate);

				Finish();
			};
		}

		protected override Dialog OnCreateDialog (int id)
		{
			int year;
			int month;
			int day;

			if(id == 0)
			{
				year = this._startDate.Year;
				month = this._startDate.Month - 1;
				day = this._startDate.Day;
			}
			else
			{
				year = this._endDate.Year;
				month = this._endDate.Month - 1;
				day = this._endDate.Day;
			}
			return new DatePickerDialog (this, HandleDateSet, year,
				month, day);
		}

		private void HandleDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			this._pressedButton.Text = e.Date.ToString();

			if(this._pressedButton == FindViewById<Button>(Resource.Id.selectStartDate))
				this._startDate = e.Date;
			else
				this._endDate = e.Date;
		}
	}
}

