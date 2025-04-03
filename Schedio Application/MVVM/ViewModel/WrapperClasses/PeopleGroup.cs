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
    public class PeopleGroup
    {
        private int _idCounter;
        public ObservableCollection<Person>? _People;

        public int IdCounter 
        { 
            get => _idCounter;
            set => _idCounter = value;
        }
        public ObservableCollection<Person>? People 
        { 
            get => _People;
            set 
            {
                _People = value;
                if (_People == null)
                    throw new NullReferenceException();
                _People.CollectionChanged += PersonAddedInCollection;
            } 
        }

        private void PersonAddedInCollection(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                    throw new NullReferenceException();
                if (e.NewItems.Count == 1)
                {
                    if (e.NewItems[0] == null)
                        throw new NullReferenceException();

                    Person? newPerson = (Person?) e.NewItems[0];

                    if (newPerson == null)
                        throw new NullReferenceException();
                    newPerson.ID = Interlocked.Increment(ref _idCounter);
                }
            }
        }

    }
}
