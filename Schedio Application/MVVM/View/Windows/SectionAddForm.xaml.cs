using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for SectionAddForm.xaml
    /// </summary>
    /// 
    //TODO: Redesign & fix binding  
    public partial class SectionAddForm : Window, INotifyPropertyChanged
    {
        public ClassSection _Section;
        private ObservableCollection<Person> _People;
        private ObservableCollection<ClassSection> _Sections;
        private ObservableCollection<RoomType> _RoomTypes;
        private string _SectionName;

        public event PropertyChangedEventHandler? PropertyChanged;

        // TODO: Adding subjects
        public string SectionName
        {
            get { return _SectionName; }
            set 
            { 
                _SectionName = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public SectionAddForm(ClassSection section, ObservableCollection<Person> people, ObservableCollection<RoomType> roomTypes, ObservableCollection<ClassSection> sections)
        {
            InitializeComponent();
            _Sections = sections;
            _Section = section;
            this.Owner = Application.Current.MainWindow;
            this.ShowInTaskbar = false;

            _People = people;
            _Section = section;
            _RoomTypes = roomTypes;
            tb_Name.Focus();

            Loaded += (sender, e) =>
            {
                if (_Section.Name != null)
                {
                    SectionName = _Section.Name;

                    foreach (Subject sub in _Section.Subjects)
                    {
                        sp_SubjectList.Children.Add(new SubjectItem(new Subject(sub, _Section), _People, _RoomTypes));
                    }
                }
            };
        }

        private void btn_AddSubject_Click(object sender, RoutedEventArgs e)
        {   
            sp_SubjectList.Children.Add(new SubjectItem(new Subject(_Section), _People, _RoomTypes)) ;
        }

        private bool IsNameExist(string name)
        {
            foreach (ClassSection cs in _Sections)
            {
                if (cs.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)   
        {
            ObservableCollection<Subject> newSubjects = new ObservableCollection<Subject>();
            // Validations
            if (_Section.Name != null && !_Section.Name.Equals(SectionName))
            {
                if (IsNameExist(SectionName))
                {
                    new MBox("Name already exist.").ShowDialog();
                    return;
                }
            }

            if (SectionName.Equals(String.Empty))
            {
                new MBox("Name cannot be empty.").ShowDialog();
                return;
            }

            
            foreach (SubjectItem subitem in sp_SubjectList.Children)
            {

                if (subitem.ValidateEntries())
                {
                    newSubjects.Add(subitem.Subject);
                }
                else
                {
                    return;
                }
            }

            _Section.Subjects = newSubjects;
            _Section.Name = SectionName;
            DialogResult = true;
        }
    }
}
