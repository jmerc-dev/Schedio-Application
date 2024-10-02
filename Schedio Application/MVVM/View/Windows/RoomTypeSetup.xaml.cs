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
        ObservableCollection<RoomType> RoomTypes;
        ObservableCollection<Room> Rooms;
        ObservableCollection<ClassSection> Sections;

        private RoomType _type;
        public RoomType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public RoomTypeSetup(ObservableCollection<RoomType> types, ObservableCollection<Room> rooms, ObservableCollection<ClassSection> sections)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Owner = Application.Current.MainWindow;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            RoomTypes = types;
            cbox_TypeNames.ItemsSource = RoomTypes;
            Rooms = rooms;
            Sections = sections;
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        public bool UpdateRoomCategory(string oldValue, string newValue)
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].Type.Equals(oldValue))
                {
                    Rooms[i].Type.Name = newValue;
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
            foreach (RoomType item in RoomTypes)
            {
                if (type.Equals(item.Name, StringComparison.CurrentCultureIgnoreCase)) 
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
                RoomTypes.Add(new RoomType(tb_ItemAdd.Text));   
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
            RoomType selectedItem = (RoomType) cbox_TypeNames.SelectedItem;

            if (selectedItem == null)
            {
                new MBox("Please choose an item to delete").ShowDialog();
                return;
            }

            foreach (Room room in Rooms)
            {
                if (room.Type == selectedItem)
                {
                    new MBox($"The {selectedItem.Name} is currently being referenced by room items.").ShowDialog();
                    return;
                }
            }

            foreach (ClassSection cs in Sections)
            {
                foreach (Subject s in cs.Subjects)
                {
                    if (s.RoomType == selectedItem)
                    {
                        new MBox($"{selectedItem.Name} is currently being referenced by Section: {cs.Name}, Subject: {s.Name}").ShowDialog();
                        return;
                    }
                }
            }

            if (RoomTypes.Contains(selectedItem))
            {
                RoomTypes.Remove(selectedItem);
                new MBox($"{selectedItem.Name} was successfully deleted", Sound.NoSound).ShowDialog();
            }
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection <ClassSection> sectionz = Sections;
            RoomType? selectedItem = (RoomType) cbox_TypeNames.SelectedItem;
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
                if (new MBox($"This will affect all occurences of {selectedItem.Name}.", MBoxType.CancelOrOK).ShowDialog() == false)
                {
                    return;
                }
            }



            for (int i = 0; i < RoomTypes.Count; i++)
            {
                if (RoomTypes[i].Equals(selectedItem))
                {
                    RoomTypes[i].Name = newValue;
                    break;
                }
            }
            new MBox($"Successfylly updated all occurences of {selectedItem.Name}.").ShowDialog();
            tb_ItemNewValue.Clear();


        }
    }
}
