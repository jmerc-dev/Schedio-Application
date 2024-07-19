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

        private ObservableCollection<Room> Rooms;
        private ObservableCollection<Person> Personnel;
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
                Trace.WriteLine("cleared");
                
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
            PersonnelAddForm form = new PersonnelAddForm();
            form.ShowInTaskbar = false;
            form.Owner = Application.Current.MainWindow;
            if (form.ShowDialog() == true)
            {
                this.Personnel.Add(form.Person);
            }

            foreach (Person person in Personnel)
            {
                Debug.WriteLine(person.Name);
            }
        }

        private void btn_AddRooms_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RoomAddForm form = new RoomAddForm();
            form.ShowInTaskbar = false;
            form.Owner = Application.Current.MainWindow;
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

        private void btn_TimeSchedule_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TimeScheduleAddForm form = new TimeScheduleAddForm();
            form.ShowInTaskbar = false;
            form.Owner = Application.Current.MainWindow;
            form.ShowDialog();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {

            switch (tabCntrl_NewPage.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    DeleteItemFrom(lv_RoomsList, typeof(Room));
                    break;
                case 2:
                    break;
                default:
                    MessageBox.Show("No tab selected.");
                    return;
            }
            //WarningConfirmation modal = new WarningConfirmation(names,type);
            
            //if (modal.ShowDialog() == true)
            //{
            //    // Delete selected list view item
            //    if (tabItem_Personnel.IsSelected)
            //    {
                    
            //    }
            //    else if (tabItem_Rooms.IsSelected)
            //    {
            //        if (!RemoveItems<Room>(Rooms, lv_RoomsList.SelectedItems))
            //        {
            //            MessageBox.Show("Cannot delete items.");
            //        }
            //    }
            //    else if (tabItem_Sections.IsSelected)
            //    {

            //    }
            //}

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

            //string[] fullType_split = fullType.Split(".");
            //string type = fullType_split[fullType_split.Length - 1];

            // Prepare list of objects to be removed
            List<string> objectsToBeRemoved = new List<string>();

            if (itemType == typeof(Person))
            {

            }
            else if (itemType == typeof(Room))
            {
                foreach (Room item in lv.SelectedItems)
                {
                    objectsToBeRemoved.Add(item.Name);
                }
            }
            // section elseif


            // Warning
            warningModal = new WarningConfirmation(specificType, objectsToBeRemoved);
            warningModal.ShowInTaskbar = false;
            warningModal.Owner = Application.Current.MainWindow;

            // Remove
            if (warningModal.ShowDialog() == true)
            {
                if (itemType == typeof(Person))
                {

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
    }
}
