using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Person : PropertyNotification
    {
        const int MAX_CAPACITY = 7;

        private string _Name;

        // TODO: Make this a dictionary
        private Day[] _Days;
        private bool _IsConstant;

        private string _ConstTime_Start;
        private string _ConstTime_End;

        public string Timeframe
        {
            get { return ConstTime_Start + " -> " + ConstTime_End; }
        }

        public string ConstTime_Start
        {
            get { return _ConstTime_Start; }
            set { _ConstTime_Start = value; }
        }

        public string ConstTime_End
        {
            get { return _ConstTime_End; }
            set { _ConstTime_End = value; }
        }

        public string Name
        {
            get { return _Name; } 
            set 
            {  
                _Name = value;
                OnPropertyChanged();
            } 
        }

        public Day[] Days
        {
            get { return _Days; }
        }

        public bool IsConstant
        {
            get { return _IsConstant; }
            set { _IsConstant = value; }
        }

        public string AvailableDays
        {
            get { return GetAvailDaysFormatted(); }
        }

        public Person() 
        {
            _Days = new Day[MAX_CAPACITY];

            PopulateDays();
            SetDaysName();
        }


        private void PopulateDays()
        {
            for (int i = 0; i < MAX_CAPACITY; i++)
            {
                _Days[i] = new Day();
            }
        }

        private void SetDaysName()
        {
            _Days[0].Name = DayOfWeek.Monday;
            _Days[1].Name = DayOfWeek.Tuesday;
            _Days[2].Name = DayOfWeek.Wednesday;
            _Days[3].Name = DayOfWeek.Thursday;
            _Days[4].Name = DayOfWeek.Friday;
            _Days[5].Name = DayOfWeek.Saturday;
            _Days[6].Name = DayOfWeek.Sunday;
        }

        public void SetAvailableDay(DayOfWeek day, bool isChecked)
        {
            _Days[(int)day].IsAvailable = isChecked;
        }

        public string GetAvailDaysFormatted()
        {
            string formattedDays = "";
            int start = -1;

            for (int i = 0; i < MAX_CAPACITY; i++)
            {
                if (_Days[i].IsAvailable)
                {
                    if (start == -1)
                    {
                        start = i;
                    }
                }
            }

            for (int i = 0; i < MAX_CAPACITY; i++)
            {
                if (_Days[i].IsAvailable)
                {
                    if (i == start)
                    {
                        formattedDays += _Days[i].GetAbbrevName();
                    }
                    else
                    {
                        formattedDays += ", " + _Days[i].GetAbbrevName();
                    }
                }
            }
            return formattedDays;
        }
    }
}
