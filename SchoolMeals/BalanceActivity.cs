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
	[Activity (Label = "BalanceActivity")]			
	public class BalanceActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			var class1 = ClassItem.Instance;

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Balance);

			Button exit = FindViewById<Button> (Resource.Id.exit);
			exit.Click += delegate {
				Finish();
			};

			this.UpdateList();
		}

		private void UpdateList()
		{
			var class1 = ClassItem.Instance;

			var linearLayout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

			linearLayout1.RemoveAllViews();

			var balances = class1.Calculate();

			foreach(var balance in balances)
			{
				var linearLayout = new LinearLayout(this);
				linearLayout.Orientation = Orientation.Horizontal;
				linearLayout1.AddView(linearLayout);

				var name = new TextView(this);
				name.SetWidth(150);
				name.Text = balance.Key.LastName + ((balance.Key.FirstName.Length > 0) ? balance.Key.FirstName.Substring(0,1) : string.Empty);

				var balanceText = new TextView(this);
				balanceText.SetWidth(150);
				balanceText.Text = balance.Value.ToString();

				var currencySign = new TextView(this);
				currencySign.SetWidth(50);
				currencySign.Text = "руб.";

				linearLayout.AddView(name);
				linearLayout.AddView(balanceText);
				linearLayout.AddView(currencySign);
			}
		}
	}
}

