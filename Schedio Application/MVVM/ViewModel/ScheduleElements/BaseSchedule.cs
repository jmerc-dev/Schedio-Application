using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class BaseSchedule
    {
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
