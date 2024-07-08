using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
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

namespace Schedio_Application.MVVM.View.Pages
{
    /// <summary>
    /// Interaction logic for NewPage.xaml
    /// </summary>
    /// 
    public partial class NewPage : Page
    {

        private ObservableCollection<Room> Rooms;

        public NewPage()
        {
            this.Rooms = new ObservableCollection<Room>();
            InitializeComponent();
            
            lv_RoomsList.ItemsSource = this.Rooms;
            this.DataContext = this;
        }
        
        private void tabCntrl_NewPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeTabImage();
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
            form.ShowDialog();
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
            WarningConfirmation modal = new WarningConfirmation("", "");
            modal.ShowInTaskbar = false;
            modal.Owner = Application.Current.MainWindow;
            if (modal.ShowDialog() == true)
            {
                Trace.WriteLine("Deleted");
            }

        }
    }
}
