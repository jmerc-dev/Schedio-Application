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

        private int _Number;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Number
        {
            get { return _Number; }
            set 
            { 
                _Number = value;
                NotifyPropertyChanged(); 
            }
        }

        public Counter()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {

            if (Number < 99)
            {
                Number += 1;
            }

            Trace.WriteLine(Number.ToString());
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
                /* Option 1: Regex for blank, 1char, 2char - IMPLEMENTED
                 * Option 2: Validate on LostFocus (still able to write invalid char) NO
                */


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
