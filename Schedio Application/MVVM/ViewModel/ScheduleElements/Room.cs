﻿using Schedio_Application.MVVM.View.Windows;
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
        
        private int _ID;
        private string _name;
        private RoomType _type;

        public static ObservableCollection<Room> RoomsList
        {
            get => _Rooms;
            set => _Rooms = value;
        }

        public int ID
        {
            get => _ID;
            set => _ID = value;
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

        public int RoomTypeID { get; set; }

        public Room() { }

        public Room(string name, RoomType type)
        {
            this._name = name;
            this._type = type;
        }

        public Room(string name, RoomType type, int id)
        {
            this._name = name;
            this._type = type;
            this._ID = id;
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
