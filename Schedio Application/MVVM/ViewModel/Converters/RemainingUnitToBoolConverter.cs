using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Schedio_Application.MVVM.ViewModel.Converters
{
    public class RemainingUnitToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Trace.WriteLine($"{value.ToString()}");
            return string.Empty;
            //double doubleValue = double.Parse(value.ToString());
            //double compareToValue = double.Parse(parameter.ToString());

            //return doubleValue <= compareToValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
