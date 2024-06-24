using System;
using System.Collections.Generic;
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

namespace Schedio_Application.MVVM.View.UserControls
{
    /// <summary>
    /// Interaction logic for SecondaryButton.xaml
    /// </summary>
    
    
    public partial class SecondaryButton : UserControl
    {
        // Holds filename in format: 'filename' no extensions
        private string? OriginalImageFileName;

        // Colors Resource Dictionary
        ResourceDictionary rd = new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/Schedio Application;component/Themes/Colors.xaml")
        };

        // Image Source
        public string ImageSource
        {
            get { return (string)GetValue(_ImageSource); }
            set { SetValue(_ImageSource, value); }
        }

        public static readonly DependencyProperty _ImageSource =
        DependencyProperty.Register("ImageSource", typeof(string), typeof(SecondaryButton), new PropertyMetadata(null));

        // Text
        public string Text
        {
            get { return (string)GetValue(_Text); }
            set { SetValue(_Text, value); }
        }

        public static readonly DependencyProperty _Text =
        DependencyProperty.Register("Text", typeof(string), typeof(SecondaryButton), new PropertyMetadata(null));

        public SecondaryButton()
        {
            InitializeComponent();
            this.DataContext = this;

            Loaded += (sender, e) =>
            {

                //Assign file name
                try
                {
                    OriginalImageFileName = findImageFilename(ImageSource.ToString());
                }
                catch (Exception ex)
                {
                    OriginalImageFileName = "";
                    MessageBox.Show(ex.Message);
                }
            };

            
        }

        private void SecondaryButton_Loaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


        // Finds .png filename from a path, returns either
        // (a) filename in format "filename.png" - Success
        // (b) Empty String - Failed
        private string findImageFilename(string path)
        {
            string[] elements = path.Split('/');

            foreach (string element in elements)
            {
                if (element.Contains(".png"))
                {
                    return element;
                }
            }
            return "";
        }

        // Returns either (a) Empty String (b) filepath with added extension in form "filename-extension.png"
        // Accepts filename in format: filename.png
        private string AddFileNameExtension(string filename, string extension)
        {
            try
            {
                if (!filename.Equals(string.Empty))
                {
                    return filename.Split(".")[0] + "-" + extension + "." + filename.Split(".")[1];
                }
                else
                {
                    return "";
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "";
            }
        }
        // Manipulates dependency property ImageSource
        // Accepts filename in format: filename.png
        private string newImagePath(string path, string prevFileName, string newFileName)
        {
            return path.Replace(prevFileName, newFileName);
        }

        private void SetButtonState(char s)
        {
            switch (s)
            {

                // Original State
                case 'o':
                    lbl_Content.Foreground = rd["SecondaryColor"] as Brush;
                    string NewImageFileName = OriginalImageFileName;
                    string PrevImageFileName = findImageFilename(ImageSource.ToString());
                    string newFileNamePath = newImagePath(ImageSource.ToString(), PrevImageFileName, NewImageFileName);
                    SetValue(_ImageSource, newFileNamePath);
                    break;

                // hovered state
                case 'h':
                    lbl_Content.Foreground = rd["PrimaryBackground"] as Brush;
                    string imageFileName = findImageFilename(ImageSource.ToString());
                    string imageFileNameWithExtension = AddFileNameExtension(findImageFilename(ImageSource.ToString()), "inversed");
                    string newFileNamePath1 = newImagePath(ImageSource.ToString(), imageFileName, imageFileNameWithExtension);
                    SetValue(_ImageSource, newFileNamePath1);
                    break;

                // disabled state
                case 'd':
                    lbl_Content.Foreground = rd["SecondaryShadeColor"] as Brush;
                    string imageFileName1 = findImageFilename(ImageSource.ToString());
                    string imageFileNameWithExtension_Disabled = AddFileNameExtension(findImageFilename(ImageSource.ToString()), "disabled");
                    string newFileNamePath_Disabled = newImagePath(ImageSource.ToString(), imageFileName1, imageFileNameWithExtension_Disabled);
                    SetValue(_ImageSource, newFileNamePath_Disabled);
                    break;
                default:
                    Trace.WriteLine("Failed to change state of button");
                    break;
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            SetButtonState('h');
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            SetButtonState('o');
        }

        private void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(btn.IsEnabled)
            {

                SetButtonState('o');

            }
            else
            {
                SetButtonState('d');
            }
        }
    }
}
