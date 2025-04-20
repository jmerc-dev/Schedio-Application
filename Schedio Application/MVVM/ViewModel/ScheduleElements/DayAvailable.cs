using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class DayAvailable : PropertyNotification
    {
        private bool _isDayAvailable;

        public bool IsDayChecked
        {
            get;
            set;
        }

        public DayAvailable() { }

    }
}
