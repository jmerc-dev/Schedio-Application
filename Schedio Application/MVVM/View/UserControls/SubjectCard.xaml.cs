using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for SubjectCard.xaml
    /// </summary>
    public partial class SubjectCard : UserControl
    {
        private double _Height;

        private SubjectEntry _Entry;

        public SubjectEntry Entry
        {
            get { return _Entry; }
            set { _Entry = value; }
        }

        public SubjectCard(SubjectEntry entry)
        {
            InitializeComponent();
            _Entry = entry;
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.Height < 100)
            {
                _Height = this.Height;
                this.Height = 100;
                this.SetValue(Canvas.ZIndexProperty, 100);
            }
            else
            {
                _Height = this.Height;
            }
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Height = _Height;
            this.SetValue(Canvas.ZIndexProperty, 1);
        }
    }
}
