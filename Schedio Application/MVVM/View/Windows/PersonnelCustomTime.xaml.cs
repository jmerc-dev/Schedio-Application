using Schedio_Application.MVVM.View.UserControls;
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

            dailyTimeframe = new Dictionary<DayOfWeek, List<TimeFrame>>();
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

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Validate entries


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
                            TimeFrame tf = new TimeFrame(entryConverted.StartTime, entryConverted.EndTime);

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
