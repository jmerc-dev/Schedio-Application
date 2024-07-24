﻿using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
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
    public partial class MBox : Window
    {
        public MBox(string message)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            SystemSounds.Asterisk.Play();

            tb_Message.Text = message;
        }

        public MBox(string message, MBoxImage image)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;

            tb_Message.Text = message;
            switch (image)
            {
                case MBoxImage.Information:
                    img_Picture.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/info.png"));
                    break;
                case MBoxImage.Warning:
                    img_Picture.Source = new BitmapImage(new Uri("pack://application:,,,/Schedio Application;component/Resources/Images/warning.png"));
                    break;
                default:
                    break;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
    }
}
