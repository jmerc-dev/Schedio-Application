using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Converter_Contexts
{
    public class RoomsConverterContext
    {
        public Dictionary<int, Room>? RoomsMap { get; set; }
    }
}
