using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class CultureDayOfWeek
    {
        /// <summary>
        /// In this method, parameter 'n' only accepts values from 0 to 6, representing Day of Week.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static DayOfWeek Convert(int n)
        {
            switch (n)
            {
                case 0:
                    return DayOfWeek.Monday;
                case 1:
                    return DayOfWeek.Tuesday;
                case 2:
                    return DayOfWeek.Wednesday;
                case 3:
                    return DayOfWeek.Thursday;
                case 4:
                    return DayOfWeek.Friday;
                case 5:
                    return DayOfWeek.Saturday;
                case 6:
                    return DayOfWeek.Sunday;
                default:
                    throw new ArgumentOutOfRangeException("int value must only range from 0 - 6");
            }
        }
    }
}
