using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class RoomType : PropertyNotification
    {
        private string _Name;

        private int _ID;
        private static int _IdCounter = 0;
        
        public static int IdCounter
        {
            get => _IdCounter;
            set => _IdCounter = value;
        }

        public int ID
        {
            get => _ID;
            set => _ID = value;
        }

        public string Name 
        {
            get { return _Name; } 
            set 
            {  
                _Name = value;
                OnPropertyChanged();
            } 
        }

        public RoomType() { }
        public RoomType(string name)
        {
            _Name = name;
            this._ID = Interlocked.Increment(ref _IdCounter);
        }

        public RoomType(string name, int id)
        {
            _Name = name;
            _ID = id;
        }
    }
}
