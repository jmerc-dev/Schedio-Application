using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for StartEndTimeInput.xaml
    /// </summary>
    public partial class StartEndTimeInput : UserControl
    {

        public string StartTime
        {
            get => ti_StartTime.Time;
        }
        public string EndTime {
            get => ti_EndTime.Time;
        }


        public StartEndTimeInput()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public StartEndTimeInput(TimeFrame timeFrame)
        {
            InitializeComponent();
            this.DataContext = this;

            SetStartTime(timeFrame.StartTime);
            SetEndTime(timeFrame.EndTime);
        }

        public bool SetStartTime(string time)
        {
            if (ti_StartTime.SetTime(time))
            {
                return true;
            }
            return false;
        }

        public bool SetEndTime(string time)
        {
            if (ti_EndTime.SetTime(time))
            {
                return true;
            }
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((StackPanel)this.Parent).Children.Remove(this);
        }
    }
}
