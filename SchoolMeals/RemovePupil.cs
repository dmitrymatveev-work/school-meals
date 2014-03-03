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
	[Activity (Label = "RemovePupil")]			
	public class RemovePupil : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			var class1 = ClassItem.Instance;

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.RemovePupil);

			var menu = FindViewById<Spinner>(Resource.Id.menu);

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
				var pupil = (string)menu.SelectedItem;
				var pupilSplittedName = pupil.Split('|');
				ClassItem.Instance.RemovePupil(new FIO{
					LastName = pupilSplittedName[0].Trim(),
					FirstName = pupilSplittedName[1].Trim()
				});

				Finish();
			};

			Button cancel = FindViewById<Button> (Resource.Id.cancel);
			cancel.Click += delegate {
				Finish();
			};
		}
	}
}

