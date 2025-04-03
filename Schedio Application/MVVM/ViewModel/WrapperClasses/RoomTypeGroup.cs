using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Schedio_Application.MVVM.ViewModel.WrapperClasses
{
    public class RoomTypeGroup
    {
        private int _IdCounter;
        public ObservableCollection<RoomType>? _RoomTypes;
        public int IdCounter
        {
            get => _IdCounter;
            set 
            {
                _IdCounter = value;
            }
        }
        public ObservableCollection<RoomType>? RoomTypes 
        { 
            get => _RoomTypes; 
            set
            {
                _RoomTypes = value;

                if (_RoomTypes == null)
                    throw new NullReferenceException();
                _RoomTypes.CollectionChanged += RoomTypeAddedInCollection;
            } 
        }

        private void RoomTypeAddedInCollection(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null || e.NewItems[0] == null)
                    throw new NullReferenceException();

                if (e.NewItems.Count == 1)
                {
                    RoomType? newRoomType = (RoomType?) e.NewItems[0];

                    if (newRoomType == null)
                        throw new NullReferenceException();

                    newRoomType.ID = Interlocked.Increment(ref _IdCounter);
                }
            }
        }
    }
}
