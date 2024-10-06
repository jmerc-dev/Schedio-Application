﻿using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class ClassSection : PropertyNotification
    {
        private string _Name;
        private ObservableCollection<Subject> _Subjects;
        private int _TotalSubjects;
        private double _TotalUnits;
        private int _UnallocatedSubjects;
        private double _UnallocatedUnits;
        private int idCounter;
        
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

        public double TotalUnits
        {
            get { return _TotalUnits; }
            set
            {
                _TotalUnits = value;
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

            Subjects.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSubListChanged);
        }

        private void OnSubListChanged(object sender, NotifyCollectionChangedEventArgs args)
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
    }
}
