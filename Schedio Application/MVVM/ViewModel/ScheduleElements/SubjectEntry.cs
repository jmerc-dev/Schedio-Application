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
        private double? _UnitsToAllocate;
        private Room? _Room;

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
