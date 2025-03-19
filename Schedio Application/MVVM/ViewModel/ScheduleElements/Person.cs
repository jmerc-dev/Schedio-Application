 using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Person : PropertyNotification
    {
        const int MAX_CAPACITY = 7;
        private static int _IdCounter = 0;

        private string _Name;
        private int _ID;
        private Day[] _Days;
        private bool _IsConstant;

        private string _ConstTime_Start;
        private string _ConstTime_End;

        public int ID
        {
            get => _ID;
        }

        public static int IdCounter
        {
            get => _IdCounter;
        }

        public string Timeframe
        {
            get { return ConstTime_Start + " -> " + ConstTime_End; }
        }

        public string ConstTime_Start
        {
            get { return _ConstTime_Start; }
            set 
            { 
                _ConstTime_Start = value;
                OnPropertyChanged();
            }
        }

        public string ConstTime_End
        {
            get { return _ConstTime_End; }
            set 
            { 
                _ConstTime_End = value;
                OnPropertyChanged();
            }
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
            set 
            { 
                _IsConstant = value;
                OnPropertyChanged();
            }
        }

        private string _AvailableDays;
        public string AvailableDays
        {
            get { return _AvailableDays; }
            set 
            { 
                _AvailableDays = value;
                OnPropertyChanged();
            }
        }

        public Person() 
        {
            _Days = new Day[MAX_CAPACITY];
            this._ID = Interlocked.Increment(ref _IdCounter);
            PopulateDays();
            SetDaysName();
        }

        // Create a constructor for loading from files
        public Person(string name, Day[] days, bool isConstant, int id)
        {
            this._Name = name;
            this._ID = id;
            if (days.Length != 7)
            {
                throw new Exception("Invalid day array object");
            }
            this.IsConstant = isConstant;
            this._Days = days;
            UpdateFormattedDays();
        }

        // May proc StringFormatException
        public bool IsAvailableAt(DayOfWeek day,TimeFrame tf)
        {
            
            foreach (Day personDays in Days)
            {
                // Checks for day availability
                if (personDays.Name == day && personDays.IsAvailable)
                {
                    // Checks for custom timeframe/constant timeframe
                    if (IsConstant)
                    {
                        TimeFrame personAvailableTf = new TimeFrame(ConstTime_Start, ConstTime_End);
                        if (personAvailableTf.CanContain(tf))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        foreach (TimeFrame personTimeframe in personDays.CustomTimeframe)
                        {
                            if (personTimeframe.CanContain(tf))
                            {
                                return true;
                            }
                        }
                        return false;
                    }

                }
            }
            return false;
        }

        private void PopulateDays()
        {
            for (int i = 0; i < MAX_CAPACITY; i++)
            {
                _Days[i] = new Day();
            }
        }

        public void UpdateFormattedDays()
        {
            AvailableDays = GetAvailDaysFormatted();
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
            if ((int) day == 0)
                _Days[6].IsAvailable = isChecked;
            else
                _Days[(int)day - 1].IsAvailable = isChecked;
            AvailableDays = GetAvailDaysFormatted();
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
