using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class TimeFrame
    {
        private DateTime _startTime;
        private DateTime _endTime;

        public DateTime StartTime 
        { 
            get {  return _startTime; } 
            set {  _startTime = value; } 
        }
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        Regex timeFormat = new Regex(@"^[0-1][0-9]:[0-6][0-9]\s[A|P]M$");
        public TimeFrame(string startTime, string endTime) 
        {
            if (DateTime.TryParse(startTime, out _startTime) && DateTime.TryParse(endTime, out _endTime))
            {
                Debug.WriteLine(StartTime.ToString() + " : " + EndTime.ToString());
            }
            else
            {
                throw new Exception("Failed to construct TimeFrame object.");
            }

        }
    }
}
