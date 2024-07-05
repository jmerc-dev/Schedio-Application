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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for Counter.xaml
    /// </summary>
    public partial class Counter : UserControl
    {
        public Counter()
        {
            InitializeComponent();
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            int num = Convert.ToInt32(tb_Num.Text);
            if (num < 99)
            {
                tb_Num.Text = (num + 1).ToString();
            }
        }

        private void btn_Sub_Click(object sender, RoutedEventArgs e)
        {
            int num = Convert.ToInt32(tb_Num.Text);
            if (num > 0)
            {
                tb_Num.Text = (num - 1).ToString();
            }
        }

        private void tb_Num_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
