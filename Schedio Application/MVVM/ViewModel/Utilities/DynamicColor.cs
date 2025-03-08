using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{

    enum ColorMode
    {
        RGB,
        HSL
    }

    public class DynamicColor : PropertyNotification
    {
        
        private RGBColor _RGB;
        private HSLColor _HSL;
        private byte _Alpha = 255;
        private Color _ActualColor;
        private ColorMode _Mode;

        public RGBColor RGB
        {
            get => _RGB;
            set => value = _RGB;
        }
        

        public HSLColor HSL
        {
            get => _HSL;
            set => value = _HSL;
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

        public Color ActualColor
        {
            get => _ActualColor;
            set
            {
                _ActualColor = value;
                OnPropertyChanged();
            }
        }

        public DynamicColor() 
        {
            _RGB = new RGBColor();
            _HSL = new HSLColor();
        }



        public DynamicColor(RGBColor rgb, HSLColor hsl)
        {
            _RGB = rgb;
            _HSL = hsl;
        }


    }
}
