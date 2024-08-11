using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
    /// Interaction logic for TimeTable.xaml
    /// </summary>
    public partial class TimeTable : UserControl
    {
        private const int ROOMHEADER_WIDTH = 200;
        private const int SUBJECTCARD_WIDTH = 200;
        private const double HOUR_HEIGHT = 120;

        public TimeTable()
        {
            InitializeComponent();
            
            Loaded += (sender, e) =>
            {
                SetCanvasDimension(TimeHeader.ActualWidth + RoomHeader.ActualWidth, TimeHeader.ActualHeight + Timeslot.ActualHeight);

                double top = 0;
                for (int i = 0; i < 20; i++)
                {
                    SubjectCard sc = new SubjectCard();
                    sc.SetValue(Canvas.TopProperty, top);
                    sc.Height = 120;
                    entriesContainer.Children.Add(sc);
                    top += 120;
                }
            };
        }

        private void SetCanvasDimension(double width, double height)
        {
            cv_Container.Width = width;
            cv_Container.Height = height;
        }

        private void PopulateRooms(ObservableCollection<Room> rooms)
        {

        }

        private void sv_Canvas_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)sender;
            TimeHeader.SetValue(Canvas.TopProperty, sv.VerticalOffset);
            TimeHeader.SetValue(Canvas.LeftProperty, sv.HorizontalOffset);
            Timeslot.SetValue(Canvas.LeftProperty, sv.HorizontalOffset);
            RoomHeader.SetValue(Canvas.TopProperty, sv.VerticalOffset);
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(entriesContainer, cv_FocusHolder);
        }
    }
}
