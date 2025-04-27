using Schedio_Application.MVVM.ViewModel.Commands;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private readonly ObservableCollection<SubjectEntry> _Entries;
        private EntryFilterElement _FilterElement;
        private bool IsSecondLevelOn;
        

        // Base Filter for Days
        private readonly ObservableCollection<SubjectEntry> _FilteredEntries;

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

        // Day filtering
        public ObservableCollection<SubjectEntry> FilteredEntries
        {
            get { return _FilteredEntries; }
        }

        private ObservableCollection<SubjectEntry>? FilterLevel2
        {
            get => _FilterLevel2;
            set
            {
                _FilterLevel2 = value;

                if (_FilterLevel2 != null)
                    _FilterLevel2.CollectionChanged += FilteredEntries2_CollectionChanged;
            }
        }

        public Workshop MyWorkshop
        {
            get => _Workshop;
        }

        private void _FilterLevel2_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public EntryLocator(Workshop wk, ObservableCollection<SubjectEntry> entries)
        {

            this._Workshop = wk;
            this._Entries = entries;
            this._FilteredEntries = new ObservableCollection<SubjectEntry>(entries);
            this._FilterObj = new EntryFilter(_Entries, _FilteredEntries);
            this._Entries.CollectionChanged += this.Entries_CollectionChanged;
            this._FilteredEntries.CollectionChanged += FilteredEntries_CollectionChanged;
            this._Workshop.Closing += (sender, e) =>
            {
                this.Close();
            };

            InitializeComponent();

            this.DataContext = this;
        }

        private void FindSubEntry()
        {
            Trace.WriteLine("Finding Subject Entry");
        }

        private void Entries_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null || e.NewItems.Count == 0)
                    throw new ArgumentNullException();

                foreach (SubjectEntry entry in e.NewItems)
                {
                    if (!_DayFilter[CultureDayOfWeek.ConvertBack(entry.DayAssigned)])
                        continue;

                    if (!FilteredEntries.Contains(entry)) 
                        FilteredEntries.Add(entry);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems == null || e.OldItems.Count == 0)
                    throw new ArgumentNullException();

                foreach (SubjectEntry entry in e.OldItems)
                {
                    if (FilteredEntries.Contains(entry))
                        FilteredEntries.Remove(entry);
                }
            }
        }

        private void FilteredEntries_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {

            ObservableCollection<SubjectEntry> src = FilteredEntries;
            ObservableCollection<SubjectEntry>? dst = IsSecondLevelOn ? FilterLevel2 : _FilterLevel3;
            EntryFilterElement efe = IsSecondLevelOn ? _FilterElement : EntryFilterElement.Subject;

            if (tb_SearchByFilter == null || tb_SearchSubject == null)
                return;

            string key = IsSecondLevelOn ? tb_SearchByFilter.Text : tb_SearchSubject.Text;

            if (dst == null || key.Equals(String.Empty))
                return;

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems == null || e.OldItems.Count == 0)
                    throw new ArgumentNullException();

                foreach (SubjectEntry s in e.OldItems)
                {
                    if (dst.Contains(s))
                        dst.Remove(s);
                    
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                EntryFilter ef = new EntryFilter(src, dst);
                ef.Filter(efe, key);
            }
        }

        private void FilteredEntries2_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (_FilterLevel3 == null)
                return;

            ObservableCollection<SubjectEntry>? src = FilterLevel2;
            ObservableCollection<SubjectEntry> dst = _FilterLevel3;
            string? key = null;

            if (tb_SearchSubject == null)
                return;

            if (src == null || dst == null)
                return;

            key = tb_SearchSubject.Text;

            if (key.Equals(string.Empty))
                return;

            EntryFilter? entryFilter = new EntryFilter(src, dst);
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null || e.NewItems.Count == 0)
                    throw new ArgumentNullException();

                entryFilter.Filter(EntryFilterElement.Subject, key);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems == null || e.OldItems.Count == 0)
                    throw new ArgumentNullException();

                foreach (SubjectEntry entry in e.OldItems)
                {
                    if (dst.Contains(entry))
                        dst.Remove(entry);
                }
            }
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
            this.Hide();
            //this.Close();

            //Trace.WriteLine("\n\n================================================================================================");

            //if (FilteredEntries != null)
            //{
            //    Trace.WriteLine("\n\n=================== Level 1 =====================");
            //    foreach (SubjectEntry s in FilteredEntries)
            //        Trace.WriteLine($"{s.SubjectInfo.OwnerSection.Name}: {s.RoomAllocated.Name} \t {s.SubjectInfo.Name} ==== {s.DayAssigned}");
            //}

            //if (_FilterLevel2 != null)
            //{
            //    Trace.WriteLine("\n\n=================== Level 2 =====================");
            //    foreach (SubjectEntry s in _FilterLevel2)
            //        Trace.WriteLine($"{s.SubjectInfo.OwnerSection.Name}: {s.RoomAllocated.Name} \t {s.SubjectInfo.Name} ==== {s.DayAssigned}");
            //}

            //if (_FilterLevel3 != null)
            //{
            //    Trace.WriteLine("\n\n=================== Level 3 =====================");
            //    foreach (SubjectEntry s in _FilterLevel3)
            //        Trace.WriteLine($"{s.SubjectInfo.OwnerSection.Name}: {s.RoomAllocated.Name} \t {s.SubjectInfo.Name} ==== {s.DayAssigned}");
            //}
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

            if (tb_SearchByFilter != null)
            {
                if (chosenFilter == EntryFilterElement.None)
                {
                    tb_SearchByFilter.IsEnabled = false;
                    IsSecondLevelOn = false;
                    FilterLevel2 = null;

                    if (tb_SearchSubject != null && lv_SubjectsList != null && _FilterLevel3 != null)
                    {
                        if (!tb_SearchSubject.Text.Equals(String.Empty))
                        {
                            lv_SubjectsList.ItemsSource = _FilterLevel3;
                            new EntryFilter(FilteredEntries, _FilterLevel3).Filter(EntryFilterElement.Subject, tb_SearchSubject.Text);
                        }
                    }
                }
                else
                    tb_SearchByFilter.IsEnabled = true;
            }
            

            _FilterElement = chosenFilter;
        }

        private void tb_SearchByFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string key = tb_SearchByFilter.Text;
            string subKey = tb_SearchSubject.Text;

            if (key.Equals(String.Empty))
            {
                IsSecondLevelOn = false;
                _FilterLevel2 = null;
                if (!subKey.Equals(String.Empty) && _FilterLevel3 != null)
                {
                    lv_SubjectsList.ItemsSource = _FilterLevel3;
                    new EntryFilter(FilteredEntries, _FilterLevel3).Filter(EntryFilterElement.Subject, tb_SearchSubject.Text);
                    return;
                }
                else
                {
                    lv_SubjectsList.ItemsSource = FilteredEntries;
                    //IsSecondLevelOn = false;
                    //FilterLevel2 = null;
                    return;
                }
            }

            if (FilterLevel2 == null) 
                FilterLevel2 = new ObservableCollection<SubjectEntry>(FilteredEntries);

            EntryFilter efLevel2 = new EntryFilter(FilteredEntries, FilterLevel2);
            efLevel2.Filter(_FilterElement, key);
            IsSecondLevelOn = true;

            if (!subKey.Equals(String.Empty) && _FilterLevel3 != null)
            {
                _FilterLevel3.Clear();
                lv_SubjectsList.ItemsSource = _FilterLevel3;
                EntryFilter ef = new EntryFilter(FilterLevel2, _FilterLevel3);
                ef.Filter(EntryFilterElement.Subject, subKey);
            }
            else
                lv_SubjectsList.ItemsSource = FilterLevel2;
        }

        private void tb_SearchSubject_TextChanged(object sender, TextChangedEventArgs e)
        {
            string subKey = tb_SearchSubject.Text;
            string filterByKey = tb_SearchByFilter.Text;

            if (subKey.Equals(String.Empty))
            {
                if (IsSecondLevelOn)
                    lv_SubjectsList.ItemsSource = FilterLevel2;
                else
                    lv_SubjectsList.ItemsSource = FilteredEntries;
                _FilterLevel3 = null;
                return;
            }

            if (_FilterLevel3 == null) 
            {
                _FilterLevel3 = new ObservableCollection<SubjectEntry>(FilterLevel2 == null ? FilteredEntries : FilterLevel2);
            }

            lv_SubjectsList.ItemsSource = _FilterLevel3;

            EntryFilter? efNewLevel = null;
            ObservableCollection<SubjectEntry>? src = FilterLevel2 != null ? FilterLevel2 : FilteredEntries;
            ObservableCollection<SubjectEntry>? newFilterLevel = _FilterLevel3;

            if (src == null || newFilterLevel == null)
                throw new ArgumentNullException();

            efNewLevel = new EntryFilter(src, newFilterLevel);
            efNewLevel.Filter(EntryFilterElement.Subject, subKey);

        }
    }
}
