﻿using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Converter_Contexts
{
    public class SectionsConverterContext : IConverterContext
    {
        public Dictionary<int, ClassSection>? ClassSectionMap { get; set; }
    }
}
