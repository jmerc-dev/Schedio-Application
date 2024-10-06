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
    // Make sure that editing subjects does not create new instance

    public enum State
    {
        New,
        Existing
    }

    public partial class SectionAddForm : Window, INotifyPropertyChanged
    {
        public ClassSection _Section;
        private ObservableCollection<Person> _People;
        private ObservableCollection<ClassSection> _Sections;
        private ObservableCollection<RoomType> _RoomTypes;
        private string _SectionName;
        private State _state;

        public event PropertyChangedEventHandler? PropertyChanged;

        // TODO: Adding subjects

        public ClassSection MySection
        {
            get { return _Section; }
        }

        public State FormState
        {
            get { return _state; }
            set 
            { 
                _state = value; 
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

        public SectionAddForm(ClassSection section, ObservableCollection<Person> people, ObservableCollection<RoomType> roomTypes, ObservableCollection<ClassSection> sections, State state)
        {
            InitializeComponent();
            _Sections = sections;
            _Section = section;
            grid_NameContainer.DataContext = section;
            this.Owner = Application.Current.MainWindow;
            _People = people;
            _Section = section;
            _RoomTypes = roomTypes;

            FormState = state;
            Loaded += (sender, e) =>
            {

                switch (state)
                {
                    case State.New:
                        break;
                    case State.Existing:
                        foreach (Subject sub in _Section.Subjects)
                        {
                            sp_SubjectList.Children.Add(new SubjectItem(sub, _People, _RoomTypes));
                        }
                        break;
                    default:
                        break;
                }


                if (MySection.Name == null)
                {
                    tb_Name.Focus();
                }
                else
                {
                    tb_Name.IsEnabled = false;
                    Keyboard.ClearFocus();
                }
            };
        }

        private void btn_AddSubject_Click(object sender, RoutedEventArgs e)
        {
            Subject newSubject = new Subject(_Section);
            MySection.Subjects.Add(newSubject);
            sp_SubjectList.Children.Add(new SubjectItem(newSubject, _People, _RoomTypes)) ;
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
            if (IsNameExist(tb_Name.Text) && !tb_Name.Text.Equals(MySection.Name))
            {
                new MBox("Name already exist.").ShowDialog();
                MySection.Name = string.Empty;
                return;
            }

            if (MySection.Name == null || MySection.Name.Equals(String.Empty))
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
            DialogResult = true;
        }

        private void btn_EditSectionName_Click(object sender, RoutedEventArgs e)
        {
            tb_Name.IsEnabled = true;
            tb_Name.Focus();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            tb_Name.IsEnabled = false;
        }

        private void tb_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            tb_Name.IsEnabled = false;
        }

        private void tb_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                tb_Name.IsEnabled = false;
            }
        }

        private void DisplaySubjectData_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(MySection.Name);
            foreach (Subject s in _Section.Subjects)
            {
                Trace.WriteLine(s.Name);
                Trace.WriteLine(s.Units);
                Trace.WriteLine(s.AssignedPerson.Name);
                Trace.WriteLine(s.RoomType.Name);
            }

            //Trace.WriteLine(MySection.Name);
        }

        private void tb_Name_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (IsNameExist(tb_Name.Text) && !tb_Name.Text.Equals(MySection.Name))
            {
                new MBox("Name already exist.").ShowDialog();
                MySection.Name = MySection.Name;
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (MySection.Name.Equals(String.Empty))
            {
                new MBox("Name cannot be empty.").ShowDialog();
                return;
            }

            foreach (SubjectItem subitem in sp_SubjectList.Children)
            {

                if (!subitem.ValidateEntries())
                {
                    return;
                }
            }

            DialogResult = true;
        }
    }
}
