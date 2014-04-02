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
	[Activity (Label = "PresenceActivity")]			
	public class PresenceActivity : Activity
	{
		private DateTime _date;
		private SortedList<FIO, PresenceList> _presenceList = new SortedList<FIO, PresenceList>();

		protected override void OnCreate (Bundle bundle)
		{
			var class1 = ClassItem.Instance;

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Presence);

			var selectDate = FindViewById<Button>(Resource.Id.selectDate);

			_date = DateTime.Today;
			selectDate.Text = _date.ToString();

			selectDate.Click += delegate {
				ShowDialog(0);
			};

			Button oK = FindViewById<Button> (Resource.Id.oK);
			oK.Click += delegate {
				if(!this._date.Equals(default(DateTime)))
				{
					var inList = new SortedList<FIO, PupilDay>();

					foreach(var presenceItem in this._presenceList)
					{
						inList.Add(presenceItem.Key, new PupilDay{
							Presence = presenceItem.Value.presence.Checked,
							IsDiscount = presenceItem.Value.discount.Checked,
							Comment = presenceItem.Value.comment.Text
						});
					}

					class1.SetPresenceAndDiscount(this._date, inList);
				}

				Finish();
			};

			Button cancel = FindViewById<Button> (Resource.Id.cancel);
			cancel.Click += delegate {
				Finish();
			};

			this.UpdateList();
		}

		protected override Dialog OnCreateDialog (int id)
		{
			return new DatePickerDialog (this, HandleDateSet, _date.Year,
				_date.Month - 1, _date.Day);
		}

		private void HandleDateSet (object sender, DatePickerDialog.DateSetEventArgs e)
		{
			var selectDate = FindViewById<Button>(Resource.Id.selectDate);
			selectDate.Text = e.Date.ToString();
			this._date = e.Date;

			this.UpdateList();
		}

		private void UpdateList()
		{
			var class1 = ClassItem.Instance;

			var linearLayout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);

			_presenceList.Clear();
			linearLayout1.RemoveAllViews();

			var selectDate = FindViewById<Button>(Resource.Id.selectDate);

			var pupilList = class1.GetPupilList(DateTime.Parse(selectDate.Text));

			foreach(var pupilItem in pupilList)
			{
				var linearLayout = new LinearLayout(this);
				linearLayout.Orientation = Orientation.Horizontal;
				linearLayout1.AddView(linearLayout);

				var name = new TextView(this);
				name.SetWidth(150);
				name.Text = pupilItem.Key.LastName + ((pupilItem.Key.FirstName.Length > 0) ? pupilItem.Key.FirstName.Substring(0,1) : string.Empty);

				var presence = new CheckBox(this);
				presence.Checked = pupilItem.Value.Presence;

				var discount = new CheckBox(this);
				discount.Checked = pupilItem.Value.IsDiscount;

				var comment = new EditText(this);
				comment.SetWidth(300);
				comment.Text = pupilItem.Value.Comment;

				this._presenceList.Add(pupilItem.Key, new PresenceList{
					presence = presence,
					discount = discount,
					comment = comment
				});

				linearLayout.AddView(name);
				linearLayout.AddView(presence);
				linearLayout.AddView(comment);
			}
		}
	}
}

