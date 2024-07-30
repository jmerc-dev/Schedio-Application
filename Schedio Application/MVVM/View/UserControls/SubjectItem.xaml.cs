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

        public event PropertyChangedEventHandler? PropertyChanged;

        public Subject Subject 
        { 
            get { return _Subject; } 
        }

        public int Units
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
        public SubjectItem(Subject subject, ObservableCollection<Person> people, ObservableCollection<string> roomTypes)
        {
            InitializeComponent();
            _Subject = subject;
            this.DataContext = subject;
            cbox_Personnel.ItemsSource = people;

            Loaded += (sender, e) =>
            {
                ctr_Units.DataContext = subject;
                cbox_RoomType.ItemsSource = roomTypes;
                tb_Name.Focus();
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
            if (Subject.Name.Equals(String.Empty))
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
            ((StackPanel)this.Parent).Children.Remove(this);
        }
    }
}
