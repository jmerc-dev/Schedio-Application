using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Commands;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class ClassSection : PropertyNotification
    {
        private static int _IdCount;

        private int _ID;
        private string? _Name;
        private ObservableCollection<Subject> _Subjects;
        private int _TotalSubjects;
        private double _TotalUnits;
        private int _AllocatedSubjects;
        private double _AllocatedUnits;
        private Color _Color;

        public RelayCommand SetColorCommand => new RelayCommand(execute => SetColor(this));

        public int ID 
        {
            get => _ID;
            set => _ID = value;
        }

        public int TotalSubjects
        {
            get { return _TotalSubjects; }
            set
            {
                _TotalSubjects = value;
                OnPropertyChanged();
            }
        }

        public Color SectionColor
        {
            get => _Color;
            set
            {
                _Color = value;
                OnPropertyChanged();
            }
        }

        public string? Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        public double TotalUnits
        {
            get { return _TotalUnits; }
            set
            {
                _TotalUnits = value;
                OnPropertyChanged();
            }
        }

        //[JsonIgnore]
        public int AllocatedSubjects
        {
            get { return _AllocatedSubjects; }
            set
            {
                _AllocatedSubjects = value;
                OnPropertyChanged();
            }
        }

        //[JsonIgnore]
        public double AllocatedUnits
        {
            get { return _AllocatedUnits; }
            set
            {
                _AllocatedUnits = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Subject> Subjects
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
            _Subjects = new ObservableCollection<Subject>();
            _Subjects.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSubListChanged);

            SectionColor = new Color
            {
                R = 38,
                G = 92,
                B = 197,
                A = 255
            };
        }

        private void OnSubListChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            TotalSubjects = Subjects.Count;
            TotalUnits = GetTotalUnits();
        }

        public double GetTotalUnits()
        {
            double total = 0;
            foreach (Subject s in Subjects)
            {
                total += s.Units;
            }
            return total;
        }

        public void UpdateAllocatedUnits()
        {
            double newAllocatedUnits = 0;

            foreach (Subject s in Subjects) 
            {
                newAllocatedUnits += s.UnitsAllocated;
            }

            AllocatedUnits = newAllocatedUnits;
        }

        public void UpdateAllocatedSubjects()
        {
            int fullyAllocatedSubjects = 0;

            foreach (Subject s in Subjects)
            {
                fullyAllocatedSubjects += s.IsAllocated ? 1 : 0;
            }

            this.AllocatedSubjects = fullyAllocatedSubjects;
            Trace.WriteLine(this.AllocatedSubjects);
        }

        public bool SetColor(ClassSection section)
        {
            
            new ColorSwatch(section).ShowDialog();
            return true;
        }

    }
}
