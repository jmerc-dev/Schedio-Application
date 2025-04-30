using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{
    public class EntryCategoryCounter
    {
        private readonly IScheduleElement _value;
        private int _count;

        public int Count 
        { 
            get => _count;
        }

        public string? Name
        {
            get => _value.Name;
        }

        public EntryCategoryCounter(IScheduleElement value, int count)
        {
            _value = value;
            _count = count;
        }

    }
}
