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
    public class MBoxTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MBoxType type = ((MBoxType) value);
            switch (type) 
            {
                case MBoxType.ConfirmDelete:
                    return Visibility.Visible;
                default:
                    return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
