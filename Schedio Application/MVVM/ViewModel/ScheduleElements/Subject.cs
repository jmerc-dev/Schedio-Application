using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Commands;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Subject : PropertyNotification
    {
        private string _Name;
        private Person _AssignedPerson;
        private RoomType _RoomType;
        private int _Units;

        private double _UnitsRemaining;
        private bool IsAllocated;

        private ObservableCollection<Room> _Rooms;
        public RelayCommand AllocSubjectCommand => new RelayCommand(execute => AllocSubject());

        public string Name 
        { 
            get { return _Name; }
            set 
            { 
                _Name = value;
                OnPropertyChanged();
            } 
        }

        public Person AssignedPerson
        {
            get { return _AssignedPerson; }
            set 
            { 
                _AssignedPerson = value;
                OnPropertyChanged();
            }
        }

        public RoomType RoomType
        {
            get { return _RoomType; }
            set 
            { 
                _RoomType = value;
                OnPropertyChanged();
            }
        }

        public int Units
        {
            get { return _Units; }
            set 
            { 
                _Units = value;
                OnPropertyChanged();
            }
        }

        public Subject(string Name, Person assignedPerson, RoomType roomType, int units)
        {
            _Name = Name;
            _AssignedPerson = assignedPerson;
            _RoomType = roomType;
            _Units = units;
        }

        public Subject()
        {

        }

        public Subject(Subject subject)
        {
            this.Name = subject.Name;
            this.RoomType = subject.RoomType;
            this.AssignedPerson = subject.AssignedPerson;
            this.Units = subject.Units;
        }


        // Entries CRUD
        private void AllocSubject()
        {
            SubjectAllocation subjectAllocation = new SubjectAllocation(this);
            if (subjectAllocation.ShowDialog() == true)
            {
                foreach (Room room in Workshop.Rooms)
                {
                    Trace.WriteLine(room.Name);
                }
            }
        }
    }
}
