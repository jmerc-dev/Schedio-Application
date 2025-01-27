using Schedio_Application.MVVM.View.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for SubjectItem.xaml
    /// </summary>
    public partial class SubjectItem : UserControl, INotifyPropertyChanged
    {
        private Subject _Subject;
        private double _XPosition, _YPosition;

        public double XPosition
        {
            get { return _XPosition; }
            set 
            { 
                _XPosition = value;
                OnPropertyChanged();
            }
        }

        public double YPosition
        {
            get { return _YPosition; }
            set 
            { 
                _YPosition = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Subject Subject 
        { 
            get { return _Subject; } 
        }

        public double Units
        {
            get => Subject.Units;
            set
            {
                Subject.Units = value;
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

        // TODO: Bind to counter
        public SubjectItem(Subject subject, ObservableCollection<Person> people, ObservableCollection<RoomType> roomTypes)
        {
            InitializeComponent();
            _Subject = subject;
            this.DataContext = subject;
            cbox_Personnel.ItemsSource = people;
            cbox_RoomType.ItemsSource = roomTypes;

            Loaded += (sender, e) =>
            {
                ctr_Units.DataContext = subject;
            };
        }

        public SubjectItem(ObservableCollection<string> people, ObservableCollection<string> roomTypes)
        {       
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                cbox_Personnel.ItemsSource = people;
                cbox_RoomType.ItemsSource = roomTypes;
                tb_Name.Focus();
            };
        }

        public bool ValidateEntries()
        {
            if (Subject.Name == null || Subject.Name.Equals(String.Empty))
            {
                new MBox("Name cannot be empty").ShowDialog();
                return false;
            }

            if (Subject.AssignedPerson == null)
            {
                new MBox("Assigned Person cannot be empty").ShowDialog();
                return false;
            }

            if (Subject.RoomType == null || Subject.RoomType.Equals(String.Empty))
            {
                new MBox("Room Type cannot be empty").ShowDialog();
                return false;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Delete
            foreach (SubjectEntry si in Subject.SubjectEntries)
            {
                if (si.SubjectInfo == Subject)
                {
                    new MBox("This subject has been allocated in the workshop.").ShowDialog();
                    return;
                }
            }
            Subject.OwnerSection.Subjects.Remove(Subject);
            ((StackPanel)this.Parent).Children.Remove(this);
        }
    }
}
