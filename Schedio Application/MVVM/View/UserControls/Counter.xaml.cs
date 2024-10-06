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

        private int oldValue = 0;


        private static readonly DependencyProperty _Number =
            DependencyProperty.Register("Number", typeof(int), typeof(Counter), new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Number
        {
            get { return (int)GetValue(_Number); }
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

            if (Number < 99)
            {
                Number += 1;
            }
        }

        private void btn_Sub_Click(object sender, RoutedEventArgs e)
        {
            if (Number > 0)
            {
                Number -= 1;
            }

        }

        private void tb_Num_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Validation that restricts input only for digits
            if (tb_Num.Text.Equals(String.Empty))
            {
                return;
            }
            else if (Regex.Match(tb_Num.Text, @"^\d{1}$").Success)
            {
                oldValue = Convert.ToInt32(tb_Num.Text);  // "1"
            }
            else if (Regex.Match(tb_Num.Text, @"^\d{2}$").Success)
            {
                oldValue = Convert.ToInt32(tb_Num.Text);
            }
            else
            {
                if (tb_Num.Text.Length == 1)
                {
                    tb_Num.Text = "";
                }
                else
                {
                    Number = oldValue;
                    tb_Num.CaretIndex = 1;
                }
            }
        }

        private void tb_Num_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_Num.Text.Equals(String.Empty))
            {
                Number = 0;
            }
        }
    }
}
