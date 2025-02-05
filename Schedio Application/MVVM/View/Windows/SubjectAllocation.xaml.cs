﻿using Microsoft.VisualBasic;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Schedio_Application.MVVM.View.Windows
{

    /// <summary>
    /// Interaction logic for SubjectAllocation.xaml
    /// </summary>
    public partial class SubjectAllocation : Window
    {
        private SubjectEntry _entry;
        private List<string> _UnitsChoices;

        public List<string> UnitsChoices 
        {
            get => _UnitsChoices;
            set => _UnitsChoices = value;
        }

        public SubjectEntry Entry
        {
            get { return _entry; }
        }

        private string[] _DaysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

        public string[] DaysOfAWeek
        {
            get => _DaysOfWeek;
        }
        public SubjectAllocation(Subject subject, State mode)
        {
            InitializeComponent();

            //cb_Day.ItemsSource = DaysOfAWeek;
            _entry = new SubjectEntry(subject);
            cbox_Rooms.ItemsSource = Workshop.Rooms;
            this.DataContext = _entry;

            cbox_SelectedUnits.ItemsSource = AddChoices(subject.UnitsRemaining);

        }

        // Continue Here
        public SubjectAllocation(SubjectEntry subEntry)
        {
            InitializeComponent();

            _entry = subEntry;
            cbox_Rooms.ItemsSource = Workshop.Rooms;
            this.DataContext = _entry;
            cbox_SelectedUnits.ItemsSource = AddChoices(subEntry.SubjectInfo.UnitsRemaining + subEntry.UnitsToAllocate);

            btn_Update.Visibility = Visibility.Visible;
            btn_Select.Visibility = Visibility.Collapsed;
            SetupValues();
        }

        private void SetupValues()
        {
            // DayAssigned
            foreach (ComboBoxItem item in cb_Day.Items)
            {
                if (item.Content.ToString().Equals(Entry.DayAssigned.ToString()))
                {
                    cb_Day.SelectedItem = item;
                }
            }

            // StartTime
            
        }

        private List<double> AddChoices(double limit)
        {
            List<double> unitsChoices = new List<double>();
            for (double i = 0.5; i <= 5.0; i += 0.5 )
            {
                if (i > limit)
                    break;
                unitsChoices.Add(i);
            }

            return unitsChoices;
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

            if (Entry.SubjectInfo.UnitsRemaining < Double.Parse(cbox_SelectedUnits.Text))
            {
                new MBox("The selected units to allocate is greater than the units remaining.").ShowDialog();
                return;
            }

            try
            {

                _entry.UnitsToAllocate = Double.Parse(cbox_SelectedUnits.Text);
                _entry.TimeFrame.StartTime = ti_Start.Time;
                _entry.TimeFrame.EndTime = DateTime.Parse(ti_Start.Time).AddHours(_entry.UnitsToAllocate).ToString("hh:mm tt");
                _entry.DayAssigned = GetChosenDay(cb_Day.Text);

                Room? room = RoomExists(cbox_Rooms.Text);
                _entry.RoomAllocated = room;

                // Validations
                if (!Entry.SubjectInfo.AssignedPerson.IsAvailableAt(GetChosenDay(cb_Day.Text), new TimeFrame(Entry.TimeFrame.StartTime, Entry.TimeFrame.EndTime)))
                {
                    if (new MBox($"{Entry.SubjectInfo.AssignedPerson.Name} is not available from {Entry.TimeFrame.StartTime} to {Entry.TimeFrame.EndTime} in {Entry.DayAssigned.ToString()}. Do you still want to continue?", MBoxType.CancelOrOK).ShowDialog() != true)
                    {
                        return;
                    }
                }

                if (room == null)
                {
                    new MBox("Room doesn't exist").ShowDialog();
                    return;
                }

                foreach (SubjectEntry entry in Subject.subjectEntries)
                {
                    if ((Entry.DayAssigned == entry.DayAssigned) && (Entry.RoomAllocated == entry.RoomAllocated))
                    {
                        // Null Checking
                        if (Entry.TimeFrame.StartTime == null || Entry.TimeFrame.EndTime == null)
                        {
                            new MBox("StartTime or EndTime is null.").ShowDialog();
                            return;
                        }
                        // Timeframe already occupied by other subjects
                        if (entry.TimeFrame.WillConcurWith(new TimeFrame(Entry.TimeFrame.StartTime, Entry.TimeFrame.EndTime)))
                        {
                            new MBox($"You cannot allocate this subject because it is conflicting with:\n{entry.SubjectInfo.OwnerSection.Name}: {entry.TimeFrame.StartTime} => {entry.TimeFrame.EndTime} in {entry.DayAssigned.ToString()}").ShowDialog();
                            return;
                        }
                    }
                    // Personnel parallel check
                    if (Entry.DayAssigned == entry.DayAssigned && Entry.RoomAllocated != entry.RoomAllocated && Entry.SubjectInfo.AssignedPerson == entry.SubjectInfo.AssignedPerson)
                    {
                        if (entry.TimeFrame.WillConcurWith(Entry.TimeFrame))
                        {
                            new MBox($"{Entry.SubjectInfo.AssignedPerson.Name} is currently assigned at the timeframe: {entry.TimeFrame.StartTime} => {entry.TimeFrame.EndTime} in {entry.DayAssigned.ToString()}").ShowDialog();
                            return;
                        }
                    }
                }

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

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
