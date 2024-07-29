using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SectionAddForm.xaml
    /// </summary>
    public partial class SectionAddForm : Window
    {
        public ClassSection _Section;
        private ObservableCollection<string> _People;
        private ObservableCollection<string> _RoomTypes;

        // TODO: Adding subjects
        private string _Name;
        public string SectionName
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public SectionAddForm(ClassSection section, ObservableCollection<Person> people, ObservableCollection<string> roomTypes)
        {
            InitializeComponent();
            this.DataContext = this;
            _People = new ObservableCollection<string>();
            this.Owner = Application.Current.MainWindow;
            this.ShowInTaskbar = false;
            _Section = section;
            _RoomTypes = roomTypes;

            foreach(Person person in people)
            {
                _People.Add(person.Name);
            }
        }

        private void btn_AddSubject_Click(object sender, RoutedEventArgs e)
        {
            sp_SubjectList.Children.Add(new SubjectItem(_People, _RoomTypes)) ;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            _Section.Name = SectionName;
            DialogResult = true;
        }
    }
}
