using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Person
    {
        const int MAX_CAPACITY = 7;

        private string _Name;
        private Day[] _Days;
        private bool _IsConstant;

        public string Name
        {
            get { return _Name; } 
            set {  _Name = value; } 
        }

        public string Timeframe
        {
            get
            {
                for (int i = 0; i < MAX_CAPACITY; i++)
                {
                    if (_Days[i].IsAvailable)
                    {
                        return _Days[i].Timeframe;
                    }
                }
                return "None";
            }
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

        public Person(string name, bool isCustom) 
        {
            this.Name = name;
            this.IsConstant = isCustom;
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

        public bool SetConstantTimeframe(TimeFrame time)
        {
            for (int i = 0; i < MAX_CAPACITY; i++)
            {
                if (_Days[i].IsAvailable)
                {
                    _Days[i].ConstantTimeframe = time;
                }
            }
            return true;
        }
        
        public Day[] GetAvailableDays()
        {
            return _Days;
        }
    }
}
