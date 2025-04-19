using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private EntryFilter _FilterObj;
        // Day Filter
        private bool[] _DayFilter = new bool[7];

        public ObservableCollection<SubjectEntry> Entries
        {
            get { return _Entries; }
        }

        public ObservableCollection<SubjectEntry>? FilteredEntries
        {
            get { return _FilteredEntries; }
        }

        public EntryLocator(Workshop wk, ObservableCollection<SubjectEntry> entries)
        {

            this._Workshop = wk;
            this._Entries = entries;
            this._FilteredEntries = new ObservableCollection<SubjectEntry>(entries);
            this._FilterObj = new EntryFilter(_Entries, _FilteredEntries);

            InitializeComponent();

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

        private void DayFilter_Changed(object sender, RoutedEventArgs e)
        {
            AssignDayFilter((CheckBox)sender);
            _FilterObj.FilterByDays(_DayFilter);

            Trace.WriteLine("####################");
            for (int i = 0; i < 7; i++)
                Trace.WriteLine(_DayFilter[i]);
        }

        private void AssignDayFilter(CheckBox? cbox)
        {
            CheckBox? checkBox = cbox;

            if (checkBox == null)
                throw new ArgumentNullException();

            bool? isChecked = checkBox.IsChecked == null ? throw new NullReferenceException() : checkBox.IsChecked;
            string? dayName = checkBox.Content.ToString();

            if (dayName == null || checkBox.Content == null || checkBox.Content.ToString() == null)
            {
                throw new ArgumentNullException();
            }

            DayOfWeek? dayOfWeek = Enum.Parse<DayOfWeek>(dayName);

            if (dayOfWeek != null)
                _DayFilter[(int)dayOfWeek - 1 < 0 ? 6 : (int)dayOfWeek - 1] = (bool) isChecked;
        }
    }
}
