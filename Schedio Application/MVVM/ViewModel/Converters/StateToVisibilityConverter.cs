using Schedio_Application.MVVM.View.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Schedio_Application.MVVM.ViewModel.Converters
{
    public class StateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            State stateValue = (State)value;
            if (stateValue == State.Existing)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
