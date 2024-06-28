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
    /// Interaction logic for PersonnelCustomTime.xaml
    /// </summary>
    public partial class PersonnelCustomTime : Window
    {
        public PersonnelCustomTime(Dictionary<string, bool> availableDays)
        {
            InitializeComponent();
            DisableDaysExpander(availableDays);

        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DisableDaysExpander(Dictionary<string, bool> availableDays)
        {
            foreach (Expander expander in sp_ExpanderContainer.Children)
            {
                expander.IsEnabled = availableDays[((TextBlock)expander.Header).Text.ToString()];
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StackPanel parent = (StackPanel)((Button)sender).Parent;
            Button newButton = new Button();
            newButton.Content = parent.Children.Count - 1;
            newButton.Click += NewButton_Click;
            parent.Children.Insert(parent.Children.Count - 1, newButton);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(((StackPanel)((Button)sender).Parent).Children.IndexOf(((Button)sender)));
        }
    }
}
