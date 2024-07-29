using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for SubjectItem.xaml
    /// </summary>
    public partial class SubjectItem : UserControl
    {

        private Subject _Subject; 


        public int MyProperty { get; set; }

        // Implement binding here for easy retrieval of data
        public SubjectItem(Subject subject, ObservableCollection<Person> people, ObservableCollection<string> roomTypes)
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                cbox_Personnel.ItemsSource = people;
                cbox_RoomType.ItemsSource = roomTypes;
            };
        }

        public SubjectItem(ObservableCollection<string> people, ObservableCollection<string> roomTypes)
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                cbox_Personnel.ItemsSource = people;
                cbox_RoomType.ItemsSource = roomTypes;

            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((StackPanel)this.Parent).Children.Remove(this);
        }
    }
}
