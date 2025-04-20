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

            // TODO: For some reason, this causes issue when clicking the 'All' checkbox
            //"All" checkbox setter
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

        private void cbox_AllDays_Clicked(object sender, RoutedEventArgs e)
        {
            CheckBox? cbox_All = sender as CheckBox;

            if (cbox_All == null || cbox_All.IsChecked == null)
                throw new NullReferenceException();

            if (container_DayFilter == null)
                return;

            foreach (CheckBox cbox in container_DayFilter.Children)
            {
                cbox.IsChecked = cbox_All.IsChecked;
            }
        }

        private void cbox_AllDays_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox? cbox_All = sender as CheckBox;

            if (cbox_All == null || cbox_All.IsChecked == null)
                throw new NullReferenceException();

            if (container_DayFilter == null)
                return;

            foreach (CheckBox cbox in container_DayFilter.Children)
            {
                cbox.IsChecked = true;
            }
        }
        
        private void cbox_AllDays_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox? cbox_All = sender as CheckBox;

            if (cbox_All == null || cbox_All.IsChecked == null)
                throw new NullReferenceException();

            if (container_DayFilter == null)
                return;

            foreach (CheckBox cbox in container_DayFilter.Children)
            {
                cbox.IsChecked = false;
            }
        }


        private bool IsAllChecked()
        {
            foreach (bool b in _DayFilter)
            {
                if (!b) return false;
            }

            return true;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (cbox_AllDays.IsChecked == null)
                throw new NullReferenceException();

            if (IsAllChecked() && (bool)!cbox_AllDays.IsChecked)
                cbox_AllDays.IsChecked = true;
            else if (!IsAllChecked() && (bool)cbox_AllDays.IsChecked)
                cbox_AllDays.IsChecked = false;
        }
    }
}
