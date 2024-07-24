using Schedio_Application.MVVM.ViewModel.Custom_Exceptions;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.View.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
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
            if (tb_Name.Text.Equals("Name"))
            {
                tb_Name.Text = "";
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
                tb_Name.Text = "Name";
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
                    img_ArrowRight.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/arrow-right.png"));
                }
                else
                {
                    btn_CustomTime.IsEnabled = true;
                    sp_ConstTimeFrame.IsEnabled = false;
                    img_ArrowRight.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/arrow-right-disabled.png"));
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

        private bool DaySelectExists()
        {
            foreach (CheckBox checkbox in wp_Days.Children)
            {
                if (checkbox.IsChecked == true)
                {
                    return true;
                }
            }
            return false;
        }

        private void btn_CustomTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!DaySelectExists())
            {
                new MBox("No selected day").ShowDialog();
                return;
            }
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
        }


        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (!DaySelectExists())
            {
                new MBox("No selected day").ShowDialog();
                return;
            }

            // Assign available days
            SetAvailableDays();

            // Set name
            _person.Name = tb_Name.Text;
            // Set schedule
            if (_person.IsConstant)
            {
                TimeFrame tf;
                // Validate time
                try
                {
                    tf = new TimeFrame(ti_TimeStart.Time, ti_TimeEnd.Time);
                }
                catch (InvalidTimeFrameException ex)
                {
                    new MBox(ex.Message).ShowDialog();
                    return;
                }

                _person.IsConstant = true;
                _person.ConstTime_Start = tf.StartTime;
                _person.ConstTime_End = tf.EndTime;
            }
            else
            {
                // add custom timeframe
                _person.IsConstant = false;

                if (dailyTimeframe == null)
                {
                    new MBox("Please setup the custom timeframe first.").ShowDialog();
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
