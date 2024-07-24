using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
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

        private string _name;
        private string _type;

        public string Name 
        { 
            get { return _name; }
            set 
            { 
                _name = value;
                OnPropertyChanged();
            } 
        }
        public string Type 
        {
            get { return _type; }
            set 
            { 
                _type = value;
                OnPropertyChanged();
            }
        }

        public Room(string name, string type)
        {
            this._name = name;
            this._type = type;
        }

        public bool Update()
        {
            RoomAddForm form = new RoomAddForm(Name, Type);
            if (form.ShowDialog() == true)
            {
                Name = form.RoomName;
                Type = form.RoomType;
                return true;
            }
            return false;
        }

    }
}
