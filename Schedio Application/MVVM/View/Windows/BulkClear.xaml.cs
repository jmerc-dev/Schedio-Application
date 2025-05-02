using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for BulkClear.xaml
    /// </summary>
    public partial class BulkClear : Window
    {
        private readonly EntryCategorizer _EntryCategorizer;

        private ObservableCollection<DayBasedCategory>? _ListByDay;
        private ObservableCollection<EntryCategoryCounter>? _ListByEntryCategory;

        private ObservableCollection<DayBasedCategory>? _FilteredListByDay;
        private ObservableCollection<EntryCategoryCounter>? _FilteredListByEntryCategory;

        public bool IsDaySelected {  get; private set; }
        public bool IsClearAllSelected { get; private set; }
        public bool IsClearSelected { get; private set; }
        public ScheduleElement SelectedCategory { get; private set; }
        public ObservableCollection<DayBasedCategory>? ListToDeleteByDays { get; private set; }
        public ObservableCollection<EntryCategoryCounter>? ListToDeleteByEntryCategory { get; private set; }

        public BulkClear(ObservableCollection<SubjectEntry> entries)
        {
            InitializeComponent();

            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            _EntryCategorizer = new EntryCategorizer(entries);
            this.DataContext = this;

            cbox_Category.SelectedIndex = 0;
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)cbox_Category.SelectedValue;
            string? value = selectedItem.Content.ToString();

            if (value == null)
                return;

            ScheduleElement element;
            bool success = Enum.TryParse<ScheduleElement>(value, true, out element);

            if (!success)
                throw new InvalidEnumArgumentException();

            switch (element)
            {
                case ScheduleElement.Day:

                    _ListByDay = _EntryCategorizer.CategorizeByDay();
                    _ListByEntryCategory = null;

                    lv_CategorizedList.ItemsSource = _ListByDay;
                    IsDaySelected = true;
                    break;
                default:
                    _ListByEntryCategory = _EntryCategorizer.CategorizeBy(element);
                    lv_CategorizedList.ItemsSource = _ListByEntryCategory;
                    break;
            }

            SelectedCategory = element;
        }

        private void tb_SearchGroup_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_SearchGroup.Text.Equals("String.Empty"))
            {
                if (IsDaySelected)
                    lv_CategorizedList.ItemsSource = _ListByDay;
                else
                    lv_CategorizedList.ItemsSource = _ListByEntryCategory;
                return;
            }

            string toSearch = tb_SearchGroup.Text;

            if (lv_CategorizedList != null)
            {
                if (IsDaySelected && _ListByDay != null)
                {
                    _FilteredListByDay = new();
                    foreach (DayBasedCategory item in _ListByDay)
                    {
                        if (item.Name.Contains(toSearch))
                            _FilteredListByDay.Add(item);
                    }
                    lv_CategorizedList.ItemsSource = _FilteredListByDay;
                }
                else if (!IsDaySelected && _ListByEntryCategory != null)
                {
                    _FilteredListByEntryCategory = new();
                    foreach (EntryCategoryCounter item in _ListByEntryCategory)
                    {
                        if (item.Name == null)
                            continue;

                        if (item.Name.Contains(toSearch))
                            _FilteredListByEntryCategory.Add(item);
                    }
                    lv_CategorizedList.ItemsSource = _FilteredListByEntryCategory;
                }
            }
        }

        private void btn_ClearSelected_Click(object sender, RoutedEventArgs e)
        {
            IsClearSelected = true;
            IsClearAllSelected = false;

            if (IsDaySelected)
            {
                ListToDeleteByDays = new();
                foreach (DayBasedCategory dayBasedCategory in lv_CategorizedList.SelectedItems)
                {
                    ListToDeleteByDays.Add(dayBasedCategory);
                }
            }
            else
            {
                ListToDeleteByEntryCategory = new();
                foreach (EntryCategoryCounter entryCategoryCounter in lv_CategorizedList.SelectedItems)
                {
                    ListToDeleteByEntryCategory.Add(entryCategoryCounter);
                }
            }

            DialogResult = true;
        }

        //Refactor this to only clear those who are in list
        private void btn_ClearAll_Click(object sender, RoutedEventArgs e)
        {
            
            IsClearSelected = false;
            IsClearAllSelected = true;
            DialogResult = true;
            
        }
    }
}
