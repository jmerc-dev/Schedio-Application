using System;
using System.Collections;
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
    /// Interaction logic for WarningConfirmation.xaml
    /// </summary>
    public partial class WarningConfirmation : Window
    {
        public WarningConfirmation(string type, string name)
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                tb_Name.Text = name;
                tb_Type.Text = type;
                DisplayMultiItems.Visibility = Visibility.Collapsed;
            };
        }

        public WarningConfirmation(List<string> itemsToBeRemoved, string type)
        {
            InitializeComponent();
            DisplayOneItem.Visibility = Visibility.Collapsed;
            tb_multiType.Text = type;

            foreach (string item in itemsToBeRemoved)
            {
                wp_Names.Children.Add(new TextBlock() { Text = item });
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
