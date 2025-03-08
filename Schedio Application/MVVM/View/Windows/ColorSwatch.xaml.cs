using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for ColorSwatch.xaml
    /// </summary>
    public partial class ColorSwatch : Window
    {
        private ClassSection section;
        public ColorSwatch(ClassSection section)
        {
            InitializeComponent();
            this.section = section;
            this.DataContext = this.section;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Trace.WriteLine($"{_DynamicColor.HSL.Hue}");
        }

        private void StandardColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            //section.SectionColor = colorPicker.SelectedColor;
            //Trace.WriteLine(section.SectionColor.ToString());
        }
    }
}
