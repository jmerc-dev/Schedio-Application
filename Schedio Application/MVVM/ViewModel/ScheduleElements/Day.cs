using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    internal class Day
    {
        

        // TODO:

        /*  - Get the days checked by the user
         *  - Convert it into a collection of strings {Monday, Tuesday, Wednesday...}
         *  - Convert it into abbreviation {Mon, Tue, Wed}
         *  - Make it one string "Mon, Tue, Wed"
         */
        private DayOfWeek _name;
        private bool _IsAvailable;

        public DayOfWeek Name
        { 
            get {  return _name; } 
            set {  _name = value; }
        }

        public bool IsAvailable
        {
            get { return _IsAvailable; }
            set { _IsAvailable = value; }
        }

        public Day()
        {
            
        }

        public static readonly IReadOnlyDictionary<string, string> DaysAcronym = new Dictionary<string, string> 
        {
            { "Monday", "Mon"},
            { "Tuesday", "Tue" },
            { "Wednesday", "Wed" },
            { "Thursday", "Thu" },
            { "Friday", "Fri" },
            { "Saturday", "Sat" },
            { "Sunday", "Sun" }
        };

        public string GetAbbrevName()
        {
            return DaysAcronym[this.Name.ToString()];
        }

    }
}
