﻿using Schedio_Application.MVVM.ViewModel.Custom_Exceptions;
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

        private static bool ValidateTimeframe(string startTime, string endTime)
        {
            if (ValidateTimeFormat(startTime) && ValidateTimeFormat(endTime))
            {
                TimeSpan start = TimeSpan.Parse(DateTime.Parse(startTime).ToString("HH:mm"));
                TimeSpan end = TimeSpan.Parse(DateTime.Parse(endTime).ToString("HH:mm"));

                if (start < end)
                {
                    return true;
                }

                return false;
            } 
            else
            {
                MessageBox.Show("Invalid time format");
                return false;
            }
        }

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
                if (!ValidateTimeframe(startTime, endTime))
                {
                    throw new InvalidTimeFrameException(startTime, endTime);
                }
            }
            else
            {
                throw new FormatException($"InvalidFormat of {startTime} or {endTime}");
            }
        }

        public bool IsOverlap(string time)
        {

            try
            {
                TimeSpan startTime = TimeSpan.Parse(DateTime.Parse(StartTime).ToString("HH:mm"));
                TimeSpan endTime = TimeSpan.Parse(DateTime.Parse(EndTime).ToString("HH:mm"));
                TimeSpan timeToCompare = TimeSpan.Parse(DateTime.Parse(time).ToString("HH:mm"));

                if ((timeToCompare >= startTime) && (timeToCompare < endTime))
                {
                    return true;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Cannot parse {time}.");
                return false;
            }

            return false;
        }
    }
}
