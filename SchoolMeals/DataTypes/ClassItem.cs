using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace SchoolMeals
{
	public class ClassItem
	{
		private static ClassItem _thisObject;

		private SortedList<FIO, SortedList<DateTime, PupilDay>> _pupils;
		private SortedList<DateTime, Cost> _days;

		private ClassItem()
		{
			if (_pupils == null)
				_pupils = new SortedList<FIO, SortedList<DateTime, PupilDay>>();
			if (_days == null)
				_days = new SortedList<DateTime, Cost>();
		}

		static ClassItem()
		{
			if(_thisObject == null)
			{
				_thisObject = new ClassItem();
				_thisObject.Deserialize();

				ThreadPool.QueueUserWorkItem(delegate {
					while(true)
					{
						var date = DateTime.Now;
						var dateStr = date.ToString("yyyy_MM_dd-HH_mm_ss");

						_thisObject.Serialize(dateStr + "-");
						var minutes = 30;
						Thread.Sleep(minutes * 60 * 1000);
					}
				});
			}
		}

		private void Deserialize()
		{
			// var dataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var dataPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)
			               + "/../Documents/SchoolMeals";

			if(File.Exists(dataPath + "/PupilsDataFile.dat") && File.Exists(dataPath + "/CostDataFile.dat"))
			{
				var fsPupils = new FileStream(dataPath + "/PupilsDataFile.dat", FileMode.Open);
				var fsCost = new FileStream(dataPath + "/CostDataFile.dat", FileMode.Open);

				BinaryFormatter formatter = new BinaryFormatter();

				this._pupils = (SortedList<FIO, SortedList<DateTime, PupilDay>>)formatter.Deserialize(fsPupils);
				this._days = (SortedList<DateTime, Cost>)formatter.Deserialize(fsCost);

				fsPupils.Close();
				fsCost.Close();
			}
		}

		public void Serialize(string appendToFileName)
		{
			var dataPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)
			               + "/../Documents/SchoolMeals";

			if(!Directory.Exists(dataPath))
				Directory.CreateDirectory(dataPath);

			string append = string.Empty;
			if(!string.IsNullOrEmpty(appendToFileName))
				append = appendToFileName;
			var fsPupils = new FileStream(dataPath + "/" + append + "PupilsDataFile.dat", FileMode.Create);
			var fsCost = new FileStream(dataPath + "/" + append + "CostDataFile.dat", FileMode.Create);

			BinaryFormatter formatter = new BinaryFormatter();

			formatter.Serialize(fsPupils, this._pupils);
			formatter.Serialize(fsCost, this._days);

			fsPupils.Close();
			fsCost.Close();
		}

		private void WriteString(FileStream stream, int codePage, string dataString)
		{
			if(dataString != null && stream.CanWrite)
			{
				var encoder = System.Text.Encoding.GetEncoding(codePage);
				var data = encoder.GetBytes(dataString);
				stream.Write(data, 0, data.Length);
			}
		}

		public void FormReport(DateTime startDate, DateTime endDate)
		{
			var cp = 1251;

			var dataPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)
			               + "/../Documents/SchoolMeals";

			if(!Directory.Exists(dataPath))
				Directory.CreateDirectory(dataPath);

			using(var report = new FileStream(dataPath + "/" + "Report(full) - " + DateTime.Today.ToString("yyyy_MM_dd") + ".csv", FileMode.Create))
			{
				this.WriteString(report, cp, ";");
				for(var date = startDate; date <= endDate; date = date.AddDays(1))
				{
					if(this._days.ContainsKey(date))
					{
						var withDiscount = this._days[date].WithDiscount;
						this.WriteString(report, cp, withDiscount == 0 ? string.Empty : withDiscount.ToString());
					}

					this.WriteString(report, cp, ";");
				}
				this.WriteString(report, cp, Environment.NewLine);

				this.WriteString(report, cp, ";");
				for(var date = startDate; date <= endDate; date = date.AddDays(1))
				{
					if(this._days.ContainsKey(date))
					{
						var normal = this._days[date].Normal;
						this.WriteString(report, cp, normal == 0 ? string.Empty : normal.ToString());
					}

					this.WriteString(report, cp, ";");
				}
				this.WriteString(report, cp, Environment.NewLine);

				this.WriteString(report, cp, ";");
				for(var date = startDate; date <= endDate; date = date.AddDays(1))
				{
					this.WriteString(report, cp, date.Date.ToString("dd MMMMM yyyy") + ";");
				}
				this.WriteString(report, cp, Environment.NewLine);

				foreach(var pupil in this._pupils)
				{
					var pupilName = string.Format("{0} {1}{2}{3}", pupil.Key.LastName,
						pupil.Key.FirstName.Length > 0 ? pupil.Key.FirstName.Substring(0,1) : string.Empty, pupil.Key.IsDiscount ? " л." : string.Empty,
						";");
					this.WriteString(report, cp, pupilName);

					for(var date = startDate; date <= endDate; date = date.AddDays(1))
					{
						if(pupil.Value.ContainsKey(date))
							this.WriteString(report, cp, pupil.Value[date].Presence ? "+" : "н");

						this.WriteString(report, cp, ";");
					}
					this.WriteString(report, cp, Environment.NewLine);

					this.WriteString(report, cp, ";");
					for(var date = startDate; date <= endDate; date = date.AddDays(1))
					{
						if(pupil.Value.ContainsKey(date))
							this.WriteString(report, cp, pupil.Value[date].Comment != null ? pupil.Value[date].Comment.Replace(Environment.NewLine, " ") : string.Empty);

						this.WriteString(report, cp, ";");
					}
					this.WriteString(report, cp, Environment.NewLine);

					this.WriteString(report, cp, ";");
					for(var date = startDate; date <= endDate; date = date.AddDays(1))
					{
						if(pupil.Value.ContainsKey(date))
						{
							var payment = pupil.Value[date].Payment;
							this.WriteString(report, cp, payment == 0 ? string.Empty : payment.ToString());
						}

						this.WriteString(report, cp, ";");
					}
					this.WriteString(report, cp, Environment.NewLine);

					this.WriteString(report, cp, ";");
					for(var date = startDate; date <= endDate; date = date.AddDays(1))
					{
						if(pupil.Value.ContainsKey(date))
							this.WriteString(report, cp, pupil.Value[date].IsDiscount ? " л." : string.Empty);

						this.WriteString(report, cp, ";");
					}
					this.WriteString(report, cp, Environment.NewLine);
				}
			}

			using(var report = new FileStream(dataPath + "/" + "Report(presence) - " + DateTime.Today.ToString("yyyy_MM_dd") + ".csv", FileMode.Create))
			{
				this.WriteString(report, cp, ";");
				for(var date = startDate; date <= endDate; date = date.AddDays(1))
				{
					this.WriteString(report, cp, date.Date.ToString("dd MMMMM yyyy") + ";");
				}
				this.WriteString(report, cp, Environment.NewLine);

				foreach(var pupil in this._pupils)
				{
					var pupilName = string.Format("{0} {1}{2}{3}", pupil.Key.LastName,
						pupil.Key.FirstName.Length > 0 ? pupil.Key.FirstName.Substring(0,1) : string.Empty, pupil.Key.IsDiscount ? " л." : string.Empty,
						";");
					this.WriteString(report, cp, pupilName);

					for(var date = startDate; date <= endDate; date = date.AddDays(1))
					{
						if(pupil.Value.ContainsKey(date))
							this.WriteString(report, cp, pupil.Value[date].Presence ? "+" : "н");

						this.WriteString(report, cp, ";");
					}
					this.WriteString(report, cp, Environment.NewLine);
				}
			}
		}

		public static ClassItem Instance
		{
			get
			{
				return _thisObject;
			}
		}

		public bool AddOrEditPupil(FIO fioOld, FIO fioNew)
		{
			if (_pupils.Keys.Any(fio => fio.Equals (fioNew) && fioNew != null && fio.IsDiscount == fioNew.IsDiscount))
			 	return false;
			if (fioOld == null)
			{
				if(!_pupils.ContainsKey(fioNew))
				{
					_pupils.Add (fioNew, new SortedList<DateTime, PupilDay> ());
					return true;
				}
				else
					return false;
			}
			else if (_pupils.ContainsKey (fioOld))
			{
				var tmpfio = _pupils.Keys.FirstOrDefault (fio => fio.Equals (fioOld));
				if (tmpfio == default(FIO))
					return false;
				else
				{
					var tmpPupilDays = _pupils[tmpfio];
					_pupils.Remove(tmpfio);
					_pupils.Add(fioNew, tmpPupilDays);
					return true;
				}
			}
			else
				return false;
		}

		public bool RemovePupil(FIO fio)
		{
			if (fio == null)
				return false;
			else if (_pupils.ContainsKey (fio))
			{
				_pupils.Remove (fio);
				return true;
			}
			return false;
		}

		public KeyValuePair<DateTime, Cost> GetCost(DateTime date)
		{
			var cost = this._days.FirstOrDefault (d => d.Key.Equals(date));
			KeyValuePair<DateTime, Cost> outCost;
			if (cost.Equals(default(KeyValuePair<DateTime, Cost>)))
				outCost = new KeyValuePair<DateTime, Cost> (date, new Cost());
			else
				outCost = new KeyValuePair<DateTime, Cost> (date, cost.Value);
			return outCost;
		}

		public void AddOrEditCost(DateTime date, Cost cost)
		{
			if (_days.ContainsKey (date))
				_days.Remove (date);
			_days.Add (date, cost);
		}

		public decimal GetAmount(DateTime date, FIO fio)
		{
			if (_pupils.ContainsKey(fio) && this._pupils[fio].ContainsKey(date))
				return this._pupils[fio][date].Payment;
			return 0;
		}

		public bool Pay(DateTime date, FIO fio, decimal payment)
		{
			if (this._pupils.ContainsKey (fio))
			{
				var pupil = this._pupils [fio];
				if (pupil.ContainsKey (date))
				{
					var pupilDay = pupil [date];
					pupilDay.Payment = payment;
				}
				else
				{
					pupil.Add (date, new PupilDay {
						Payment = payment,
						Presence = true,
						IsDiscount = this._pupils.First(pupilItem => pupilItem.Key.Equals(fio)).Key.IsDiscount
					});
				}
				return true;
			}
			else
				return false;
		}

		public SortedList<FIO, PupilDay> GetPupilList(DateTime date)
		{
			var outList = new SortedList<FIO, PupilDay> ();
			foreach (var pupil in this._pupils)
			{
				var pupilDayEntry = pupil.Value.FirstOrDefault (p => p.Key.Equals (date));
				PupilDay pupilDay;
				if (pupilDayEntry.Value == null)
					pupilDay = new PupilDay { IsDiscount = pupil.Key.IsDiscount };
				else
					pupilDay = new PupilDay {
						IsDiscount = pupilDayEntry.Value.IsDiscount,
						Payment = pupilDayEntry.Value.Payment,
						Presence = pupilDayEntry.Value.Presence,
						Comment = pupilDayEntry.Value.Comment
					};

				var key = new FIO {
					FirstName = pupil.Key.FirstName,
					IsDiscount = pupil.Key.IsDiscount,
					LastName = pupil.Key.LastName
				};
				outList.Add (key, pupilDay);
			}
			return outList;
		}

		public void SetPresenceAndDiscount(DateTime date, SortedList<FIO, PupilDay> inList)
		{
			foreach (var inListItem in inList)
			{
				var pupil = this._pupils [inListItem.Key];
				var pupilDay = pupil.FirstOrDefault (p => p.Key.Equals (date));
				if (pupilDay.Equals(default(KeyValuePair<DateTime, PupilDay>)))
					pupil.Add (date, inListItem.Value);
				else
				{
					pupil.Remove(date);
					pupil.Add(date, new PupilDay{
						IsDiscount = inListItem.Value.IsDiscount,
						Payment = pupilDay.Value.Payment,
						Presence = inListItem.Value.Presence,
						Comment = inListItem.Value.Comment
					});
				}
			}
		}

		public SortedList<FIO, decimal> Calculate()
		{
			var output = new SortedList<FIO, decimal>();

			foreach(var pupilProfile in this._pupils)
			{
				decimal balance = 0;

				foreach(var pupilDayEntry in pupilProfile.Value)
				{
					balance += pupilDayEntry.Value.Payment;

					if(pupilDayEntry.Value.Presence)
					{
						var cost = this._days.FirstOrDefault(day => day.Key.Equals(pupilDayEntry.Key));
						if(!cost.Equals(default(KeyValuePair<DateTime, Cost>)))
						{
							balance -= pupilDayEntry.Value.IsDiscount ? cost.Value.WithDiscount : cost.Value.Normal;
						}
					}
				}

				output.Add(pupilProfile.Key, balance);
			}

			return output;
		}
	}
}

