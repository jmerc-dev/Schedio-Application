using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{

    public enum EntryFilterElement
    {
        Person,
        Room,
        Section,
        Subject
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
            switch (e)
            {
                case EntryFilterElement.Person:
                    break;
                case EntryFilterElement.Room:
                    break;
                case EntryFilterElement.Section:
                    break;
                case EntryFilterElement.Subject:
                    break;
                default: 
                    return false;
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
        
    }
}
