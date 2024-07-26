﻿using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for TimeScheduleAddForm.xaml
    /// </summary>
    public partial class TimeScheduleAddForm : Window, INotifyPropertyChanged
    {
        private bool _IsConstant;
        private BaseSchedule _Schedule;

        public event PropertyChangedEventHandler? PropertyChanged;
        public BaseSchedule Schedule
        {
            get { return _Schedule;}
        }

        public bool IsConstant
        {
            get { return _IsConstant; }
            set 
            { 
                _IsConstant = value;
                OnPropertyChanged();
            }
        }

        public bool IsCustom
        {
            get { return !_IsConstant; }
        }

        public TimeScheduleAddForm(BaseSchedule baseSchedule)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Owner = Application.Current.MainWindow;

            _Schedule = baseSchedule;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void CheckBox_All_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (Grid container in sp_DaysContainer.Children)
            {
                container.Children.OfType<CheckBox>().ToList().ForEach(checkBox =>
                {
                    if (((CheckBox)sender).IsChecked == true)
                    {
                        checkBox.IsChecked = true;
                    }
                    else
                    {
                        checkBox.IsChecked = false;
                    }
                });
            }
        }

        private void CheckBox_Day_Checked(object sender, RoutedEventArgs e)
        {
            // To not affect constant timeframe mode
            if (chb_ConstantTime.IsChecked == true)
            {
                return;
            }

            ((Grid)((CheckBox)sender).Parent).Children.OfType<StackPanel>().ToList().ForEach((stackPanel) =>
            {
                stackPanel.IsEnabled = true;
            });

            if (chb_All.IsChecked != true)
            {
                try
                {
                    foreach (Grid container in sp_DaysContainer.Children)
                    {
                        if (container.Children.OfType<CheckBox>().FirstOrDefault().IsChecked != true)
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                chb_All.IsChecked = true;
            }
        }

        private void CheckBox_Day_Unchecked(object sender, RoutedEventArgs e)
        {
            // To not affect constant timeframe mode
            if (chb_ConstantTime.IsChecked == true)
            {
                return;
            }

            ((Grid)((CheckBox)sender).Parent).Children.OfType<StackPanel>().ToList().ForEach((stackPanel) =>
            {
                stackPanel.IsEnabled = false;
            });

            chb_All.IsChecked = false;
        }

        private void CheckBox_ConstantTime_Click(object sender, RoutedEventArgs e)
        {
            bool? isNotNull = ((CheckBox)sender).IsChecked;
            bool isConstant;
            if (isNotNull == null)
            {
                MessageBox.Show("Constant timeframe toggle is null.");
                return;
            }
            else
            {
                isConstant = isNotNull.Value;
            }

            // Toggle each day timeframe
            if (isConstant == true)
            {
                foreach (Grid container in sp_DaysContainer.Children)
                {
                    container.Children.OfType<StackPanel>().ToList().ForEach((stackPanel) =>
                    {
                        stackPanel.IsEnabled = false;
                    });
                }
            }
            else
            {
                foreach (Grid container in sp_DaysContainer.Children)
                {
                    CheckBox? chb = container.Children.OfType<CheckBox>().FirstOrDefault();
                    StackPanel? daysContainer;
                    if (chb == null)
                    {
                        new MBox($"An error occured at while closing Constant Time toggle.").ShowDialog();
                        return;
                    }
                    else
                    {
                        if (chb.IsChecked == true)
                        {
                            daysContainer = container.Children.OfType<StackPanel>().FirstOrDefault();
                        }
                        else
                        {
                            daysContainer = null;
                        }
                    }

                    if (daysContainer != null)
                    {
                        daysContainer.IsEnabled = true;
                    }
                }
            }
            
        }

        private bool daySelected()
        {
            foreach (Grid container in sp_DaysContainer.Children)
            {
                CheckBox? cb_Day = container.Children.OfType<CheckBox>().FirstOrDefault();
                if (cb_Day == null)
                {
                    new MBox("cb_Day is null").ShowDialog();
                    return false;
                }

                if (cb_Day.IsChecked == true)
                {
                    return true;
                }
            }
            return false;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<DayOfWeek, TimeFrame>? dailyTimeframe = new Dictionary<DayOfWeek, TimeFrame>();

            if (!daySelected())
            {
                new MBox("Day available cannot be empty").ShowDialog();
                return;
            }
            // Validate
            if (IsConstant)
            {
                // Set constant status
                
                string startTime = "";
                string endTime = ""; 
                foreach (var child in sp_ConstTime.Children)
                {
                    if (child.GetType() == typeof(TimeInput))
                    {
                        TimeInput timeInput = (TimeInput) child;
                        

                        if (timeInput.IsStart == true)
                        {
                            startTime = timeInput.Time;
                        }
                        else
                        {
                            endTime = timeInput.Time;
                        }
                    }
                }
                
                TimeFrame tf;
                try
                {
                    tf = new TimeFrame(startTime, endTime);
                } 
                catch (Exception ex)
                {
                    new MBox(ex.Message).ShowDialog();
                    return;
                }

                
                foreach (Grid container in sp_DaysContainer.Children)
                {
                    CheckBox? cb_Day = container.Children.OfType<CheckBox>().FirstOrDefault();
                    if (cb_Day == null)
                    {
                        new MBox("cb_Day is null").ShowDialog();
                        return;
                    }

                    if (cb_Day.IsChecked == true)
                    {
                        DayOfWeek day;

                        if (!Enum.TryParse<DayOfWeek>(cb_Day.Content.ToString(), out day))
                        {
                            new MBox($"Cannot parse {cb_Day.Content}").ShowDialog();
                            return;
                        }
                        dailyTimeframe.Add(day,tf);
                    }
                }

                _Schedule.DailyTimeframe = dailyTimeframe;
                _Schedule.IsConstant = true;
            }
            else
            {
                // Set  timeframes
                foreach (Grid container in sp_DaysContainer.Children)
                {
                    CheckBox? cb_Day = container.Children.OfType<CheckBox>().FirstOrDefault();
                    if (cb_Day == null)
                    {
                        new MBox("cb_Day is null").ShowDialog();
                        return;
                    }

                    // Find container of timeframe
                    StackPanel? tfContainer;
                    if (cb_Day.IsChecked == true)
                    {
                        tfContainer = container.Children.OfType<StackPanel>().FirstOrDefault();
                    }
                    else
                    {
                        new MBox("tfContainer is null").ShowDialog();
                        return;
                    }

                    string? constStartTime;
                    string? constEndTime;
                    foreach (var timeInput in tfContainer.Children)
                    {
                        if (timeInput.GetType() == typeof(TimeInput))
                        {
                            // TODO: Complete custom time saving
                        }
                    }

                }
            }
            
            DialogResult = true;
        }
    }
}
