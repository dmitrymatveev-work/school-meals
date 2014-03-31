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
	[Activity (Label = "PayActivity")]			
	public class PayActivity : Activity
	{
		private FIO _currentPupil = default(FIO);
		private DateTime _dateTime = default(DateTime);

		protected override void OnCreate (Bundle bundle)
		{
			var class1 = ClassItem.Instance;

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Pay);

			var menu = FindViewById<Spinner>(Resource.Id.menu);

			menu.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(menu_ItemSelected);
			var pupilList = class1.GetPupilList(DateTime.Today);
			var pupilArray = new List<string>();
			foreach(var item in pupilList)
			{
				pupilArray.Add(string.Format("{0} | {1} | {2}", item.Key.LastName, item.Key.FirstName, item.Key.IsDiscount ? "л" : ""));
			}
			var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, pupilArray.ToArray());
			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			menu.Adapter = adapter;

			var amount = FindViewById<EditText>(Resource.Id.amount);

			var onDateChangedListener = new OnDateChangedListener();
			onDateChangedListener.DateChanged += (object sender, DateChangedEventArgs e) => {
				_dateTime = e.DateTime;

				if(this._currentPupil != null && this._dateTime != null)
					amount.Text = class1.GetAmount(this._dateTime, this._currentPupil).ToString();
			};
			var datePicker = FindViewById<DatePicker>(Resource.Id.datePicker1);
			datePicker.Init(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, onDateChangedListener);

			Button oK = FindViewById<Button> (Resource.Id.oK);
			oK.Click += delegate {
				decimal tempAmount;

				if(this._currentPupil != null && this._dateTime != null && decimal.TryParse(amount.Text, out tempAmount))
					class1.Pay(this._dateTime, this._currentPupil, tempAmount);
			};

			Button cancel = FindViewById<Button> (Resource.Id.cancel);
			cancel.Click += delegate {
				Finish();
			};
		}

		private void menu_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var class1 = ClassItem.Instance;

			var menu = (Spinner)sender;

			var pupil = (string)menu.GetItemAtPosition(e.Position);
			var pupilSplittedName = pupil.Split('|');

			this._currentPupil = new FIO{
				FirstName = pupilSplittedName[1].Trim(),
				LastName = pupilSplittedName[0].Trim()
			};

			var amount = FindViewById<EditText>(Resource.Id.amount);
			if(this._currentPupil != null && this._dateTime != null)
				amount.Text = class1.GetAmount(this._dateTime, this._currentPupil).ToString();
		}
	}
}

