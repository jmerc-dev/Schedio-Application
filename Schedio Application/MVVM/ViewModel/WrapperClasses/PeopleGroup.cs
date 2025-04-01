using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.WrapperClasses
{
    public class PeopleGroup
    {
        private int _idCounter;

        public int IdCounter 
        { 
            get => Person.IdCounter;
            set => _idCounter = value;
        }
        public ObservableCollection<Person>? People { get; set; }
    }
}
