using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    internal class Time
    {

        // For time data
        private DateTime time;

        // For time span data
        

        // Regex Time
        Regex timeFormat = new Regex(@"^[0-1][0-9]:[0-6][0-9]\s[A|P]M$");
        public Time(string time)
        {
            if (!timeFormat.Match(time).Success)
            {
                return;
            }

            if (!DateTime.TryParse(time, out this.time))
            {
                MessageBox.Show("Failed to parse Time");
                return;
            }
        }

    }
}
