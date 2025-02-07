using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Custom_Exceptions
{
    public class PersonnelOverlapException : Exception
    {
        public PersonnelOverlapException()
        {

        }

        public PersonnelOverlapException(SubjectEntry mainEntry, SubjectEntry existingEntry) :
            base($"{mainEntry.SubjectInfo.AssignedPerson.Name} is currently assigned at the timeframe: {existingEntry.TimeFrame.StartTime} => {existingEntry.TimeFrame.EndTime} in {existingEntry.DayAssigned.ToString()}")
        {

        }

    }
}
