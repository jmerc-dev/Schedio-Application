using Schedio_Application.MVVM.ViewModel.ScheduleElements;
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
    /// Interaction logic for SubjectAllocation.xaml
    /// </summary>
    public partial class SubjectAllocation : Window
    {
        private SubjectEntry entry;

        public SubjectAllocation(Subject subject)
        {
            InitializeComponent();

            entry = new SubjectEntry(subject);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {

                }
                else
                {
                    this.DragMove();
                }
        }

        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
