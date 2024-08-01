using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
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

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for RoomAddForm.xaml
    /// </summary>
    public partial class RoomAddForm : Window, INotifyPropertyChanged
    {
        private string _RoomName;
        private RoomType _ChosenRoomType;
        private ObservableCollection<Room> _Rooms;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string RoomName
        {
            get => _RoomName;
        }

        public RoomType RoomType
        {
            get => _ChosenRoomType;
            set
            {
                _ChosenRoomType = value;
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

        public RoomAddForm(ObservableCollection<RoomType> types, ObservableCollection<Room> rooms) 
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Owner = Application.Current.MainWindow;
            this.DataContext = this;
            _Rooms = rooms;

            cb_Type.ItemsSource = types;
        }

        public RoomAddForm(Room room, ObservableCollection<RoomType> types, ObservableCollection<Room> rooms)
        {
            InitializeComponent();
            this.DataContext = this;
            this.ShowInTaskbar = false;
            this.Owner = Application.Current.MainWindow;
            _Rooms = rooms;
            _RoomName = room.Name;

            cb_Type.ItemsSource = types;
            tbx_Name.Text = room.Name;
            Loaded += (sender, e) =>
            {
                RoomType = room.Type;
            };
        }

        private bool isNameExists(string name)
        {
            foreach(Room room in _Rooms)
            {
                if (room.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (isNameExists(tbx_Name.Text) && !tbx_Name.Text.Equals(_RoomName, StringComparison.CurrentCultureIgnoreCase))
            {
                new MBox("Name already exists.").ShowDialog();
                return;
            }

            RoomType selectedType = (RoomType) cb_Type.SelectedItem;
            if (tbx_Name.Text.Equals(String.Empty))
            {
                new MBox("Please fill up the Name field.").ShowDialog();
                return;
            }
            else if (selectedType == null || selectedType.Equals("Choose a value..."))
            {
                new MBox("Please select a type.").ShowDialog();
                return;
            }

            _RoomName = tbx_Name.Text;
            _ChosenRoomType = selectedType;

            DialogResult = true;
        }

        
    }
}
