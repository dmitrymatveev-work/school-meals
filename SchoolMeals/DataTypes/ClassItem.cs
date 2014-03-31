using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
			}

		}

		private void Deserialize()
		{
			var dataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			if(File.Exists(dataPath + "/PupilsDataFile.dat") && File.Exists(dataPath + "/CostDataFile.dat"))
			{
				FileStream fsPupils = new FileStream(dataPath + "/PupilsDataFile.dat", FileMode.Open);
				FileStream fsCost = new FileStream(dataPath + "/CostDataFile.dat", FileMode.Open);

				BinaryFormatter formatter = new BinaryFormatter();

				this._pupils = (SortedList<FIO, SortedList<DateTime, PupilDay>>)formatter.Deserialize(fsPupils);
				this._days = (SortedList<DateTime, Cost>)formatter.Deserialize(fsCost);

				fsPupils.Close();
				fsCost.Close();
			}
		}

		public void Serialize()
		{
			var dataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			FileStream fsPupils = new FileStream(dataPath + "/PupilsDataFile.dat", FileMode.Create);
			FileStream fsCost = new FileStream(dataPath + "/CostDataFile.dat", FileMode.Create);

			BinaryFormatter formatter = new BinaryFormatter();

			formatter.Serialize(fsPupils, this._pupils);
			formatter.Serialize(fsCost, this._days);

			fsPupils.Close();
			fsCost.Close();
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
				_pupils.Add (fioNew, new SortedList<DateTime, PupilDay> ());
				return true;
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
			if (_pupils.ContainsKey (fio))
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
						IsDiscount = fio.IsDiscount
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
						Payment = inListItem.Value.Payment,
						Presence = inListItem.Value.Presence,
						Comment = inListItem.Value.Comment
					});
				}
			}
		}
	}
}

