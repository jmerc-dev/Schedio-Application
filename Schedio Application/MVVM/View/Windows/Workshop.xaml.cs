﻿using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Schedio_Application.MVVM.View.Windows
{
    /// <summary>
    /// Interaction logic for Workshop.xaml
    /// </summary>
    public partial class Workshop : Window, INotifyPropertyChanged
    {

        private ClassSection? _SelectedSection;

        public ClassSection? SelectedSection
        {
            get { return _SelectedSection; }
            set 
            { 
                _SelectedSection = value;
                OnPropertyChanged();
            } 
        }
        
        public string SelectedSectionNull
        {
            get { return "No Chosen Section"; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Workshop()
        {
            _Rooms = new ObservableCollection<Room>();
            Personnel = new ObservableCollection<Person>();
            Sections = new ObservableCollection<ClassSection>();

            InitializeComponent();
            lv_RoomsList.ItemsSource = this.Rooms;
            lv_PersonnelList.ItemsSource = this.Personnel;
            lv_SectionList.ItemsSource = this.Sections;

            Rooms.CollectionChanged += new NotifyCollectionChangedEventHandler(room_CollectionChanged);

            Loaded += (sender, e) =>
            {
                this.DataContext = this;
            };

            Sections.CollectionChanged += new NotifyCollectionChangedEventHandler(section_CollectionChanged);

            Closing += (sender, e) =>
            {
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            };
        }

        // Subjects panel
        private void btn_ShowSubjects_Click(object sender, RoutedEventArgs e)
        {
            border_Subjects.Visibility = Visibility.Visible;
            btn_ShowSubjects.Visibility = Visibility.Collapsed;
        }

        private void btn_HideSubjects_Click(object sender, RoutedEventArgs e)
        {
            border_Subjects.Visibility = Visibility.Collapsed;
            btn_ShowSubjects.Visibility = Visibility.Visible;
        }

        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            foreach (ClassSection section in Sections)
            {
                Trace.WriteLine($"{section.Name} = {SelectedSection.Name} : {section == SelectedSection}");
            }

            SelectedSection = null;
        }

        private void btn_BrowseSectionExplorer_Click(object sender, RoutedEventArgs e)
        {
            SectionExplorer sectionExplorer = new SectionExplorer(Sections);
            
            if (sectionExplorer.ShowDialog() == true)
            {
                // Save selected Section
                SelectedSection = sectionExplorer.SelectedSection;
            }



        }
    }




    // Schedule Data Management
    public partial class Workshop : Window
    {
        private ObservableCollection<Room> _Rooms;
        private ObservableCollection<Room> TempRooms;
        private ObservableCollection<RoomType> RoomTypes;

        public ObservableCollection<Room> Rooms 
        { 
            get { return _Rooms; }
            set { _Rooms = value; }
        }

        private ObservableCollection<Person> Personnel;
        private ObservableCollection<Person> TempPersonnel;

        private ObservableCollection<ClassSection> Sections;
        private ObservableCollection<ClassSection> TempSections;

        private WarningConfirmation? warningModal;

        private void tabCtrl_DataManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeTabImage();
            if (e.Source is TabControl)
            {
                clearListViewSelectedItems(lv_PersonnelList);
                clearListViewSelectedItems(lv_RoomsList);
                clearListViewSelectedItems(lv_SectionList);
            }
        }

        private void clearListViewSelectedItems(ListView lv)
        {
            lv.SelectedItems.Clear();
        }

        private void changeTabImage()
        {
            TabItem[] tabItems = { tabItem_Personnel, tabItem_Rooms, tabItem_Sections };

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

        // Personnel Related Functions
        private void btn_AddPersonnel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Person newPerson = new Person();
            PersonnelAddForm form = new PersonnelAddForm(newPerson, Personnel);

            if (form.ShowDialog() == true)
            {
                this.Personnel.Add(form.Person);
            }
        }

        // Rooms Related Functions
        private void btn_SetupRoomTypes_Click(object sender, RoutedEventArgs e)
        {
            RoomTypeSetup rts;

            if (RoomTypes == null)
            {
                RoomTypes = new ObservableCollection<RoomType>();
            }

            rts = new RoomTypeSetup(RoomTypes, Rooms, Sections);
            rts.ShowDialog();
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

        private void room_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // Add vertical lines
                foreach (TabItem tabItem in tabCtrl_DayTimeTableContainer.Items)
                {
                    if (tabItem.Content.GetType() == typeof(TimeTable))
                    {
                        TimeTable timeTable = (TimeTable)tabItem.Content;
                        timeTable.addVerticalLine();
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // Remove vertical lines
                foreach (TabItem tabItem in tabCtrl_DayTimeTableContainer.Items)
                {
                    if (tabItem.Content.GetType() == typeof(TimeTable))
                    {
                        TimeTable timeTable = (TimeTable)tabItem.Content;
                        timeTable.removeVerticalLine();
                    }
                }
            }
        }

        // Section Related Functions
        private void btn_AddSections_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (RoomTypes == null || RoomTypes.Count == 0)
            {
                new MBox("Please setup the room types first").ShowDialog();
                return;
            }
            if (Personnel.Count == 0)
            {
                new MBox("Setup personnel first").ShowDialog();
                return;
            }

            SectionAddForm form = new SectionAddForm(new ClassSection(), Personnel, RoomTypes, Sections);

            if (form.ShowDialog() == true)
            {
                this.Sections.Add(form._Section);
            }
        }

        private void section_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Removed
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ClassSection s in Sections)
                {
                    if (s == SelectedSection)
                    {
                        return;
                    }
                }

                SelectedSection = null;
            }
        }

        // Search function
        private void TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchBox = (TextBox)sender;
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
            else if (searchBox.Name.Equals("tb_SearchSection"))
            {
                TempSections = new ObservableCollection<ClassSection>();
                if (!searchBox.Text.Equals(String.Empty))
                {
                    foreach (ClassSection section in Sections)
                    {
                        if (section.Name.StartsWith(searchBox.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            TempSections.Add(section);
                        }
                    }

                    lv_SectionList.ItemsSource = TempSections;
                }
                else
                {
                    lv_SectionList.ItemsSource = Sections;
                }
            }
        }

        // Deletion
        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            switch (tabCtrl_DataManager.SelectedIndex)
            {
                case 0:
                    // Check for references in sections
                    foreach (ClassSection cs in Sections)
                    {
                        foreach (Subject s in cs.Subjects)
                        {
                            if (lv_PersonnelList.SelectedItems.Contains(s.AssignedPerson))
                            {
                                new MBox($"{s.AssignedPerson.Name} was currently being referenced by Section: {cs.Name}, Subject: {s.Name}").ShowDialog();
                                return;
                            }
                        }
                    }
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
                    if (DeleteItemFrom(lv_SectionList, typeof(ClassSection)))
                    {
                        tb_SearchSection.Text = String.Empty;
                        break;
                    };
                    break;
                default:
                    new MBox("No tab selected.").ShowDialog();
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
            else if (itemType == typeof(ClassSection))
            {
                foreach (ClassSection item in lv.SelectedItems)
                {
                    objectsToBeRemoved.Add(item.Name);
                }
            }

            // Warning
            warningModal = new WarningConfirmation(specificType, objectsToBeRemoved); ;
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
                else if (itemType == typeof(ClassSection))
                {
                    return RemoveItems<ClassSection>(Sections, lv.SelectedItems);
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

        // Editing
        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            switch (tabCtrl_DataManager.SelectedIndex)
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
                    if (lv_SectionList.SelectedItems.Count == 0)
                    {
                        new MBox("Please select an item.").ShowDialog();
                    }
                    else if (lv_SectionList.SelectedItems.Count != 1)
                    {
                        new MBox("Editing is unavailable for multiple items.").ShowDialog();
                    }
                    else
                    {
                        // Update
                        ClassSection section = (ClassSection)lv_SectionList.SelectedItem;
                        SectionAddForm updateForm = new SectionAddForm(section, Personnel, RoomTypes, Sections);
                        if (updateForm.ShowDialog() == true)
                        {

                        }
                    }
                    break;
                default:
                    new MBox("No Tab selected").ShowDialog();
                    return;
            }
        }
    }
}
