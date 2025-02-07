using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Commands;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for SubjectCard.xaml
    /// </summary>
    public partial class SubjectCard : UserControl
    {
        private const double HOUR_HEIGHT = 120.0;
        private int _HorizontalIndex;
        private double _Height;
        private SubjectEntry _Entry;

        public SubjectEntry Entry
        {
            get { return _Entry; }
            set { _Entry = value; }
        }

        public int HorizontalIndex
        {
            get => _HorizontalIndex;
            set
            {
                _HorizontalIndex = value;
            }
        }
        public SubjectCard(SubjectEntry entry)
        {
            InitializeComponent();
            _Entry = entry;
            
            this.DataContext = entry;
            this.Height = entry.UnitsToAllocate * HOUR_HEIGHT;

            this.SetValue(Canvas.TopProperty, HourToTopPositionConverter(entry.TimeFrame.StartTime));
            this.SetValue(Canvas.LeftProperty, Convert.ToDouble(Workshop.Rooms.IndexOf(entry.RoomAllocated) * 200));
        }

        public void UpdatePosition()
        {
            this.SetValue(Canvas.TopProperty, HourToTopPositionConverter(Entry.TimeFrame.StartTime));
            this.SetValue(Canvas.LeftProperty, Convert.ToDouble(Workshop.Rooms.IndexOf(Entry.RoomAllocated) * 200));
        }

        public void UpdateDimension()
        {
            this.Height = Entry.UnitsToAllocate * HOUR_HEIGHT;
        }

        private double HourToTopPositionConverter(string time)
        {
            DateTime startTime = DateTime.Parse(time);
            TimeSpan tp = startTime.Subtract(DateTime.Parse("12:00 AM"));

            //Trace.WriteLine("Time difference: " + tp.TotalMinutes);

            return tp.TotalMinutes * 2;
        }
        
        public bool SetPosition(TimeFrame tf, Room room)
        {
            this.SetValue(Canvas.TopProperty, HourToTopPositionConverter(tf.StartTime));
            this.SetValue(Canvas.LeftProperty, Convert.ToDouble(Workshop.Rooms.IndexOf(room) * 200));
            return true;
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
