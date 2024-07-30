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
        private ObservableCollection<Person> _People;
        private ObservableCollection<string> _RoomTypes;
        private string _SectionName;

        // TODO: Adding subjects
        public string SectionName
        {
            get { return _SectionName; }
            set { _SectionName = value; }
        }

        public SectionAddForm(ClassSection section, ObservableCollection<Person> people, ObservableCollection<string> roomTypes)
        {
            InitializeComponent();
            _Section = section;
            this.Owner = Application.Current.MainWindow;
            this.ShowInTaskbar = false;

            _People = people;
            _Section = section;
            _RoomTypes = roomTypes;


            tb_Name.Focus();
        }

        private void btn_AddSubject_Click(object sender, RoutedEventArgs e)
        {   
            sp_SubjectList.Children.Add(new SubjectItem(new Subject(), _People, _RoomTypes)) ;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            _Section.Subjects.Clear();
            foreach (SubjectItem subitem in sp_SubjectList.Children)
            {

                if (subitem.ValidateEntries())
                {
                    _Section.Subjects.Add(subitem.Subject);
                }
                else
                {
                    return;
                }
            }
            _Section.Name = SectionName;
            DialogResult = true;
        }
    }
}
