using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class BaseSchedule
    {
        public static string[] TimeBreakpoints = 
            { "12:00 AM", "1:00 AM", "2:00 AM", "3:00 AM", "4:00 AM", "5:00 AM",
              "6:00 AM", "7:00 AM", "8:00 AM", "9:00 AM", "10:00 AM", "11:00 AM",
              "12:00 PM", "1:00 PM", "2:00 PM", "3:00 PM", "4:00 PM", "5:00 PM",
              "6:00 PM", "7:00 PM", "8:00 PM", "9:00 PM", "10:00 PM", "11:00 PM",
              "12:00 AM"};

        private bool _IsConstant;
        private Dictionary<DayOfWeek, TimeFrame>? _DailyTimeframe;

        public bool IsConstant
        {
            get { return _IsConstant; }
            set { _IsConstant = value; }
        }

        public Dictionary<DayOfWeek, TimeFrame>? DailyTimeframe
        {
            get { return _DailyTimeframe; }
            set { _DailyTimeframe = value; }
        }

        public BaseSchedule()
        {

        }

        public bool IsAvailable(DayOfWeek day)
        {
            if (_DailyTimeframe != null)
            {
                return _DailyTimeframe.ContainsKey(day);
            }
            return false;
        }
    }
}
