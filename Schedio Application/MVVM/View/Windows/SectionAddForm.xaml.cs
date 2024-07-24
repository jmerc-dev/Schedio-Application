﻿using Schedio_Application.MVVM.View.UserControls;
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
    /// Interaction logic for SectionAddForm.xaml
    /// </summary>
    public partial class SectionAddForm : Window
    {
        public SectionAddForm()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            this.ShowInTaskbar = false;
        }

        private void btn_AddSubject_Click(object sender, RoutedEventArgs e)
        {
            sp_SubjectList.Children.Insert(sp_SubjectList.Children.Count - 1, new SubjectItem());
        }
    }
}
