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

        public DateTime[] ConvertToDateTime(string timeframe)
        {
            // 12:00 AM - 12:00 PM
            string[] timeframe_Split = timeframe.Split('-');

            for (int i = 0; i < timeframe_Split.Length; i++)
            {
                timeframe_Split[i] = timeframe_Split[i].Trim();
            }

            DateTime[] convertedTimeframe = new DateTime[2];
            try
            {
                for (int i = 0; i< convertedTimeframe.Length; i++)
                {
                    convertedTimeframe[i] = DateTime.Parse(timeframe_Split[i]);
                }

                return convertedTimeframe;
            } catch 
            {
                throw new FormatException("Timeframe is not in the format: 00:00 AM - 00:00 PM");
            }
        }

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
