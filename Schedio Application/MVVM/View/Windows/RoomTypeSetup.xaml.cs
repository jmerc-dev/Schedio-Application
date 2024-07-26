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
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for RoomTypeSetup.xaml
    /// </summary>
    public partial class RoomTypeSetup : Window
    {
        ObservableCollection<string> RoomTypes;
        ObservableCollection<Room> Rooms;
        public RoomTypeSetup(ObservableCollection<string> types, ObservableCollection<Room> rooms)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Owner = Application.Current.MainWindow;

            RoomTypes = types;
            cbox_TypeNames.ItemsSource = RoomTypes;
            Rooms = rooms;
        }

        public bool UpdateRoomCategory(string oldValue, string newValue)
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].Type.Equals(oldValue))
                {
                    Rooms[i].Type = newValue;
                }
            }
            return true;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        private bool TypeExist(string type)
        {
            foreach (string item in RoomTypes)
            {
                if (type.Equals(item, StringComparison.CurrentCultureIgnoreCase)) 
                {
                    return true;
                }
            }
            return false;
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string item = tb_ItemAdd.Text;
            if (item.Equals(String.Empty))
            {
                new MBox("Invalid input").ShowDialog();
                return;
            }

            if (!TypeExist(item))
            {
                RoomTypes.Add(tb_ItemAdd.Text);
                new MBox($"{item} is added", Sound.NoSound).ShowDialog();
                tb_ItemAdd.Clear();
            }
            else
            {
                new MBox($"{item} already exist").ShowDialog();
            }

        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            string? selectedItem = cbox_TypeNames.SelectedValue.ToString();

            if (selectedItem == null)
            {
                new MBox("Please choose an item to delete").ShowDialog();
                return;
            }

            foreach (Room room in Rooms)
            {
                if (room.Type.Equals(selectedItem))
                {
                    new MBox($"The {selectedItem} is currently being referenced by room items.").ShowDialog();
                    return;
                }
            }

            if (RoomTypes.Contains(selectedItem))
            {
                RoomTypes.Remove(selectedItem);
                new MBox($"{selectedItem} was successfully deleted", Sound.NoSound).ShowDialog();
            }
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            string? selectedItem = cbox_TypeNames.SelectedValue.ToString();
            if (selectedItem == null)
            {
                new MBox("Please choose an item to update").ShowDialog();
                return;
            }

            string newValue = tb_ItemNewValue.Text;
            if (newValue.Equals(String.Empty))
            {
                new MBox("Please enter a new value").ShowDialog();
                return;
            } 
            else if (TypeExist(newValue))
            {
                new MBox($"{newValue} already exist").ShowDialog();
                return;
            }

            // Warning
            if (Rooms.Count > 0)
            {
                if (new MBox($"This will affect all occurences of {selectedItem}.", MBoxType.CancelOrOK).ShowDialog() == false)
                {
                    return;
                }
            }

            for (int i = 0; i < RoomTypes.Count; i++)
            {
                if (RoomTypes[i].Equals(selectedItem))
                {
                    RoomTypes[i] = newValue;
                    break;
                }
            }

            if (UpdateRoomCategory(selectedItem, newValue))
            {
                new MBox($"Successfylly updated all occurences of {selectedItem}.").ShowDialog();
                tb_ItemNewValue.Clear();
            }

        }
    }
}
