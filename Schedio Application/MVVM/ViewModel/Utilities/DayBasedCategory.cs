using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{
    public class DayBasedCategory
    {
        private DayOfWeek _Day;
        private int _Count;

        public string Name 
        { 
            get => _Day.ToString();
        }

        public int Count
        {
            get => _Count;
        }

        public DayBasedCategory(DayOfWeek day, int count) 
        { 
            _Day = day;
            _Count = count;
        }
    }
}
