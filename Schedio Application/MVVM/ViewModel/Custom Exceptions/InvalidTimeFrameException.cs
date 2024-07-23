using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Custom_Exceptions
{
    public class InvalidTimeFrameException : Exception
    {
        public InvalidTimeFrameException() { }

        public InvalidTimeFrameException(string startTime, string endTime) :
            base($"{startTime} -> {endTime} is an invalid timeframe.")
        {

        }

        
    }
    
}
