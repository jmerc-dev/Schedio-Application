using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private EntryFilterElement _FilterElement;

        // Base Filter for Days
        private ObservableCollection<SubjectEntry> _FilteredEntries;

        // 2nd Level: filter for cbox_Filters (Room, Section, Person) OR Subject Filter if cbox_Filter is NONE.
        private ObservableCollection<SubjectEntry>? _FilterLevel2;

        // 3rd Level: Subject Filter IF (cbox_Filters is not NONE)
        private ObservableCollection<SubjectEntry>? _FilterLevel3;

        private EntryFilter _FilterObj;

        // Day Filter
        private bool[] _DayFilter = new bool[7];

        public ObservableCollection<SubjectEntry> Entries
        {
            get { return _Entries; }
        }

        public ObservableCollection<SubjectEntry> FilteredEntries
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

        private void cbox_Filters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tb_SearchByFilter != null)
                tb_SearchByFilter.Text = String.Empty;
            ComboBox? comboBox = sender as ComboBox;

            if (comboBox == null || comboBox.SelectedItem == null)
                throw new NullReferenceException();

            ComboBoxItem? selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedItem == null)
                throw new NullReferenceException();

            string? chosenFilterString = selectedItem.Content.ToString();

            if (chosenFilterString == null) 
                throw new NullReferenceException();

            EntryFilterElement chosenFilter;
            bool success = Enum.TryParse<EntryFilterElement>(chosenFilterString, false, out chosenFilter);

            if (!success && lv_SubjectsList != null)
            {
                lv_SubjectsList.ItemsSource = FilteredEntries;
                _FilterElement = EntryFilterElement.None;
                return;
            }

            _FilterElement = chosenFilter;
        }

        private void tb_SearchByFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string key = tb_SearchByFilter.Text;
            if (key.Equals(String.Empty))
            {
                lv_SubjectsList.ItemsSource = FilteredEntries;
                _FilterLevel2 = null;
                return;
            }

            if (_FilterLevel2 == null) 
                _FilterLevel2 = new ObservableCollection<SubjectEntry>(FilteredEntries);

            EntryFilter efLevel2 = new EntryFilter(FilteredEntries, _FilterLevel2);
            efLevel2.Filter(_FilterElement, key);
            lv_SubjectsList.ItemsSource = _FilterLevel2;
        }

        private void tb_SearchSubject_TextChanged(object sender, TextChangedEventArgs e)
        {
            string key = tb_SearchSubject.Text;

            if (key.Equals(String.Empty))
            {
                if (_FilterLevel2 != null)
                    lv_SubjectsList.ItemsSource = _FilterLevel2;
                else
                    lv_SubjectsList.ItemsSource = FilteredEntries;
            }                

            EntryFilter efNewLevel;     // Can be level 2 or 3

            if (_FilterElement == EntryFilterElement.None || _FilterLevel2 == null)
            {
                _FilterLevel2 = new ObservableCollection<SubjectEntry>(FilteredEntries);
                efNewLevel = new EntryFilter(FilteredEntries, _FilterLevel2);
                lv_SubjectsList.ItemsSource = _FilterLevel2;
            }
            else
            {
                if (_FilterLevel3 == null)
                    _FilterLevel3 = new ObservableCollection<SubjectEntry>(_FilterLevel2);
                efNewLevel = new EntryFilter(_FilterLevel2, _FilterLevel3);
                lv_SubjectsList.ItemsSource = _FilterLevel3;
            }

            efNewLevel.Filter(EntryFilterElement.Subject, key);

        }
    }
}
