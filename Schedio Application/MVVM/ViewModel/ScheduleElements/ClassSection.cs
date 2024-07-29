using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class ClassSection : PropertyNotification
    {
        private string _Name;
        private List<Subject> _Subjects;

        public int TotalSubjects
        {
            get { return Subjects.Count; }
        }

        public string Name
        {
            get { return _Name; }
            set 
            { 
                _Name = value;
                OnPropertyChanged();
            }
        }

        public int TotalUnits
        {
            get 
            { 
                int totalUnits = 0;
                foreach (Subject subject in Subjects)
                {
                    totalUnits += subject.Units;
                }
                return totalUnits;
            }
        }

        public List<Subject> Subjects
        {
            get { return _Subjects; }
            set 
            { 
                _Subjects = value;
                OnPropertyChanged();
            }
        }

        public ClassSection()
        {
            _Subjects = new List<Subject>();
            _Name = string.Empty;
        }
    }
}
