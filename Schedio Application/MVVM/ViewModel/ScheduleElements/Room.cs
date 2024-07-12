using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Room
    {

        private string _name;
        private string _type;

        public string Name 
        { 
            get { return _name; }
            set { _name = value; } 
        }
        public string Type 
        {
            get { return _type; }
            set { _type = value; }
        }

        public Room(string name, string type)
        {
            this._name = name;
            this._type = type;
        }

    }
}
