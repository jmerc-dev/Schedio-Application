using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Custom_Exceptions
{
    public class TimeframeOverlapException : Exception
    {
        public TimeframeOverlapException()
        {
            
        }

        public TimeframeOverlapException(string time, string startTime, string endTime) :
            base($"Time :{time} overlaps the Timeframe: {startTime} -> {endTime}")
        {

        }
    }
}
