using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class SubjectEntry : PropertyNotification
    {
        private Subject _Subject;
        private string? _StartTime;
        private double _UnitsToAllocate;
        private Room? _Room;
        private DayOfWeek? _DayAssigned;

        public Subject SubjectInfo 
        { 
            get {  return _Subject; } 
            set 
            {  
                _Subject = value;
                OnPropertyChanged();
            }
        }

        public string? StartTime
        {
            get { return _StartTime; }
            set 
            { 
                _StartTime = value;
                OnPropertyChanged();
            }
        }

        public double UnitsToAllocate
        {
            get { return _UnitsToAllocate; }
            set 
            { 
                _UnitsToAllocate = value;
                OnPropertyChanged();
            }
        }

        public Room? RoomAllocated
        {
            get { return _Room; }
            set 
            { 
                _Room = value;
                OnPropertyChanged();
            }
        }

        public DayOfWeek? DayAssigned
        {
            get { return _DayAssigned; }
            set 
            { 
                _DayAssigned = value;
                OnPropertyChanged();
            }
        }

        public SubjectEntry(Subject subject)
        {
            _Subject = subject;
        }

        public SubjectEntry(Subject subject, string startTime, double units, Room room)
        {
            _Subject = subject;
            _StartTime = startTime;
            _UnitsToAllocate = units;
            _Room = room;
        }

    }
}
