using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Custom_Exceptions
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException() { }

        public InvalidFileException(string filePath) :
            base($"The application cannot load the file {filePath} because it is invalid.")
        {

        }
    }
}
