using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Custom_Exceptions
{
    public class NotInListException : Exception
    {
        public NotInListException() { }

        public NotInListException(string message) : base($"{message}")
        {
            
        }
    }
}
