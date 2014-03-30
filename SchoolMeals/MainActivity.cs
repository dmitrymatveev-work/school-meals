using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SchoolMeals
{
	[Activity (Label = "SchoolMeals", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button addOrEditPupilButton = FindViewById<Button> (Resource.Id.addOrEditPupilButton);
			Button removePupilButton = FindViewById<Button> (Resource.Id.removePupilButton);
			Button addOrEditCostButton = FindViewById<Button> (Resource.Id.addOrEditCostButton);
			Button payCostButton = FindViewById<Button> (Resource.Id.payCostButton);
			Button setPresenceAndDiscountButton = FindViewById<Button> (Resource.Id.setPresenceAndDiscountButton);
			
			addOrEditPupilButton.Click += delegate {
				StartActivity(typeof(AddOrEditPupilActivity));
			};
			removePupilButton.Click += delegate {
				StartActivity(typeof(RemovePupil));
			};
			addOrEditCostButton.Click += delegate {
				StartActivity(typeof(AddOrEditCostActivity));
			};
			payCostButton.Click += delegate {
			};
			setPresenceAndDiscountButton.Click += delegate {
			};
		}
	}
}


