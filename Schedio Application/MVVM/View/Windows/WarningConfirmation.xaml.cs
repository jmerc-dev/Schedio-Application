using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
            this.Owner = Application.Current.MainWindow;
            Loaded += (sender, e) =>
            {
                tb_Name.Text = name;
                tb_Type.Text = type;
                DisplayMultiItems.Visibility = Visibility.Collapsed;
                SystemSounds.Exclamation.Play();
            };
        }

        public WarningConfirmation(string type, List<string> itemsToBeRemoved)
        {
            InitializeComponent();
            DisplayOneItem.Visibility = Visibility.Collapsed;
            tb_multiType.Text = type;
            this.Owner = Application.Current.MainWindow;
            SystemSounds.Asterisk.Play();
            foreach (string item in itemsToBeRemoved)
            {
                wp_Names.Children.Add(new TextBlock() { Text = item });
            }

        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
