using Schedio_Application.MVVM.ViewModel.ScheduleElements;
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

        public TimeframeOverlapException(TimeFrame source, TimeFrame end) :
            base($"Overlapping Timeframes:\n{source.StartTime} -> {source.EndTime}\n{end.StartTime} -> {end.EndTime}")
        {
            
        }

        public TimeframeOverlapException(TimeFrame source, TimeFrame end, string day) :
            base($"Overlapping Timeframes in [{day}]:\n{source.StartTime} -> {source.EndTime}\n{end.StartTime} -> {end.EndTime}\n")
        {

        }

        public TimeframeOverlapException(SubjectEntry mainEntry, SubjectEntry existingEntry) :
            base($"You cannot allocate this subject because it is conflicting with:\n{existingEntry.SubjectInfo.OwnerSection.Name}: {existingEntry.TimeFrame.StartTime} => {existingEntry.TimeFrame.EndTime} in {existingEntry.DayAssigned.ToString()}")
        {

        }
    }
}
