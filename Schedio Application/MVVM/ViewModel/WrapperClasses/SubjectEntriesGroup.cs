using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.WrapperClasses
{
    public class SubjectEntriesGroup
    {
        private ObservableCollection<SubjectEntry>? _entries;
        private int _IdCounter;

        public int IdCounter
        {
            get => _IdCounter;
            set => _IdCounter = value;
        }

        public ObservableCollection<SubjectEntry>? SubjectEntries 
        {
            get => _entries; 
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _entries = value;

                _entries.CollectionChanged += SubjectEntryAddedInCollection;
            } 
        }

        private void SubjectEntryAddedInCollection(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                    throw new NullReferenceException();
                if (e.NewItems.Count == 1)
                {
                    if (e.NewItems[0] == null)
                        throw new NullReferenceException();

                    SubjectEntry? newSubjectEntry = (SubjectEntry?)e.NewItems[0];

                    if (newSubjectEntry == null)
                        throw new NullReferenceException();
                    newSubjectEntry.ID = Interlocked.Increment(ref _IdCounter);
                }
            }
        }
    }
}
