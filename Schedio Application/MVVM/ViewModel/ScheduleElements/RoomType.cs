using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class RoomType : PropertyNotification
    {
        private string _Name;

        public string Name 
        {
            get { return _Name; } 
            set 
            {  
                _Name = value;
                OnPropertyChanged();
            } 
        }

        public RoomType(string name)
        {
            _Name = name;
        }

        public RoomType()
        {
            _Name = "";
        }
    }
}
