using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Diagnostics;

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for HomeNavButtons.xaml
    /// </summary>
    public partial class HomeNavButtons : UserControl
    {

        private bool btn_HomeButton_isFocused = false;
        private bool btn_NewButton_isFocused = false;
        private bool btn_OpenButton_isFocused = false;

        private ImageSource home_focused_Image = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/home-focused.png"));
        private ImageSource home_Image = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/home.png"));
        private ImageSource new_focused_Image = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/new-focused.png"));
        private ImageSource new_Image = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/new.png"));
        private ImageSource open_Image = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/open.png"));
        private ImageSource open_focused_Image = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/open-focused.png"));

        private ResourceDictionary ColorDictionary;

        public bool HomeButtonFocus
        {
            get { return btn_HomeButton_isFocused;}
            set {  btn_HomeButton_isFocused = value;}
        }

        public bool NewButtonFocus
        {
            get { return btn_NewButton_isFocused; }
            set { btn_NewButton_isFocused = value; }
        }

        public bool OpenButtonFocus
        {
            get { return btn_OpenButton_isFocused; }
            set { btn_OpenButton_isFocused = value; }
        }

        public HomeNavButtons()
        {
            InitializeComponent();
            ColorDictionary = new ResourceDictionary();
            ColorDictionary.Source = new Uri("/Schedio Application;component/Themes/Colors.xaml", UriKind.RelativeOrAbsolute);
        }

        private void clearFocuses()
        {
            btn_HomeButton_isFocused = false;
            btn_NewButton_isFocused = false;
            btn_OpenButton_isFocused = false;
        }

        private void resetButtonImages()
        {
            HomeImage.Source = home_Image;
            NewImage.Source = new_Image;
            OpenImage.Source = open_Image;
        }

        // Home Button Events
        private void btn_HomeButton_Clicked(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_HomeButton_GotFocus(object sender, RoutedEventArgs e)
        {
            clearFocuses();
            resetButtonImages();
            btn_HomeButton_isFocused = true;
            HomeImage.Source = home_focused_Image;
            //HomeBackground.Fill = (Brush) ColorDictionary["ShadedSecondaryColor"];
        }

        private void btn_HomeButton_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void btn_HomeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            HomeImage.Source = home_focused_Image;
        }

        private void btn_HomeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!btn_HomeButton_isFocused)
            {
                HomeImage.Source = home_Image;
            }
            
        }


        // New Button Events
        private void btn_NewButton_GotFocus(object sender, RoutedEventArgs e)
        {
            clearFocuses();
            resetButtonImages();
            btn_NewButton_isFocused = true;
            NewImage.Source = new_focused_Image;
        }

        private void btn_NewButton_LostFocus(object sender, RoutedEventArgs e)
        {
        
        }

        private void btn_NewButton_MouseEnter(object sender, MouseEventArgs e)
        {
            NewImage.Source = new_focused_Image;
        }

        private void btn_NewButton_MouseLeave(object sender, MouseEventArgs e)
        { 
            if (!btn_NewButton_isFocused)
            {
                NewImage.Source = new_Image;
            }
        }

        // Open Button events
        private void OpenButton_GotFocus(object sender, RoutedEventArgs e)
        {
            clearFocuses();
            resetButtonImages();
            btn_OpenButton_isFocused = true;
            OpenImage.Source = open_focused_Image;
        }

        private void OpenButton_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void OpenButton_MouseEnter(object sender, MouseEventArgs e)
        {
            OpenImage.Source = open_focused_Image;
        }

        private void OpenButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!OpenButtonFocus)
            {
                OpenImage.Source = open_Image;
            }
            
        }
    }
}
