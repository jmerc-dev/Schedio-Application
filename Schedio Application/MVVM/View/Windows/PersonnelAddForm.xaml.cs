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

        public Person Person
        {
            get { return _person; }
        }

        public PersonnelAddForm()
        {
            InitializeComponent();
        }

        private void tb_Name_GotKeyboardFocus(object sender, KeyboardEventArgs e)
        {
            if (tb_Name.Text.Equals("Add Name"))
            {
                tb_Name.Text = string.Empty;
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
                tb_Name.Text = "Add Name";
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void tb_Name_GotKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {

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
            PersonnelCustomTime form = new PersonnelCustomTime(availableDays);
            form.ShowInTaskbar = false;
            form.Owner = Application.Current.MainWindow;
            form.ShowDialog();
            this.Opacity = 1;
        }


        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            _person = new Person(tb_Name.Text, (bool)btn_TimeframeSetter.IsChecked);

            if (tb_Name.Text.Equals("Add Name"))
            {
                MessageBox.Show("Please enter your name.");
                return;
            }

            foreach (CheckBox checkbox in wp_Days.Children)
            {
                try
                {
                    DayOfWeek day = Enum.Parse<DayOfWeek>(checkbox.Content.ToString());
                    _person.SetAvailableDay(day,(bool) checkbox.IsChecked);
                } catch
                {
                    MessageBox.Show("Failed to parse checkbox content to WeekDays");
                }
            }
            Trace.WriteLine(_person.IsConstant);
            if (_person.IsConstant)
            {
                _person.SetConstantTimeframe(new TimeFrame(ti_TimeStart.Time, ti_TimeEnd.Time));
            }
            else
            {
                // TODO: Set custom schedule
            }

            DialogResult = true;
        }
    }
}
