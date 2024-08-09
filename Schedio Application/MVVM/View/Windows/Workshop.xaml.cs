﻿using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for Workshop.xaml
    /// </summary>
    public partial class Workshop : Window
    {
        public Workshop()
        {
            InitializeComponent();
        }

        private void sv_Canvas_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)sender;
            recta.SetValue(Canvas.TopProperty, sv.VerticalOffset);
            recta.SetValue(Canvas.LeftProperty, sv.HorizontalOffset);
            Timeslot.SetValue(Canvas.LeftProperty, sv.HorizontalOffset);
            RoomCategoriesContainer.SetValue(Canvas.TopProperty, sv.VerticalOffset);
        }

        private void entriesContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
