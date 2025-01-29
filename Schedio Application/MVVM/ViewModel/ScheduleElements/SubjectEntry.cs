using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public enum ScheduleElement
    {
        Person,
        Room,
        RoomType,
        Subject,
        Day,
        ClassSection
    }
    public class SubjectEntry : PropertyNotification
    {
        private Subject _Subject;
        private string? _StartTime;
        private string? _EndTime;
        private TimeFrame _TimeFrame;
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

        public TimeFrame TimeFrame
        {
            get => _TimeFrame;
            set => _TimeFrame = value;
        }

        public string? StartTime
        {
            get { return _StartTime; }
            set 
            { 
                _StartTime = value;

                if (value != null)
                {
                    DateTime endTime = DateTime.Parse(value).AddHours(UnitsToAllocate);
                    DateTime endTime1 = DateTime.Parse(value).AddHours(3);
                    EndTime = endTime.ToString("hh:mm tt");
                }
                OnPropertyChanged();
            }
        }

        public string? EndTime
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value;
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
            _TimeFrame = new TimeFrame();
        }

        public SubjectEntry(Subject subject, TimeFrame tf, double units, Room room)
        {
            _Subject = subject;
            _TimeFrame = tf;
            _UnitsToAllocate = units;
            _Room = room;
        }

    }
}
