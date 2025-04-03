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
    /// Interaction logic for SectionExplorer.xaml
    /// </summary>
    public partial class SectionExplorer : Window
    {
        ObservableCollection<ClassSection> Sections;
        ObservableCollection<ClassSection> SectionsTemp;

        private bool _ForManaging;

        public ClassSection SelectedSection;
        public SectionExplorer(ObservableCollection<ClassSection> sections)
        {
            InitializeComponent();
            this.Sections = sections;
            lv_SectionSimpleList.ItemsSource = this.Sections;
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
            if (lv_SectionSimpleList.SelectedItems.Count == 1)
            {
                SelectedSection = (ClassSection) lv_SectionSimpleList.SelectedItem;
                DialogResult = true;
            }
            else
            {
                new MBox("Select only one (1) section.").ShowDialog();
            }
        }

        private void tb_SearchSection_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_SearchSection.Text.Equals(String.Empty))
            {
                lv_SectionSimpleList.ItemsSource = this.Sections;
                return;
            } 
            else
            {
                SectionsTemp = new ObservableCollection<ClassSection>();

                foreach (ClassSection cs in Sections)
                {
                    if (cs.Name.Contains(tb_SearchSection.Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SectionsTemp.Add(cs);
                    }
                }

                lv_SectionSimpleList.ItemsSource = SectionsTemp;

            }


        }
    }
}
