using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.ViewModel.Custom_Exceptions;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
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
using static System.Net.Mime.MediaTypeNames;


namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for PersonnelCustomTime.xaml
    /// </summary>
    public partial class PersonnelCustomTime : Window
    {
        public Dictionary<DayOfWeek, List<TimeFrame>> dailyTimeframe;

        // Creating new
        public PersonnelCustomTime(Dictionary<string, bool> availableDays)
        {
            InitializeComponent();
            DisableDaysExpander(availableDays);
            this.Owner = System.Windows.Application.Current.MainWindow;

            dailyTimeframe = new Dictionary<DayOfWeek, List<TimeFrame>>();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        // Updating
        public PersonnelCustomTime(Dictionary<string, bool> availableDays, Dictionary<DayOfWeek, List<TimeFrame>> dtf)
        {   
            InitializeComponent();
            dailyTimeframe = dtf;
            DisableDaysExpander(availableDays);


            foreach (Expander exp in sp_ExpanderContainer.Children)
            {
                DayOfWeek day;
                if(!Enum.TryParse(((TextBlock)exp.Header).Text, true, out day))
                {
                    MessageBox.Show($"Cannot Parse {((TextBlock)exp.Header).Text}");
                }

                if (!dailyTimeframe.ContainsKey(day))
                {
                    continue;
                }

                foreach (TimeFrame tf in dailyTimeframe[day])
                {
                    StackPanel container = (StackPanel)exp.Content;
                    container.Children.Insert(container.Children.Count - 1, new StartEndTimeInput(tf));
                }
            }
        }

        private bool ValidateTimeOverlap() // Returns true if there are no overlaps in timeframe entries
        {
            // loop in each day
            foreach(Expander exp in sp_ExpanderContainer.Children)
            {
                List<TimeFrame> dayTf = new List<TimeFrame>();
                if (exp.IsEnabled)
                {
                    // Loop in timeframe entries
                    StackPanel entryContainer = (StackPanel)exp.Content;
                    foreach(var entry in entryContainer.Children)
                    {
                        if (entry.GetType() == typeof(StartEndTimeInput))
                        {
                            TimeFrame tfEntry = new TimeFrame(((StartEndTimeInput) entry).StartTime, ((StartEndTimeInput)entry).EndTime);
                            if (dayTf.Count != 0)
                            {
                                // Comparison
                                foreach(TimeFrame timeFrame in dayTf)
                                {
                                    if (timeFrame.IsOverlap(tfEntry.StartTime) || timeFrame.IsOverlap(tfEntry.EndTime) || timeFrame.IsContainedBy(tfEntry))
                                    {
                                        throw new TimeframeOverlapException(timeFrame, tfEntry, ((TextBlock) exp.Header).Text);
                                    }
                                }
                                dayTf.Add(tfEntry);
                            }
                            else
                            {
                                dayTf.Add(tfEntry);
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Validate entries
            try
            {
                ValidateTimeOverlap();
            }
            catch (Exception ex)
            {
                new MBox(ex.Message).ShowDialog();
                return;
            }

            // Clear dictionary
            foreach (KeyValuePair<DayOfWeek, List<TimeFrame>> listTf in dailyTimeframe)
            {
                listTf.Value.Clear();
            }
            // Loop through mon-sun
            foreach (Expander exp in sp_ExpanderContainer.Children)
            {
                // Only loop on enabled (available)
                if (exp.IsEnabled)
                {
                    if (((StackPanel)exp.Content).Children.Count == 1)
                    {
                        new MBox($"{((TextBlock)exp.Header).Text} is empty.").ShowDialog();
                        return;
                    }
                    DayOfWeek day;
                    
                    if (!Enum.TryParse(((TextBlock)exp.Header).Text, true, out day))
                    {
                        MessageBox.Show($"Unable to parse {((TextBlock)exp.Header).Text} to {typeof(DayOfWeek)}");
                    }
                    
                    // Iterate on each timeframe inside a day
                    foreach (var entry in ((StackPanel)exp.Content).Children)
                    {
                        if (entry.GetType() == typeof(StartEndTimeInput))
                        {
                            StartEndTimeInput entryConverted = (StartEndTimeInput)entry;
                            TimeFrame tf;
                            try
                            {
                                tf = new TimeFrame(entryConverted.StartTime, entryConverted.EndTime);
                            }
                            catch (Exception ex)
                            {
                                new MBox(ex.Message).ShowDialog();
                                return;
                            }
                            

                            if (!dailyTimeframe.ContainsKey(day))
                            {
                                dailyTimeframe.Add(day, new List<TimeFrame>());
                            }
                            dailyTimeframe[day].Add(tf);
                        }
                    }
                }
            }

            DialogResult = true;
        }

        public void UpdateAvailableDays(Dictionary<string, bool> availableDays)
        {
            DisableDaysExpander(availableDays);
        }

        private void DisableDaysExpander(Dictionary<string, bool> availableDays)
        {
            foreach (Expander expander in sp_ExpanderContainer.Children)
            {
                expander.IsEnabled = availableDays[((TextBlock)expander.Header).Text.ToString()];

                if (!availableDays[((TextBlock)expander.Header).Text.ToString()])
                {
                    expander.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Add entry
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartEndTimeInput entry = new StartEndTimeInput(); 

            StackPanel parent = (StackPanel)((Button)sender).Parent;
            parent.Children.Insert(parent.Children.Count - 1, entry);

        }


    }
}
