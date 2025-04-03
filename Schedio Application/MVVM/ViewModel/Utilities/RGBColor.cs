using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{
    public class RGBColor : PropertyNotification
    {
        private byte _Red;
        private byte _Green;
        private byte _Blue;
        private byte _Alpha;

        public byte R
        {
            get => _Red;
            set 
            {
                _Red = value;
                OnPropertyChanged();
            }
        }

        public byte G
        {
            get => _Green;
            set 
            { 
                _Green = value;
                OnPropertyChanged();
            }
        }

        public byte B
        {
            get => _Blue;
            set
            {
                _Blue = value;
                OnPropertyChanged();
            }
        }

        public byte Alpha
        {
            get => _Alpha;
            set
            {
                _Alpha = value;
                OnPropertyChanged();
            }
        }


        public RGBColor()
        {

        }

        public RGBColor(Color value)
        {
            this.R = value.R;
            this.B = value.B;
            this.G = value.G;
        }


    }
}
