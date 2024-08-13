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
        private ObservableCollection<Room> Rooms;

        public TimeTable()
        {
            InitializeComponent();
            Rooms = new ObservableCollection<Room>();

            Rooms.Add(new Room("101", new RoomType("Lab")));
            Rooms.Add(new Room("102", new RoomType("Lab")));
            Rooms.Add(new Room("103", new RoomType("Classic")));
            Rooms.Add(new Room("104", new RoomType("Lab")));
            Rooms.Add(new Room("101", new RoomType("Lab")));
            Rooms.Add(new Room("102", new RoomType("Lab")));
            Rooms.Add(new Room("103", new RoomType("Classic")));
            Rooms.Add(new Room("104", new RoomType("Lab")));
            Rooms.Add(new Room("101", new RoomType("Lab")));
            Rooms.Add(new Room("102", new RoomType("Lab")));
            Rooms.Add(new Room("103", new RoomType("Classic")));
            Rooms.Add(new Room("104", new RoomType("Lab")));
            PopulateRooms(Rooms);
            PopulateTimeslot();

            Loaded += (sender, e) =>
            {
                double fullWidth = TimeHeader.ActualWidth + RoomHeader.ActualWidth;
                double fullHeight = TimeHeader.ActualHeight+ Timeslot.ActualHeight;

                SetControlWidth(fullWidth);
                SetControlHeight(fullHeight);
            };
        }

        private void SetControlWidth(double width)
        {
            cv_Container.Width = width;
        }

        private void SetControlHeight(double height)
        {
            cv_Container.Height = height;
        } 

        private void PopulateRooms(ObservableCollection<Room> rooms)
        {
            foreach (Room room in rooms)
            {
                sp_RoomHeader.Children.Add(new TextBlock { Text = room.Name });
            }
        }

        private void PopulateTimeslot()
        {
            foreach (string t in BaseSchedule.TimeBreakpoints)
            {
                sp_TimeHeader.Children.Add(new TextBlock { Text = t });
            }
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
