using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    internal class Person
    {
        const int MAX_CAPACITY = 7;

        private string _Name;
        private Day[] _Days;
        private string _Timespan;
        private bool _IsCustom;

        public string Name
        {
            get { return _Name; } 
            set {  _Name = value; } 
        }

        public string Timespan
        {
            get { return _Timespan; }
            set {; }
        }

        public bool IsCustom
        {
            get { return _IsCustom; }
            set { _IsCustom = value; }
        }

        public Person(string name, bool isCustom) 
        {
            this.Name = name;
            this.IsCustom = isCustom;
            _Days = new Day[MAX_CAPACITY];

            PopulateDays();
            SetDaysName();
            //if (SetTimeSpan())
            //{

            //}
        }

        private bool SetTimeSpan(Day[] days)
        {

            return true;
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
        
        public Day[] GetAvailableDays()
        {
            return _Days;
        }
    }
}
