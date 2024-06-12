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
        public HomeNavButtons()
        {
            InitializeComponent();
            this.DataContext = this;

            Loaded += (sender, args) =>
            {
                if (LabelName.Equals("Home"))
                {
                    btn_Button.Focus();
                }
            };
        }


        // Image
        public string ImagePath
        {
            get { return (string)GetValue(_ImagePath); }
            set { SetValue(_ImagePath, value); }
        }

        public static readonly DependencyProperty _ImagePath =
        DependencyProperty.Register("ImagePath", typeof(string), typeof(HomeNavButtons), new PropertyMetadata(null));

        // Default Image
        public string DefaultImagePath
        {
            get { return (string)GetValue(_DefaultImagePath); }
            set { SetValue(_DefaultImagePath, value); }
        }

        public static readonly DependencyProperty _DefaultImagePath =
        DependencyProperty.Register("DefaultImagePath", typeof(string), typeof(HomeNavButtons), new PropertyMetadata(null));

        // Focused Image
        public string FocusedImagePath
        {
            get { return (string)GetValue(_FocusedImagePath); }
            set { SetValue(_FocusedImagePath, value); }
        }

        public static readonly DependencyProperty _FocusedImagePath =
        DependencyProperty.Register("FocusedImagePath", typeof(string), typeof(HomeNavButtons), new PropertyMetadata(null));

        // Label
        public string LabelName
        {
            get { return (string)GetValue(_Label); }
            set { SetValue(_Label, value); }
        }

        public static readonly DependencyProperty _Label =
        DependencyProperty.Register("LabelName", typeof(string), typeof(HomeNavButtons), new PropertyMetadata(null));

        // Button Events

        private void btn_Button_GotFocus(object sender, RoutedEventArgs e)
        {
            ButtonImage.Source = new BitmapImage(new Uri((string)GetValue(_FocusedImagePath)));
        }

        private void btn_Button_LostFocus(object sender, RoutedEventArgs e)
        {
            ButtonImage.Source = new BitmapImage(new Uri((string)GetValue(_DefaultImagePath)));
        }
    }
}
