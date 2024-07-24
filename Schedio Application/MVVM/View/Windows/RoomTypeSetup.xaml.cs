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
        public RoomTypeSetup(ObservableCollection<string> types)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Owner = Application.Current.MainWindow;

            RoomTypes = types;
            cbox_TypeNames.ItemsSource = RoomTypes;

            cbox_TypeNames.SelectedIndex = 0;
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
            if (item.Equals(String.Empty) || item.Equals("Choose a value..."))
            {
                new MBox("Invalid input").ShowDialog();
                return;
            }

            if (!TypeExist(item))
            {
                RoomTypes.Add(tb_ItemAdd.Text);
                new MBox($"{item} is added", Sound.NoSound).ShowDialog();
            }
            else
            {
                new MBox($"{item} already exist").ShowDialog();
            }

        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            string? selectedItem = cbox_TypeNames.SelectedValue.ToString();

            if (selectedItem == null || selectedItem.Equals("Choose a value..."))
            {
                new MBox("Please choose an item to delete").ShowDialog();
                return;
            }

            if (RoomTypes.Contains(selectedItem))
            {
                RoomTypes.Remove(selectedItem);
                new MBox($"{selectedItem} was successfully deleted", Sound.NoSound).ShowDialog();
                cbox_TypeNames.SelectedIndex = 0;
            }
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            string? selectedItem = cbox_TypeNames.SelectedValue.ToString();
            if (selectedItem == null || selectedItem.Equals("Choose a value..."))
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

            for (int i = 0; i < RoomTypes.Count; i++)
            {
                if (RoomTypes[i].Equals(selectedItem))
                {
                    RoomTypes[i] = newValue;
                    new MBox($"{selectedItem} changed to {newValue}", Sound.NoSound).ShowDialog();
                    cbox_TypeNames.SelectedIndex = 0;
                    return;
                }
            }
        }
    }
}
