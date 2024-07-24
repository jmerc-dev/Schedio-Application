using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TimeScheduleAddForm.xaml
    /// </summary>
    public partial class TimeScheduleAddForm : Window
    {
        public TimeScheduleAddForm()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
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
            foreach (Grid container in sp_DaysContainer.Children)
            {
                container.Children.OfType<StackPanel>().ToList().ForEach((stackPanel) =>
                {
                    stackPanel.IsEnabled = !isConstant;
                });
            }
 
            ti_ConstTime.IsEnabled = isConstant;

        }
    }
}
