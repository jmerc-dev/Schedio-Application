using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class Counter : UserControl
    {

        private string oldValue = "0";

        public Counter()
        {
            InitializeComponent();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(tb_Num.Text);
                if (num < 99)
                {
                    tb_Num.Text = (num + 1).ToString();
                }
            }
            catch
            {
                tb_Num.Text = "1";
            }

        }

        private void btn_Sub_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(tb_Num.Text);
                if (num > 0)
                {
                    tb_Num.Text = (num - 1).ToString();
                }
            } catch
            {
                tb_Num.Text = "1";
            }

        }

        private void tb_Num_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_Num.Text.Equals(String.Empty))
            {
                oldValue = String.Empty;
                return;
            }

            string pattern = @"^\d{1,2}$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(tb_Num.Text))
            {
                oldValue = tb_Num.Text;
            }
            else
            {
                tb_Num.Text = oldValue;
                tb_Num.CaretIndex = oldValue.Length;
            }
        }
    }
}
