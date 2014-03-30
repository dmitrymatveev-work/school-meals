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
	[Activity (Label = "AddOrEditCostActivity")]			
	public class AddOrEditCostActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			var class1 = ClassItem.Instance;

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.AddOrEditCost);

			var normalCost = FindViewById<EditText>(Resource.Id.normalCost);
			var withDiscount = FindViewById<EditText>(Resource.Id.withDiscount);

			normalCost.FocusChange += (object sender, View.FocusChangeEventArgs e) => {
				if(e.HasFocus)
					normalCost.Text = string.Empty;
			};

			withDiscount.FocusChange += (object sender, View.FocusChangeEventArgs e) => {
				if(e.HasFocus)
					withDiscount.Text = string.Empty;
			};

			var onDateChangedListener = new OnDateChangedListener();
			onDateChangedListener.DateChanged += (object sender, DateChangedEventArgs e) => {
				var cost = class1.GetCost(e.DateTime).Value;
				normalCost.Text = cost.Normal.ToString();
				withDiscount.Text = cost.WithDiscount.ToString();
			};
			var datePicker = FindViewById<DatePicker>(Resource.Id.datePicker1);
			datePicker.Init(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, onDateChangedListener);

			Button oK = FindViewById<Button> (Resource.Id.oK);
			oK.Click += delegate {
				decimal tempNormalCost;
				decimal tempWithDiscount;
				if(decimal.TryParse(normalCost.Text, out tempNormalCost) && decimal.TryParse(withDiscount.Text, out tempWithDiscount))
					class1.AddOrEditCost(datePicker.DateTime,
						new Cost{ Normal = tempNormalCost, WithDiscount = tempWithDiscount });
			};

			Button cancel = FindViewById<Button> (Resource.Id.cancel);
			cancel.Click += delegate {
				Finish();
			};
		}
	}
}

