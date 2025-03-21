using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Room : PropertyNotification
    {
        private static ObservableCollection<Room> _Rooms = new ObservableCollection<Room>();
        private static int _IdCounter; // Auto initialize when loading a file
        private int _ID;
        private string _name;
        private RoomType _type;

        public static ObservableCollection<Room> RoomsList
        {
            get { return _Rooms; }
        }

        public int ID
        {
            get => _ID;
        }

        public static int IDCounter
        {
            get => IDCounter;
        }

        public string Name 
        { 
            get { return _name; }
            set 
            { 
                _name = value;
                OnPropertyChanged();
            } 
        }
        public RoomType Type 
        {
            get { return _type; }
            set 
            { 
                _type = value;
                OnPropertyChanged();
            }
        }

        public Room(string name, RoomType type)
        {
            this._name = name;
            this._type = type;
            this._ID = Interlocked.Increment(ref _IdCounter);
        }

        public Room(string name, RoomType type, int id)
        {
            this._name = name;
            this._type = type;
            this._ID = id;
        }

        public string Serialize()
        {
            return Name;
        }

        public static Room? RoomExists(string roomname) 
        {
            foreach (Room room in Room.RoomsList)
            {
                if (room.Name.Equals(roomname, StringComparison.CurrentCulture))
                {
                    return room;
                }
            }
            return null;
        }
    }
}
