using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    internal class TimeFrame
    {
        private DateTime _startTime;
        private DateTime _endTime;

        Regex timeFormat = new Regex(@"^[0-1][0-9]:[0-6][0-9]\s[A|P]M$");
        public TimeFrame(Time startTime, Time endTime) 
        {

        }
    }
}
