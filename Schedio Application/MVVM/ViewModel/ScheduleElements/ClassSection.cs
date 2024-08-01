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
        private int _TotalSubjects;
        private int _TotalUnits;

        public int TotalSubjects
        {
            get { return _TotalSubjects; }
            set
            {
                _TotalSubjects = value;
                OnPropertyChanged();
            }
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
            get { return _TotalUnits; }
            set
            {
                _TotalUnits = value;
                OnPropertyChanged();
            }
        }

        public List<Subject> Subjects
        {
            get { return _Subjects; }
            set
            {
                _Subjects = value;
                TotalSubjects = Subjects.Count;
                TotalUnits = GetTotalUnits();
                OnPropertyChanged();
            }
        }

        public ClassSection()
        {
            _Subjects = new List<Subject>();
            _Name = string.Empty;
        }

        public int GetTotalUnits()
        {
            int total = 0;
            foreach (Subject s in Subjects)
            {
                total += s.Units;
            }
            return total;
        }
    }
}
