using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
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
    /// 

    public enum MBoxImage
    {
        Information,
        Warning
    }

    public enum Sound
    {
        NoSound
    }

    public enum MBoxType
    {
        OK,
        CancelOrOK,
        ConfirmDelete
    }
    public partial class MBox : Window
    {
        private MBoxType? _Type;
        public MBoxType? Type
        {
            get { return _Type; }
        }

        public MBox(string message)
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            this.DataContext = this;
            _Type = MBoxType.OK;
            SystemSounds.Asterisk.Play();
            tb_Message.Text = message;
            btn_OK.Focus();
        }

        public MBox(string message, Sound sound)
        {
            InitializeComponent();
            this.DataContext = this;
            _Type = MBoxType.OK;
            this.Owner = Application.Current.MainWindow;
            switch (sound) 
            {
                case Sound.NoSound:
                    break;
            }

            tb_Message.Text = message;
            btn_OK.Focus();
        }

        public MBox(string message, MBoxType type)
        {
            InitializeComponent();
            this.DataContext = this;
            _Type = type;
            this.Owner = Application.Current.MainWindow;
            switch (type)
            {
                case MBoxType.CancelOrOK:
                    btn_Cancel.Visibility = Visibility.Visible;
                    btn_OK.IsCancel = false;
                    tb_Message.Text = message;
                    break;
                case MBoxType.OK:
                    tb_Message.Text = message;
                    break;
                case MBoxType.ConfirmDelete:
                    img_Picture.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/warning.png"));
                    btn_OK.Visibility = Visibility.Collapsed;
                    tb_Message.Text = "Are you sure you want to delete? Please type “Confirm” to continue.";
                    break;
                default:
                    throw new ArgumentException();
            }

            
            btn_OK.Focus();
        }

        public MBox(string message, MBoxImage image)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.DataContext = this;
            _Type = MBoxType.OK;
            this.Owner = Application.Current.MainWindow;

            tb_Message.Text = message;
            switch (image)
            {
                case MBoxImage.Information:
                    img_Picture.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/info.png"));
                    break;
                case MBoxImage.Warning:
                    img_Picture.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/warning.png"));
                    btn_OK.Background = new SolidColorBrush(Colors.Red);
                    break;
                default:
                    break;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void tbx_Confirm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbx_Confirm.Text.Equals("Confirm", StringComparison.CurrentCulture))
                btn_OKRed.IsEnabled = true;
            else
                btn_OKRed.IsEnabled = false;
        }
    }
}
