using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public interface IScheduleElement
    {
        public string? Name { get; set; }
        public int ID { get; set; }

        // Still have issues with implementing Day class with this
    }
}
