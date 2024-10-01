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
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for SubjectAllocation.xaml
    /// </summary>
    public partial class SubjectAllocation : Window
    {
        private SubjectEntry _entry;
        public SubjectEntry Entry
        {
            get { return _entry; }
        }

        public SubjectAllocation(Subject subject)
        {
            InitializeComponent();
            _entry = new SubjectEntry(subject);

            cbox_Rooms.ItemsSource = Workshop.Rooms;
            this.DataContext = _entry;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {

                }
                else
                {
                    this.DragMove();
                }
        }

        // TODO: Fix Subject Card
        // 10-02-2024

        private DayOfWeek GetChosenDay(string dayName)
        {
            DayOfWeek chosenDay;
            if (Enum.TryParse<DayOfWeek>(dayName, out chosenDay))
            {
                return chosenDay;
            }
            else
            {
                throw new FormatException($"Cannot parse {dayName} because it is not on the required format.");
            }
        }

        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {

            // Validation
            if (cb_Day.SelectedValue == null)
            {
                new MBox("Please select a day to be assigned.").ShowDialog();
                return;
            }

            try
            {
                _entry.StartTime = ti_Start.Time;
                _entry.UnitsToAllocate = Double.Parse(cbox_SelectedUnits.Text);
                _entry.DayAssigned = GetChosenDay(cb_Day.Text);

                Room? room = RoomExists(cbox_Rooms.Text);

                if (room == null)
                {
                    new MBox("Room doesn't exist").ShowDialog();
                    return;
                }

                _entry.RoomAllocated = room;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                new MBox(ex.Message, MBoxImage.Warning).ShowDialog();
                return;
            }
        }

        private Room? RoomExists(string roomname)
        {
            foreach (Room room in Workshop.Rooms)
            {
                if (room.Name.Equals(roomname, StringComparison.CurrentCulture))
                {
                    return room;
                }
            }
            return null;
        }
    }
}
