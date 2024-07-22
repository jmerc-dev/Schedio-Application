using Schedio_Application.MVVM.ViewModel.Custom_Exceptions;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections;
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
    /// Interaction logic for PersonnelAddForm.xaml
    /// </summary>
    public partial class PersonnelAddForm : Window
    {

        private Person _person;
        private PersonnelCustomTime form;
        public Dictionary<DayOfWeek, List<TimeFrame>> dailyTimeframe;

        // Properties
        public string PersonName
        { 
            get { return _person.Name; } 
            set { _person.Name = value; } 
        }

        public Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public bool IsConstant
        {
            get { return _person.IsConstant; }
            set { _person.IsConstant = value; }
        }

        public PersonnelAddForm(Person person)
        {
            _person = person;
            InitializeComponent();

            this.DataContext = this;
            this.Owner = Application.Current.MainWindow;
            this.ShowInTaskbar = false;

            // Setting datacontexts
            wp_Days.DataContext = _person;
            sp_ConstTimeFrame.DataContext = _person;

            tb_Name.Focus();
        }

        private void tb_Name_GotKeyboardFocus(object sender, KeyboardEventArgs e)
        {
            if (tb_Name.Text.Equals("Add Name"))
            {
                //tb_Name.Text = string.Empty;
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tb_Name_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (tb_Name.Text.Equals(string.Empty))
            {
                //PersonName = "Add Name";
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void btn_TimeframeSetter_Click(object sender, RoutedEventArgs e)
        {
            if (btn_TimeframeSetter.IsChecked != null)
            {
                if ((bool) btn_TimeframeSetter.IsChecked)
                {
                    btn_CustomTime.IsEnabled = false;
                    sp_ConstTimeFrame.IsEnabled = true;

                }
                else
                {
                    btn_CustomTime.IsEnabled = true;
                    sp_ConstTimeFrame.IsEnabled = false;
                }
            }
        }

        private void chb_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox cb_Day in wp_Days.Children)
            {
                cb_Day.IsChecked = chb_SelectAll.IsChecked;
            }
        }

        private void chb_Day_Unchecked(object sender, RoutedEventArgs e)
        {
            chb_SelectAll.IsChecked = false;
        }

        private void chb_Day_Checked(object sender, RoutedEventArgs e)
        {
            if (chb_SelectAll.IsChecked == false)
            {
                bool AllChecked = true;

                foreach (CheckBox cb_Day in wp_Days.Children)
                {
                    if (cb_Day.IsChecked == false)
                    {
                        AllChecked = false;
                        break;
                    }
                }

                chb_SelectAll.IsChecked = AllChecked;
            }
            
        }

        private void btn_CustomTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dictionary<string, bool> availableDays = new Dictionary<string, bool>();

            foreach (CheckBox checkbox in wp_Days.Children)
            {
                if (checkbox.IsChecked == true)
                {
                    availableDays.Add(checkbox.Content.ToString(), true);
                }
                else
                {
                    availableDays.Add(checkbox.Content.ToString(), false);
                }
            }

            this.Opacity = 0;

            if (dailyTimeframe == null)
            {
                form = new PersonnelCustomTime(availableDays);
            }
            else
            {
                form = new PersonnelCustomTime(availableDays, dailyTimeframe);
            }
            
            form.ShowInTaskbar = false;
            form.Owner = Application.Current.MainWindow;
            if (form.ShowDialog() == true)
            {
                dailyTimeframe = form.dailyTimeframe;
            }

            this.Opacity = 1;
        }


        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            
            // Assign available days
            SetAvailableDays();

            // Set name
            _person.Name = tb_Name.Text;
            // Set schedule
            if (_person.IsConstant)
            {
                _person.IsConstant = true;
                _person.ConstTime_Start = ti_TimeStart.Time;
                _person.ConstTime_End = ti_TimeEnd.Time;
            }
            else
            {
                // add custom timeframe
                _person.IsConstant = false;

                if (dailyTimeframe == null)
                {
                    MessageBox.Show("Please setup the custom timeframe first.");
                    return;
                }

                foreach (KeyValuePair<DayOfWeek, List<TimeFrame>> keyValuePair in dailyTimeframe)
                {
                    // Locate where to put items
                    for (int i = 0; i < _person.Days.Length; i++)
                    {
                        if (keyValuePair.Key == _person.Days[i].Name)
                        {
                            _person.Days[i].CustomTimeframe = keyValuePair.Value;
                        }
                    }
                }
            }

            for (int i = 0; i < _person.Days.Length; i++)
            {
                Trace.WriteLine(_person.Days[i].Name.ToString() + " : " + _person.Days[i].IsAvailable);
                if (_person.Days[i].IsAvailable)
                {
                    foreach(TimeFrame tf in _person.Days[i].CustomTimeframe)
                    {
                        Trace.WriteLine(tf.StartTime, tf.EndTime);
                    }
                }
            }
            DialogResult = true;
        }

        private void SetAvailableDays()
        {
            int index = 0;
            foreach (CheckBox checkbox in wp_Days.Children) 
            {
                if (checkbox.Content.ToString().Equals(_person.Days[index].Name.ToString()))
                {

                    if (checkbox.IsChecked != null)
                    {
                        _person.Days[index].IsAvailable = (bool) checkbox.IsChecked;
                    }
                }
                index++;
            }
        }
    }
}
