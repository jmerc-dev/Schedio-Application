﻿using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for TimeInput.xaml
    /// </summary>
    /// 


    public partial class TimeInput : UserControl, INotifyPropertyChanged
    {
        private TextBox[] textboxes = new TextBox[4];
        private int tbTraversalIndex;
        private bool? _IsStart;

        // Time Value
        private string _Time, _Period;
        private int _HourTenths, _Hour, _MinTenths, _Min;

        public bool? IsStart
        {
            get { return _IsStart; }
            set { _IsStart = value; }
        }

        public string Time
        {
            get { return HourTenths.ToString() + Hour.ToString() + ":" + MinTenths.ToString() + Min.ToString() + " " + Period; }
        }

        public string Period
        {
            get { return _Period; }
            set
            {
                _Period = value;
                OnPropertyChanged();
            }
        }

        public int HourTenths
        {
            get { return _HourTenths; }
            set
            {
                _HourTenths = value;
                OnPropertyChanged();
            }
        }
        public int Hour
        {
            get { return _Hour; }
            set
            {
                _Hour = value;
                OnPropertyChanged();
            }
        }
        public int MinTenths
        {
            get { return _MinTenths; }
            set
            {
                _MinTenths = value;
                OnPropertyChanged();
            }
        }
        public int Min
        {
            get { return _Min; }
            set
            {
                _Min = value;
                OnPropertyChanged();
            }
        }

        // Font Height Dependency Property
        public string FontHeight
        {
            get { return (string)GetValue(_FontHeight); }
            set { SetValue(_FontHeight, value); }
        }

        public static readonly DependencyProperty _FontHeight =
            DependencyProperty.Register("FontHeight", typeof(string), typeof(TimeInput), new PropertyMetadata(null));

        // Image Height Dependency Property
        public string ButtonHeight
        {
            get { return (string)GetValue(_ButtonHeight); }
            set { SetValue(_ButtonHeight, value); }
        }

        public static readonly DependencyProperty _ButtonHeight =
            DependencyProperty.Register("ButtonHeight", typeof(string), typeof(TimeInput), new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string PropertyName="")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        public bool SetTime(string time)
        {
            // Validation of time
            if (!TimeFrame.ValidateTimeFormat(time))
            {
                return false;
            }
            // String manipulation : 00:00 PM
            string[] timeSplit = time.Split(' ');
            if (timeSplit.Length != 2) {return false; }

            // Value distribution
            try
            {
                HourTenths = Int32.Parse(timeSplit[0][0].ToString());
                Hour = Int32.Parse(timeSplit[0][1].ToString());
                MinTenths = Int32.Parse(timeSplit[0][3].ToString());
                Min = Int32.Parse(timeSplit[0][4].ToString());

                Period = timeSplit[1].ToUpper();
                return true;
            } 
            catch (FormatException ex)
            {
                return false;
            }
        }

        public TimeInput()
        {
            InitializeComponent();
            this.DataContext = this;

            _Time = "";
            Period = "AM";

            Loaded += (sender, e) =>
            {
                // Add textbox to array
                textboxes[0] = tb_HourTenths;
                textboxes[1] = tb_Hour;
                textboxes[2] = tb_MinTenths;
                textboxes[3] = tb_Min;
            };

        }

        private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            string input;

            if (Regex.Match(e.Key.ToString(), "D[0-9]").Success)
            {
                input = e.Key.ToString().TrimStart('D');

                if (CheckValue(tbTraversalIndex, Int32.Parse(input), Int32.Parse(textboxes[0].Text), Int32.Parse(textboxes[1].Text)))
                {
                    ((TextBox)sender).Text = input;
                    if (tbTraversalIndex >= 0 && tbTraversalIndex <= 2)
                    {
                        textboxes[tbTraversalIndex + 1].Focus();
                    }
                }
            }
            else if (Regex.Match(e.Key.ToString(), "NumPad[0-9]").Success)
            {
                try
                {
                    
                    input = FindFirstNum(e.Key.ToString()).ToString();
                    if (Int32.Parse(input) != -1)
                    {
                        if (CheckValue(tbTraversalIndex, Int32.Parse(input), Int32.Parse(textboxes[0].Text), Int32.Parse(textboxes[1].Text)))
                        {
                            ((TextBox)sender).Text = input.ToString();
                            if (tbTraversalIndex >= 0 && tbTraversalIndex <= 2)
                            {
                                textboxes[tbTraversalIndex + 1].Focus();
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (e.Key == Key.Right)
            {
                if (tbTraversalIndex != 3)
                {
                    textboxes[tbTraversalIndex + 1].Focus();
                }
            }
            else if (e.Key == Key.Left)
            {
                if (tbTraversalIndex != 0)
                {
                    textboxes[tbTraversalIndex - 1].Focus();
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            char text = ((TextBox)sender).Text[0];
            if (!Char.IsDigit(text))
            {
                ((TextBox)sender).Text = "0";
            }
        }

        private void TimeLabel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Period.Equals("AM"))
            {
                Period = "PM";
            }
            else
            {
                Period = "AM";
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (Period.Equals("AM"))
            {
                Period = "PM";
            }
            else
            {
                Period = "AM";
            }
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            string tbName = ((TextBox)sender).Name;
            int index = FindIndex(tbName, textboxes);
            if (index != -1)
            {
                tbTraversalIndex = index;
            }
        }

        private int FindFirstNum(string word)
        {
            foreach (char letter in word)
            {
                if (char.IsDigit(letter))
                {
                    return Convert.ToInt32(letter.ToString());
                }
            }
            return -1;
        }

        private int FindIndex(string name, TextBox[] tboxes)
        {
            for (int i = 0; i < tboxes.Length; i++)
            {
                if (tboxes[i].Name.Equals(name))
                {
                    return i;
                }
            }
            return -1;
        }

        private bool CheckValue(int index, int number, int tenthHourValue, int hourValue)
        {
            switch (index)
            {
                case 0:
                    return ((number == 0 || number == 1) && (hourValue >= 0 && hourValue <= 2)) ? true : false;
                case 1:
                    if (tenthHourValue == 1)
                    {
                        return (number >= 0 && number <= 2) ? true : false;
                    }
                    else
                    {
                        return true;
                    }
                case 2:
                    return (number >= 0 && number <= 5) ? true : false;
                case 3:
                    return true;
                default:
                    return false;
            }
        }
    }
}
