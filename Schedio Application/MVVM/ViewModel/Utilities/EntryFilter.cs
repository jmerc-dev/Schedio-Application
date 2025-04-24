using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{

    public enum EntryFilterElement
    {
        None,
        Person,
        Room,
        Section,
        Subject,
        Day
    }

    /// <summary>
    /// This class only handles subject entry filtering
    /// </summary>
    public class EntryFilter
    {
        private ObservableCollection<SubjectEntry> _Source;
        private ObservableCollection<SubjectEntry> _Filter;
        public EntryFilter(ObservableCollection<SubjectEntry> source, ObservableCollection<SubjectEntry> filter) 
        {
            this._Source = source;
            this._Filter = filter;
        }

        /// <summary>
        ///  Use FilterByDays method for day-related filtering.
        /// </summary>
        public bool Filter(EntryFilterElement e, string key)
        {
            if (e == EntryFilterElement.Day || e == EntryFilterElement.None)
                throw new InvalidEnumArgumentException();

            foreach (SubjectEntry s in _Source)
            {
                bool result = EntryFilter.CheckKey(e, s, key);
                Execute(result, s);
            }

            return true;
        }

        /// <summary>
        /// The daysFilter length should be 7, corresponding to the number of days in a week. Indexing: 0 -> Monday to 6 -> Sunday.
        /// </summary>
        public bool FilterByDays(bool[] daysFilter)
        {
            if (daysFilter.Length != 7)
                throw new ArgumentException("Days Filter array should be equal to 7.");

            for (int i = 0; i < 7; i++)
            {
                DayOfWeek day = CultureDayOfWeek.Convert(i);
                bool status = false;
                foreach (SubjectEntry entry in _Source)
                {
                    if (entry.DayAssigned == day)
                    {
                        status = daysFilter[i] == true ? AddIfAbsent(entry) : RemoveIfPresent(entry);
                    }
                }
            }
            return true;
        }

        public static bool CheckKey(EntryFilterElement e, SubjectEntry s, string key)
        {
            bool result = false;
            switch (e)
            {
                case EntryFilterElement.Person:
                    result = s.SubjectInfo.AssignedPerson.Name != null && s.SubjectInfo.AssignedPerson.Name.Contains(key, StringComparison.CurrentCultureIgnoreCase);
                    break;
                case EntryFilterElement.Room:
                    result = s.RoomAllocated != null && s.RoomAllocated.Name.Contains(key, StringComparison.CurrentCultureIgnoreCase);
                    break;
                case EntryFilterElement.Section:
                    result = s.SubjectInfo.OwnerSection != null && s.SubjectInfo.OwnerSection.Name != null && s.SubjectInfo.OwnerSection.Name.Contains(key, StringComparison.CurrentCultureIgnoreCase);
                    break;
                case EntryFilterElement.Subject:
                    result = s.SubjectInfo.Name != null && s.SubjectInfo.Name.Contains(key, StringComparison.CurrentCultureIgnoreCase);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
            return result;
        }

        private bool AddIfAbsent(SubjectEntry entry)
        {
            if (!_Filter.Contains<SubjectEntry>(entry))
                _Filter.Add(entry);

            return true;
        }

        private bool RemoveIfPresent(SubjectEntry entry)
        {
            if (_Filter.Contains<SubjectEntry>(entry))
                _Filter.Remove(entry);

            return true;
        }
        private void Execute(bool result, SubjectEntry s)
        {
            if (result)
                AddIfAbsent(s);
            else
                RemoveIfPresent(s);
        }

    }
}
