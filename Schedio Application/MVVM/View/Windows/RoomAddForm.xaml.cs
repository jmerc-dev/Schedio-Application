using System;
using System.Collections.Generic;
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
    /// Interaction logic for RoomAddForm.xaml
    /// </summary>
    public partial class RoomAddForm : Window
    {

        private string? _RoomName;
        private string? _ChosenRoomType;
        
        public string RoomName
        {
            get => _RoomName;
        }

        public string RoomType
        {
            get => _ChosenRoomType;
        }

        public RoomAddForm()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            _RoomName = tbx_Name.Text;
            _ChosenRoomType = cb_Type.Text;

            DialogResult = true;
        }
    }
}
