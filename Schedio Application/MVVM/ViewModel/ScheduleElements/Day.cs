using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Day
    {
        

        // TODO:

        /*  - Get the days checked by the user
         *  - Convert it into a collection of strings {Monday, Tuesday, Wednesday...}
         *  - Convert it into abbreviation {Mon, Tue, Wed}
         *  - Make it one string "Mon, Tue, Wed"
         */
        private DayOfWeek _name;
        private bool _IsAvailable;
        private TimeFrame _ConstantTimeframe;
        private List<TimeFrame> _CustomTimeframe;

        // TODO: Add setters/getters for custom timeframe list

        public string Timeframe
        {
            get 
            { 
                return (_ConstantTimeframe.StartTime.ToString("hh:mm tt") + " - " + _ConstantTimeframe.EndTime.ToString("hh:mm tt")).ToUpper();
            }
        }

        public DayOfWeek Name
        { 
            get {  return _name; } 
            set {  _name = value; }
        }

        public List<TimeFrame> CustomTimeframe
        {
            get { return this._CustomTimeframe; }
        }

        public bool IsAvailable
        {
            get { return _IsAvailable; }
            set { _IsAvailable = value; }
        }

        public TimeFrame ConstantTimeframe
        {
            get { return _ConstantTimeframe; }
            set { _ConstantTimeframe = value; }
        }

        public Day()
        {
            _CustomTimeframe = new List<TimeFrame>();
        }

        // Custom time frame methods
        public bool AddTimeFrame(TimeFrame newTime)
        {
            _CustomTimeframe.Add(newTime);
            return true;
        }

        public bool DeleteTimeFrame(TimeFrame newTime)
        {
            try
            {
                _CustomTimeframe.Remove(newTime);
                return true;
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public bool ClearCustomTimeframe()
        {
            _CustomTimeframe.Clear();
            return true;
        }

        public static readonly IReadOnlyDictionary<string, string> DaysAcronym = new Dictionary<string, string> 
        {
            { "Monday", "M"},
            { "Tuesday", "T" },
            { "Wednesday", "W" },
            { "Thursday", "Th" },
            { "Friday", "F" },
            { "Saturday", "S" },
            { "Sunday", "Su" }
        };

        public string GetAbbrevName()
        {
            return DaysAcronym[this.Name.ToString()];
        }

        public bool AddCustomTimeframe(TimeFrame timeFrame)
        {
            _CustomTimeframe.Add(timeFrame);
            return true;
        }

    }
}
