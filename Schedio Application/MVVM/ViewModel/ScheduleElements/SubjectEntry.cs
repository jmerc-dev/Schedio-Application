using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class SubjectEntry
    {
        private Subject _Subject;
        private string? _StartTime;
        private double _UnitsToAllocate;
        private Room? _Room;
        private DayOfWeek? _DayAssigned;

        public Subject SubjectInfo 
        { 
            get {  return _Subject; } 
            set {  _Subject = value; }
        }

        public string? StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        public double UnitsToAllocate
        {
            get { return _UnitsToAllocate; }
            set { _UnitsToAllocate = value; }
        }

        public Room? RoomAllocated
        {
            get { return _Room; }
            set { _Room = value; }
        }

        public DayOfWeek? DayAssigned
        {
            get { return _DayAssigned; }
            set { _DayAssigned = value; }
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
