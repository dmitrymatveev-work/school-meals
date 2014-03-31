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
			Button exitButton = FindViewById<Button> (Resource.Id.exit);

			addOrEditPupilButton.Click += delegate {
				StartActivity(typeof(AddOrEditPupilActivity));
			};
			removePupilButton.Click += delegate {
				StartActivity(typeof(RemovePupilActivity));
			};
			addOrEditCostButton.Click += delegate {
				StartActivity(typeof(AddOrEditCostActivity));
			};
			payCostButton.Click += delegate {
				StartActivity(typeof(PayActivity));
			};
			setPresenceAndDiscountButton.Click += delegate {
				StartActivity(typeof(PresenceActivity));
			};

			exitButton.Click += delegate {
				Finish();
			};
		}

		protected override void OnDestroy ()
		{
			var class1 = ClassItem.Instance;
			class1.Serialize();

			base.OnDestroy ();
		}
	}
}


