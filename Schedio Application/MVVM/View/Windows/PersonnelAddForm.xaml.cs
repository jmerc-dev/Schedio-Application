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
using System.Collections.ObjectModel;

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for PersonnelAddForm.xaml
    /// </summary>
    public partial class PersonnelAddForm : Window
    {

        private Person _person;
        private PersonnelCustomTime form;
        private ObservableCollection<Person> _people;
        public Dictionary<DayOfWeek, List<TimeFrame>> dailyTimeframe;

        // Properties

        public Person Person
        {
            get { return _person; }
            set { _person = value; }
        }

        public PersonnelAddForm(Person person, ObservableCollection<Person> people)
        {
            _person = person;
            _people = people;
            InitializeComponent();

            this.DataContext = this;
            this.Owner = Application.Current.MainWindow;
            tb_Name.Focus();

            // Update
            Loaded += (sender, e) =>
            {
                if (person.Name != null)
                {
                    // populate
                    UpdateName(person.Name);
                    UpdateIsConstant(person.IsConstant);
                    UpdateDayAvailable(person.Days);
                    if (person.IsConstant) 
                    {
                        UpdateConstTimeframe(person.ConstTime_Start, person.ConstTime_End);
                    }
                    else
                    {
                        UpdateCustomTime();
                    }
                }
            };
            
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
                if ((bool)btn_TimeframeSetter.IsChecked)
                {
                    

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

        private bool IsNameExists(string name)
        {
            foreach (Person person in _people)
            {
                if (person.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
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
            string oldName = _person.Name;
            string newName = tb_Name.Text;

            if (oldName == null)
            {
                if (IsNameExists(newName))
                {
                    new MBox("Name already exists.").ShowDialog();
                    return;
                }
            }
            else
            {
                if (!oldName.Equals(newName, StringComparison.CurrentCultureIgnoreCase) && IsNameExists(newName))
                {
                    new MBox("Name already exists.").ShowDialog();
                    return;
                }
            }

            if (!DaySelectExists())
            {
                new MBox("No selected day").ShowDialog();
                return;
            }

            

            // Set name
            _person.Name = tb_Name.Text;

            // Set schedule
            bool ConstantTf;
            if (btn_TimeframeSetter.IsChecked != null)
            {
                ConstantTf = (bool) btn_TimeframeSetter.IsChecked;
            }
            else
            {
                new MBox("isConstant is null").ShowDialog();
                return;
            }

            if (ConstantTf)
            {
                if (!SetConstantTimeframe())
                {
                    return;
                }
            }
            else
            {
                if (!SetCustomTimeframe())
                {
                    return;
                }
            }
            // Assign available days
            SetAvailableDays();
            DialogResult = true;
        }

        private bool SetConstantTimeframe()
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
                return false;
            }

            _person.IsConstant = true;
            _person.ConstTime_Start = tf.StartTime;
            _person.ConstTime_End = tf.EndTime;
            return true;
        }

        private bool SetCustomTimeframe()
        {
            // add custom timeframe
            _person.IsConstant = false;

            if (dailyTimeframe == null)
            {
                new MBox("Please setup the custom timeframe first.").ShowDialog();
                return false;
            }

            // Check if available days are populated with timeframe/s
            foreach (CheckBox cb in wp_Days.Children)
            {
                if (cb.IsChecked == true)
                {
                    DayOfWeek day;
                    if (!Enum.TryParse(cb.Content.ToString(), out day))
                    {
                        new MBox($"Unable to parse {cb.Content}").ShowDialog();
                        return false;
                    }
                    if (dailyTimeframe[day] == null || dailyTimeframe[day].Count == 0)
                    {
                        new MBox($"Day: {cb.Content} is empty").ShowDialog();
                        return false;
                    }
                }
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
            return true;
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

            _person.UpdateFormattedDays();
        }

        // Update Methods

        private void UpdateName(string name)
        {
            tb_Name.Text = name;
        }

        private void UpdateIsConstant(bool constanttf)
        {
            btn_TimeframeSetter.IsChecked = constanttf;
        }

        private void UpdateDayAvailable(Day[] days)
        {
            foreach(CheckBox cb in wp_Days.Children)
            {
                string name = cb.Content.ToString();

                for (int i = 0; i < days.Length; i++)
                {
                    if (days[i].Name.ToString().Equals(name))
                    {
                        cb.IsChecked = days[i].IsAvailable;
                        break;
                    }
                }

                
            }
        }

        private void UpdateConstTimeframe(string start, string end)
        {
            ti_TimeStart.SetTime(start);
            ti_TimeEnd.SetTime(end);
        }

        private void UpdateCustomTime()
        {
            dailyTimeframe = new Dictionary<DayOfWeek, List<TimeFrame>>();
            for (int i = 0; i < _person.Days.Length; i++)
            {
                dailyTimeframe.Add(_person.Days[i].Name, new List<TimeFrame>(_person.Days[i].CustomTimeframe));
            }
        }

        private void img_ArrowRight_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Image img = (Image)sender;
            
            if (img.IsEnabled)
            {
                img_ArrowRight.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/arrow-right.png"));
            }
            else
            {
                img_ArrowRight.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/arrow-right-disabled.png"));
            }
        }

        private void btn_TimeframeSetter_Checked(object sender, RoutedEventArgs e)
        {
            btn_CustomTime.IsEnabled = false;
        }

        private void btn_TimeframeSetter_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_CustomTime.IsEnabled = true;
        }
    }
}
