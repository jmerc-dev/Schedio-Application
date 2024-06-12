using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedio_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void HomeButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine("Home button clicked!");
        }

        private void HomeButton_GotFocus(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("pack://application:,,,/Schedio Application;component/MVVM/View/Pages/HomePage.xaml"));
        }

        private void NewButton_GotFocus(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("pack://application:,,,/Schedio Application;component/MVVM/View/Pages/NewPage.xaml"));
        }

        private void OpenButton_GotFocus(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(new Uri("pack://application:,,,/Schedio Application;component/MVVM/View/Pages/OpenPage.xaml"));
        }
    }
}