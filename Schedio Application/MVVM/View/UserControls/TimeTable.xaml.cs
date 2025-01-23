using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using static System.Collections.Specialized.BitVector32;

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for TimeTable.xaml
    /// </summary>
    /// 

    public enum DataAction
    {
        Add,
        Delete,
        Update
    }

    public partial class TimeTable : UserControl
    {
        private const int ROOMHEADER_WIDTH = 200;
        private const int ROOMHEADER_HORIZONTAL_OFFSET = 5;
        private const int SUBJECTCARD_WIDTH = 200;
        private const double HOUR_HEIGHT = 120;

        private long entryIndex = 0;

        public static readonly DependencyProperty _Rooms = DependencyProperty.Register(
            "Rooms",
            typeof(ObservableCollection<Room>),
            typeof(TimeTable),
            new PropertyMetadata(null));

        public ObservableCollection<Room> Rooms
        {
            get { return (ObservableCollection<Room>) GetValue(_Rooms); }
            set { SetValue(_Rooms, value); }
        }

        public TimeTable()
        {
            InitializeComponent();
            //Rooms.Add(new Room("101", new RoomType("Lab")));
            //Rooms.Add(new Room("102", new RoomType("Lab")));
            //Rooms.Add(new Room("103", new RoomType("Classic")));
            //Rooms.Add(new Room("104", new RoomType("Lab")));
            //Rooms.Add(new Room("101", new RoomType("Lab")));
            //Rooms.Add(new Room("102", new RoomType("Lab")));
            //Rooms.Add(new Room("103", new RoomType("Classic")));
            //Rooms.Add(new Room("104", new RoomType("Lab")));
            //Rooms.Add(new Room("101", new RoomType("Lab")));
            //Rooms.Add(new Room("102", new RoomType("Lab")));
            //Rooms.Add(new Room("103", new RoomType("Classic")));
            //Rooms.Add(new Room("104", new RoomType("Lab")));
            
            PopulateTimeslot();

            Loaded += (sender, e) =>
            {
                lv_RoomHeader.ItemsSource = Rooms;
                

                double fullWidth = TimeHeader.ActualWidth + RoomHeader.ActualWidth;
                double fullHeight = TimeHeader.ActualHeight+ Timeslot.ActualHeight;

                SetControlWidth(fullWidth);
                SetControlHeight(fullHeight);

                
            };
        }

        private void Entries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) 
        { 
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null && e.NewItems.Count == 1)
                {
                    // add control
                    if (e.NewItems[0] != null)
                    {
                        SubjectEntry newEntry = (SubjectEntry)e.NewItems[0];

                        SubjectCard newSubCard = new SubjectCard(newEntry);
                        entriesContainer.Children.Add(newSubCard);
                    }
                }
            }
        }

        public bool addEntry(SubjectEntry entry)
        {
            entriesContainer.Children.Add(new SubjectCard(entry));
            return true;
        }

        public bool removeEntry(SubjectEntry entry)
        {
            //foreach (SubjectCard sc in entriesContainer.Children)
            //{
            //    if (sc.Entry == entry)
            //    {
            //        entriesContainer.Children.Remove(sc);
            //    }
            //}

            //Trace.WriteLine(entriesContainer.Children[1]);
            for (int i = 0; i < entriesContainer.Children.Count; i++)
            {
                if (entriesContainer.Children[i].GetType() == typeof(SubjectCard))
                {
                    SubjectCard sc = (SubjectCard) entriesContainer.Children[i];
                    if (sc.Entry == entry)
                    {
                        entriesContainer.Children.Remove(sc);
                    }
                }
            }

            return true;
        }

        public void addVerticalLine()
        {
            Rectangle rect = new Rectangle();
            rect.SetValue(Canvas.LeftProperty, (double) SUBJECTCARD_WIDTH * Rooms.Count);
            VerticalLineContainer.Children.Add(rect);
        }

        public void removeVerticalLine()
        {
            VerticalLineContainer.Children.RemoveAt(VerticalLineContainer.Children.Count - 1);
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
                //sp_RoomHeader.Children.Add(new TextBlock { Text = room.Name });
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
            
        }

        private void AddSubjectCard(SubjectEntry entry) 
        {
            Subject.SubjectEntries.Add(entry);
            entriesContainer.Children.Add(new SubjectCard(entry));
        }

        private void DeleteSubjectCard(SubjectEntry entry)
        {

        }

        // Mouse dragging 
        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vOff = 1;
        private void scrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            scrollMousePoint = e.GetPosition(sv_Canvas);
            hOff = sv_Canvas.HorizontalOffset;
            vOff = sv_Canvas.VerticalOffset;

            sv_Canvas.CaptureMouse();
        }

        private void scrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sv_Canvas.IsMouseCaptured)
            {
                sv_Canvas.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(sv_Canvas).X));
                sv_Canvas.ScrollToVerticalOffset(vOff + (scrollMousePoint.Y - e.GetPosition(sv_Canvas).Y));
            }
        }

        private void scrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            sv_Canvas.ReleaseMouseCapture();
        }

        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            sv_Canvas.ScrollToVerticalOffset(sv_Canvas.VerticalOffset + -e.Delta);
        }

        
    }
}
