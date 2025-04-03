using Microsoft.Win32;
using Schedio_Application.MVVM.Model;
using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Custom_Exceptions;
using Schedio_Application.MVVM.ViewModel.WrapperClasses;
using System.Diagnostics;
using System.IO;
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
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - 8;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
        }

        private void btn_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Application.Current?.MainWindow?.Close();
        }

        private void btn_MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_MaximizeWindow_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                img_Max.Source = new BitmapImage(
                    new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/maximize.png"));
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                img_Max.Source = new BitmapImage(
                    new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/restore-down.png"));
            }

        }

        private void btn_OpenProject_MouseEnter(object sender, MouseEventArgs e)
        {
            img_OpenProject.Source = new BitmapImage(
                new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/open-file-inversed.png"));
        }

        private void btn_OpenProject_MouseLeave(object sender, MouseEventArgs e)
        {
            img_OpenProject.Source = new BitmapImage(
                new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/open-file.png"));
        }

        private void btn_NewProject_Click(object sender, RoutedEventArgs e)
        {
            Workshop wk = new Workshop();
            Application.Current.MainWindow.Visibility = Visibility.Collapsed;
            wk.Show();
        }

        private void btn_OpenProject_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON File (JSON)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    FileLoader fileLoader = new FileLoader(openFileDialog.FileName);
                    FullDataWrapper? data = fileLoader.Execute();

                    if (data != null)
                    {
                        Workshop wk = new Workshop(data);
                        Application.Current.MainWindow.Visibility = Visibility.Collapsed;
                        wk.Show();
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }

                }
                catch (Exception ex)
                {
                    new MBox(ex.Message, MBoxImage.Warning).ShowDialog();
                    return;
                }
            }
        }
    }
}