using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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

namespace Schedio_Application.MVVM.View.Pages
{
    /// <summary>
    /// Interaction logic for NewPage.xaml
    /// </summary>
    /// 
    public partial class NewPage : Page
    {
        private Dictionary<DayOfWeek, TimeFrame> BaseSchedule;

        private ObservableCollection<Room> Rooms;
        private ObservableCollection<Room> TempRooms;
        private ObservableCollection<string> RoomTypes;

        private ObservableCollection<Person> Personnel;
        private ObservableCollection<Person> TempPersonnel;

        private WarningConfirmation? warningModal;

        public NewPage()
        {
            this.Rooms = new ObservableCollection<Room>();
            this.Personnel = new ObservableCollection<Person>();
            InitializeComponent();
            
            lv_RoomsList.ItemsSource = this.Rooms;
            lv_PersonnelList.ItemsSource = this.Personnel;
            this.DataContext = this;
        }
        
        private void tabCntrl_NewPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                clearListViewSelectedItems(lv_PersonnelList);
                clearListViewSelectedItems(lv_RoomsList);
                clearListViewSelectedItems(lv_SectionList);
            }
            changeTabImage();
        }

        private void clearListViewSelectedItems(ListView lv)
        {
            lv.SelectedItems.Clear();
        }

        private void changeTabImage()
        {
            TabItem[] tabItems = { tabItem_Personnel, tabItem_Rooms, tabItem_Sections};

            for (int i = 0; i < tabItems.Length; i++)
            {
                String tabItemName = tabItems[i].Name.ToString().Substring(8, tabItems[i].Name.Length - 8);

                if (tabItems[i].IsSelected)
                {
                    ((Image)tabItems[i].FindName("img_" + tabItemName)).Source = new BitmapImage(new Uri(String.Format("pack://application:,,,/Schedio Application;component/Resources/Images/{0}-filled.png", tabItemName.ToLower())));
                }
                else
                {
                    ((Image)tabItems[i].FindName("img_" + tabItemName)).Source = new BitmapImage(new Uri(String.Format("pack://application:,,,/Schedio Application;component/Resources/Images/{0}.png", tabItemName.ToLower())));
                }
            }
        }

        // Add button will be manipulated according to selected Tab Item
        private void ChangeAddButton(SecondaryButton button)
        {
            TabItem[] tabItems = { tabItem_Personnel, tabItem_Rooms, tabItem_Sections};

            for (int i = 0;i < tabItems.Length;i++)
            {
                String tabItemName = tabItems[i].Name.ToString().Substring(8, tabItems[i].Name.Length - 8);
       
                if (tabItems[i].IsSelected)
                {
                    if (tabItemName.Equals("Time"))
                    {
                        button.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        button.Visibility = Visibility.Visible;
                    }
                }
            }
        }


        private void btn_AddPersonnel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Person newPerson = new Person();
            PersonnelAddForm form = new PersonnelAddForm(newPerson, Personnel);

            if (form.ShowDialog() == true)
            {
                this.Personnel.Add(form.Person);
            }
        }

        private void btn_AddRooms_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (RoomTypes == null || RoomTypes.Count < 1)
            {
                new MBox("Please setup the room types first").ShowDialog();
                return;
            }

            RoomAddForm form = new RoomAddForm(RoomTypes, Rooms);
            if (form.ShowDialog() == true)
            {
                this.Rooms.Add(new Room(form.RoomName, form.RoomType));
            }
        }

        private void btn_AddSections_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SectionAddForm form = new SectionAddForm();
            form.ShowInTaskbar = false;
            form.Owner = Application.Current.MainWindow;
            form.ShowDialog();
        }

        private void btn_BaseSchedule_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TimeScheduleAddForm form = new TimeScheduleAddForm();
            form.Owner = Application.Current.MainWindow;
            form.ShowDialog();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            switch (tabCntrl_NewPage.SelectedIndex)
            {
                case 0:
                    if (DeleteItemFrom(lv_PersonnelList, typeof(Person))) 
                    {
                        tb_SearchPersonnel.Text = String.Empty;
                    }
                    
                    break;
                case 1:
                    if (DeleteItemFrom(lv_RoomsList, typeof(Room)))
                    {
                        tb_SearchRooms.Text = String.Empty;
                        break;
                    };
                    break;
                case 2:
                    break;
                default:
                    MessageBox.Show("No tab selected.");
                    return;
            }
        }

        private bool DeleteItemFrom(ListView lv, Type itemType)
        {
            if (lv.SelectedItems.Count == 0)
            {
                return false;
            }

            // Type
            string[] fullType = itemType.ToString().Split(".");
            string specificType = fullType[fullType.Length - 1];

            // Prepare list of objects to be removed
            List<string> objectsToBeRemoved = new List<string>();

            if (itemType == typeof(Person))
            {
                foreach (Person item in lv.SelectedItems)
                {
                    objectsToBeRemoved.Add(item.Name);
                }
            }
            else if (itemType == typeof(Room))
            {
                foreach (Room item in lv.SelectedItems)
                {
                    objectsToBeRemoved.Add(item.Name);
                }
            }
            // TODO: section elseif


            // Warning
            warningModal = new WarningConfirmation(specificType, objectsToBeRemoved);;
            warningModal.Owner = Application.Current.MainWindow;

            // Remove
            if (warningModal.ShowDialog() == true)
            {
                if (itemType == typeof(Person))
                {
                    return RemoveItems<Person>(Personnel, lv.SelectedItems);
                }
                else if (itemType == typeof(Room))
                {
                    return RemoveItems<Room>(Rooms, lv.SelectedItems);
                }
            }
            return false;
        }

        private bool RemoveItems<T>(ObservableCollection<T> source, IList valuesToBeDeleted)
        {
            List<T> toBeRemoved = new List<T>();
            foreach (T item in valuesToBeDeleted)
            {
                toBeRemoved.Add(item);
            }

            foreach (T item in toBeRemoved)
            {
                source.Remove(item);
            }

            return true;
        }

        private void tabItem_LostFocus(object sender, RoutedEventArgs e)
        {
            string name = ((TabItem)sender).Name;

            if (name.Equals("tabItem_Personnel"))
            {
                clearListViewSelectedItems(lv_PersonnelList);
            }
            else if (name.Equals("tabItem_Rooms"))
            {
                clearListViewSelectedItems(lv_RoomsList);
            }
            else if (name.Equals("tabItem_Sections"))
            {
                clearListViewSelectedItems(lv_SectionList);
            }
        }

        private void TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchBox = (TextBox) sender;
            if (searchBox.Name.Equals("tb_SearchRooms"))
            {
                TempRooms = new ObservableCollection<Room>();

                if (!searchBox.Text.Equals(String.Empty))
                {
                    foreach (Room room in Rooms)
                    {
                        if (room.Name.StartsWith(searchBox.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            TempRooms.Add(room);
                        }
                    }

                    lv_RoomsList.ItemsSource = TempRooms;
                }
                else
                {
                    lv_RoomsList.ItemsSource = Rooms;
                }
            }
            else if (searchBox.Name.Equals("tb_SearchPersonnel"))
            {
                TempPersonnel = new ObservableCollection<Person>();

                if (!searchBox.Text.Equals(String.Empty))
                {
                    foreach (Person person in Personnel)
                    {
                        if (person.Name.StartsWith(searchBox.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            TempPersonnel.Add(person);
                        }
                    }

                    lv_PersonnelList.ItemsSource = TempPersonnel;
                }
                else
                {
                    lv_PersonnelList.ItemsSource = Personnel;
                }
            }
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            switch (tabCntrl_NewPage.SelectedIndex)
            {
                case 0:
                    // Personnel
                    if (lv_PersonnelList.SelectedItems.Count == 0)
                    {
                        new MBox("Please select an item.").ShowDialog();
                    }
                    else if (lv_PersonnelList.SelectedItems.Count != 1)
                    {
                        new MBox("Editing is unavailable for multiple items.").ShowDialog();
                    }
                    else
                    {
                        Person person = (Person)lv_PersonnelList.SelectedItem;
                        PersonnelAddForm form = new PersonnelAddForm(person, Personnel);

                        if (form.ShowDialog() == true)
                        {

                        }
                    }
                    break;
                case 1:
                    // Rooms
                    if (lv_RoomsList.SelectedItems.Count == 0)
                    {
                        new MBox("Please select an item.").ShowDialog();
                    }
                    else if (lv_RoomsList.SelectedItems.Count != 1)
                    {
                        new MBox("Editing is unavailable for multiple items.").ShowDialog();
                    }
                    else
                    {
                        // Update
                        Room room = (Room)lv_RoomsList.SelectedItem;
                        RoomAddForm roomAddForm = new RoomAddForm(room, RoomTypes, Rooms);
                        if (roomAddForm.ShowDialog() == true)
                        {
                            room.Name = roomAddForm.RoomName;
                            room.Type = roomAddForm.RoomType;
                        }
                    }
                    break;
                case 2:
                    break;
                default:
                    new MBox("No Tab selected").ShowDialog();
                    return;
            }
        }

        private void btn_SetupRoomTypes_Click(object sender, RoutedEventArgs e)
        {
            RoomTypeSetup rts;

            if (RoomTypes == null)
            {
                RoomTypes = new ObservableCollection<string>();
            }

            rts = new RoomTypeSetup(RoomTypes, Rooms);
            rts.ShowDialog();
        }
    }
}
