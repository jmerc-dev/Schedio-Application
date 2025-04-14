using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EntryLocator.xaml
    /// </summary>
    public partial class EntryLocator : Window
    {
        private Workshop _Workshop;
        private ObservableCollection<SubjectEntry> _Entries;
        private ObservableCollection<SubjectEntry>? _FilteredEntries;

        public ObservableCollection<SubjectEntry> Entries
        {
            get { return _Entries; }
        }

        public EntryLocator(Workshop wk, ObservableCollection<SubjectEntry> entries)
        {
            InitializeComponent();

            this._Workshop = wk;
            this._Entries = entries;

            this.DataContext = this;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
