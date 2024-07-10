using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    internal class TimeFrame
    {
        private DateTime _startTime;
        private DateTime _endTime;

        public TimeFrame(string startTime, string endTime) 
        {
            if (DateTime.TryParse(startTime, out this._startTime) && DateTime.TryParse(endTime, out this._endTime))
            {
                MessageBox.Show("Failed to parse TimeFrame");
                return;
            }
            

        }
    }
}
