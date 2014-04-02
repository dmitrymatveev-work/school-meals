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
	[Activity (Label = "AddOrEditPupilActivity")]			
	public class AddOrEditPupilActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			var class1 = ClassItem.Instance;

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.AddOrEditPupil);

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

			Button oK = FindViewById<Button> (Resource.Id.oK);
			oK.Click += delegate {
				FIO old = null;
				var edit = FindViewById<CheckBox>(Resource.Id.edit);
				if(edit.Checked)
				{
					old = new FIO();
					var pupil = (string)menu.SelectedItem;
					var pupilSplittedName = pupil.Split('|');
					old.LastName = pupilSplittedName[0].Trim();
					old.FirstName = pupilSplittedName[1].Trim();
					if(pupilSplittedName.Length == 3)
						old.IsDiscount = pupilSplittedName[2].Trim() == "л";
				}
				ClassItem.Instance.AddOrEditPupil(old, new FIO {
					FirstName = FindViewById<EditText>(Resource.Id.firstName).Text,
					LastName = FindViewById<EditText>(Resource.Id.lastName).Text,
					IsDiscount = FindViewById<CheckBox>(Resource.Id.isDiscount).Checked
				});

				Finish();
			};

			Button cancel = FindViewById<Button> (Resource.Id.cancel);
			cancel.Click += delegate {
				Finish();
			};

			var lastName = FindViewById<EditText>(Resource.Id.lastName);
			lastName.FocusChange += (object sender, View.FocusChangeEventArgs e) => {
				var currentSender = sender as EditText;
				if(e.HasFocus && currentSender != null)
					currentSender.Text = string.Empty;
			};

			var firstName = FindViewById<EditText>(Resource.Id.firstName);
			firstName.FocusChange += (object sender, View.FocusChangeEventArgs e) => {
				var currentSender = sender as EditText;
				if(e.HasFocus && currentSender != null)
					currentSender.Text = string.Empty;
			};
		}

		private void menu_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var menu = (Spinner)sender;

			var pupil = (string)menu.GetItemAtPosition(e.Position);
			var pupilSplittedName = pupil.Split('|');

			var lastName = FindViewById<EditText>(Resource.Id.lastName);
			lastName.Text = pupilSplittedName[0].Trim();
			var firstName = FindViewById<EditText>(Resource.Id.firstName);
			firstName.Text = pupilSplittedName[1].Trim();
			if(pupilSplittedName.Length == 3)
			{
				var discount = FindViewById<CheckBox>(Resource.Id.isDiscount);
				discount.Checked = pupilSplittedName[2].Trim() == "л";
			}
		}
	}
}

