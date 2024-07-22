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

        public string StartTime 
        { 
            get {  return _startTime.ToString("hh:mm tt").ToUpper(); } 
            set 
            {  
                try
                {
                    _startTime = DateTime.Parse(value);
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } 
        }
        public string EndTime
        {
            get { return _endTime.ToString("hh:mm tt").ToUpper(); }
            set
            {
                try
                {
                    _endTime = DateTime.Parse(value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static Regex timeFormat = new Regex(@"^[0-1][0-9]:[0-6][0-9]\s[A|P]M$");

        public static bool ValidateTimeFormat(string time)
        {
            if (timeFormat.Match(time).Success == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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
            } catch (Exception ex)
            {
                MessageBox.Show($"Cannot parse {timeframe}");
                throw new FormatException(ex.Message);
            }
        }

        public TimeFrame(string startTime, string endTime) 
        {
            if (DateTime.TryParse(startTime, out _startTime) && DateTime.TryParse(endTime, out _endTime))
            {

            }
            else
            {
                throw new Exception("Failed to construct TimeFrame object.");
            }

        }
    }
}
