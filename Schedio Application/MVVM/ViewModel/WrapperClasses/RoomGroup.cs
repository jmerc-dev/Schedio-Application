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
    public class RoomGroup
    {
        private int _IdCounter;
        public ObservableCollection<Room>? _Rooms { get; set; }

        public int IdCounter
        {
            get => _IdCounter;
            set => _IdCounter = value;
        }

        public ObservableCollection<Room>? Rooms
        {
            get => _Rooms;
            set
            {
                _Rooms = value;
                if (_Rooms == null)
                    throw new NullReferenceException();
                _Rooms.CollectionChanged += RoomAddedInCollection;
            }
        }

        private void RoomAddedInCollection(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                    throw new NullReferenceException();
                if (e.NewItems.Count == 1)
                {
                    if (e.NewItems[0] == null)
                        throw new NullReferenceException();

                    Room? newRoom = (Room?)e.NewItems[0];

                    if (newRoom == null)
                        throw new NullReferenceException();
                    newRoom.ID = Interlocked.Increment(ref _IdCounter);
                }
            }
        }
    }
}
