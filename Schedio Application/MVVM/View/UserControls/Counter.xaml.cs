using Schedio_Application.MVVM.View.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for Counter.xaml
    /// </summary>
    public partial class Counter : UserControl, INotifyPropertyChanged
    { 

        private static readonly DependencyProperty _Number =
            DependencyProperty.Register("Number", typeof(double), typeof(Counter), new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;
        private double oldValue;


        public double Number
        {
            get { return (double)GetValue(_Number); }
            set 
            {
                
                SetValue(_Number, value);
            }
        }

        public Counter()
        {
            InitializeComponent();
        }


        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {

            if (Number + 1 > 99)
            {
                return;
            }

            if (Number < 99)
            {
                Number += 1;
            }
        }

        private void btn_Sub_Click(object sender, RoutedEventArgs e)
        {
            if (Number - 1 < 0)
            {
                return;
            }

            if (Number > 0)
            {
                Number -= 1;
            }

        }

        private void tb_Num_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tb_Num.Text.Equals(String.Empty))
                {
                    
                    return;
                }
                Convert.ToDouble(tb_Num.Text);
            } catch (Exception ex)
            {
                new MBox("Invalid input").ShowDialog();
                tb_Num.Clear();
            }

        }

        private void tb_Num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_Num.Text.Equals(String.Empty))
            {
                Trace.WriteLine("empty string");
                Number = 0;
                tb_Num.Text = "0";
            }
        }

        private void tb_Num_KeyDown(object sender, KeyEventArgs e)
        {
            // Keypress
            string key = e.Key.ToString();
            string pattern = @"^[D][0-9]$";
            string pattern2 = @"^NumPad[0-9]$";
            if (new Regex(pattern).IsMatch(key) || new Regex(pattern2).IsMatch(key) || key.Equals("Decimal") || key.Equals("OemPeriod"))
            {
                // DO nothing
                
            }
            else if (key.Equals("Return") || key.Equals("Escape"))
            {
                // TODO: Upon pressing enter, the values here should save
                // Fix tomorrow
                //Keyboard.ClearFocus();
            }
            else
            {
                
                e.Handled = true;
                return;
            }
        }

        
    }
}
