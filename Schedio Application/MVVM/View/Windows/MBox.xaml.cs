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
    /// Interaction logic for MBox.xaml
    /// </summary>
    public partial class MBox : Window
    {
        public MBox(string message)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            tb_Message.Text = message;
        }
    }
}
