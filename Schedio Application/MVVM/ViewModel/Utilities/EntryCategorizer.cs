using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{
    public class EntryCategorizer
    {
        private readonly ObservableCollection<SubjectEntry> _Entries;
        private ObservableCollection<SubjectEntry> MyProperty { get => _Entries; }

        public EntryCategorizer(ObservableCollection<SubjectEntry> entries)
        {
            this._Entries = entries;
        }

        /// <summary>
        /// Grouping of entries exclusively by Day.
        /// </summary>
        /// <returns>ObservableCollection of maximum size 7 or else null</returns>
        public ObservableCollection<DayBasedCategory>? CategorizeByDay()
        {
            ObservableCollection <DayBasedCategory> groupedByDayList = new ObservableCollection<DayBasedCategory>();
            Dictionary<DayOfWeek, int> elementCountPair = new Dictionary<DayOfWeek, int>();

            foreach (SubjectEntry entry in _Entries) 
            {
                DayOfWeek day = entry.DayAssigned;
                if (elementCountPair.ContainsKey(day))
                    elementCountPair[day] += 1;
                else
                    elementCountPair[day] = 1;
            }

            if (elementCountPair.Count > 7)
                throw new ArgumentOutOfRangeException();

            foreach (KeyValuePair<DayOfWeek, int> kvp in elementCountPair)
            {
                groupedByDayList.Add(new DayBasedCategory(kvp.Key, kvp.Value));
            }

            return groupedByDayList;
        }

        /// <summary>
        /// Only accepts Person, Room, and ClassSection of enum ScheduleElement.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>List of EntryCategoryCounter which has both the element value and count.</returns>
        public ObservableCollection<EntryCategoryCounter>? CategorizeBy(ScheduleElement element)
        {
            ObservableCollection<EntryCategoryCounter> groupedList = new ObservableCollection<EntryCategoryCounter>();
            Dictionary<IScheduleElement, int>? elementCountPair = GetCountBasedOnElement(element);

            if (elementCountPair == null)
                return null;

            foreach (KeyValuePair<IScheduleElement, int> item in elementCountPair)
            {
                groupedList.Add(new EntryCategoryCounter(item.Key, item.Value));
            }

            return groupedList;
        }


        private Dictionary<IScheduleElement, int>? GetCountBasedOnElement(ScheduleElement element)
        {
            Dictionary<IScheduleElement, int> elementCountPairs = new Dictionary<IScheduleElement, int>();

            foreach (SubjectEntry entry in _Entries)
            {
                switch (element)
                {
                    case ScheduleElement.Person:
                        RegisterEntryElement(elementCountPairs, entry.SubjectInfo.AssignedPerson);
                        break;
                    case ScheduleElement.Room:
                        RegisterEntryElement(elementCountPairs, entry.RoomAllocated);
                        break;
                    case ScheduleElement.ClassSection:
                        RegisterEntryElement(elementCountPairs, entry.SubjectInfo.OwnerSection);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return elementCountPairs;
        }

        private void RegisterEntryElement(Dictionary<IScheduleElement, int> elementCountPair, IScheduleElement? element)
        {
            if (element == null)
                return;

            if (elementCountPair.ContainsKey(element))
                elementCountPair[element] += 1;
            else
                elementCountPair.Add(element, 1);
        }

    }
}
