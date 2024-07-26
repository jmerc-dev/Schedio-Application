using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for RoomAddForm.xaml
    /// </summary>
    public partial class RoomAddForm : Window
    {
        private string _RoomName;
        private string _ChosenRoomType;
        private ObservableCollection<Room> _Rooms;
        
        public string RoomName
        {
            get => _RoomName;
        }

        public string RoomType
        {
            get => _ChosenRoomType;
        }

        public RoomAddForm(ObservableCollection<string> types, ObservableCollection<Room> rooms) 
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Owner = Application.Current.MainWindow;
            this.DataContext = this;
            _Rooms = rooms;

            cb_Type.ItemsSource = types;
        }

        public RoomAddForm(Room room, ObservableCollection<string> types, ObservableCollection<Room> rooms)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.Owner = Application.Current.MainWindow;
            _Rooms = rooms;

            _RoomName = room.Name;
            _ChosenRoomType = room.Type;

            cb_Type.ItemsSource = types;
            tbx_Name.Text = room.Name;
            cb_Type.Text = room.Type;
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

            string selectedType = cb_Type.SelectedItem.ToString();
            if (tbx_Name.Text.Equals(String.Empty))
            {
                MessageBox.Show("Please fill up the Name field.");
                return;
            }
            else if (selectedType.Equals(null) || selectedType.Equals("Choose a value..."))
            {
                MessageBox.Show("Please select a type.");
                return;
            }

            _RoomName = tbx_Name.Text;
            _ChosenRoomType = selectedType;

            DialogResult = true;
        }

        
    }
}
