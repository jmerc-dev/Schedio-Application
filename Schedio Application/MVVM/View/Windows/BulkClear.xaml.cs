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
        private ScheduleElement selectedCategory;

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
                    lv_CategorizedList.ItemsSource = _EntryCategorizer.CategorizeByDay();
                    break;
                default:
                    lv_CategorizedList.ItemsSource = _EntryCategorizer.CategorizeBy(element);
                    break;
            }

            selectedCategory = element;
        }
    }
}
