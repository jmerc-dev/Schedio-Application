using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Commands;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Subject : PropertyNotification
    {
        private int id;
        private string _Name;
        private Person _AssignedPerson;
        private RoomType _RoomType;
        private double _Units;
        private ClassSection _ClassSection;

        private double _UnitsRemaining;
        private bool IsAllocated;

        // TODO
        public Action<SubjectEntry, DataAction> SubjectOperation;

        private static ObservableCollection<SubjectEntry> subjectEntries = new ObservableCollection<SubjectEntry>();
        /*
         * Inclusions: 
         */
        public RelayCommand AllocSubjectCommand => new RelayCommand(execute => AllocSubject());

        // Implement id system per subject
        public int Id
        {
            get { return id; }
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

        public Person AssignedPerson
        {
            get { return _AssignedPerson; }
            set 
            { 
                _AssignedPerson = value;
                OnPropertyChanged();
            }
        }

        public RoomType RoomType
        {
            get { return _RoomType; }
            set 
            { 
                _RoomType = value;
                OnPropertyChanged();
            }
        }

        public double Units
        {
            get { return _Units; }
            set 
            { 
                if (_Units > 0)
                {
                    double allocatedUnits = _Units - UnitsRemaining;
                    _Units = value;
                    UnitsRemaining = _UnitsRemaining + allocatedUnits;
                }
                else
                {
                    _Units = value;
                    UnitsRemaining = value;
                }

                if (this.OwnerSection != null)
                {
                    this.OwnerSection.TotalUnits = this.OwnerSection.GetTotalUnits();
                }
                
                OnPropertyChanged();
            }
        }

        public double UnitsRemaining
        {
            get { return _UnitsRemaining; }
            set
            {
                _UnitsRemaining = value;
                OnPropertyChanged();
            }
        }

        public ClassSection OwnerSection
        {
            get { return _ClassSection; }
            set 
            { 
                _ClassSection = value;
                OnPropertyChanged();
            }
        }

        public static ObservableCollection<SubjectEntry> SubjectEntries
        {
            get { return subjectEntries; }
        }

        public Subject(string Name, Person assignedPerson, RoomType roomType, int units)
        {
            _Name = Name;
            _AssignedPerson = assignedPerson;
            _RoomType = roomType;
            _Units = units;
        }


        public Subject(ClassSection ownerSection)
        {
            OwnerSection = ownerSection;
        }

        public Subject()
        {

        }

        public Subject(Subject subject, ClassSection section)
        {
            this.Name = subject.Name;
            this.RoomType = subject.RoomType;
            this.AssignedPerson = subject.AssignedPerson;
            this.Units = subject.Units;
            this._ClassSection = section;
        }


        // Entries CRUD

        // Use delegate to add subjectCards
        private void AllocSubject()
        {
            SubjectAllocation subjectAllocation = new SubjectAllocation(this);
            if (subjectAllocation.ShowDialog() == true)
            {
                if (UnitsRemaining - subjectAllocation.Entry.UnitsToAllocate < 0)
                {
                    new MBox("Failed to add. Units to allocate is bigger than the remaining.", MBoxImage.Information).ShowDialog();
                    return;
                }

                subjectEntries.Add(subjectAllocation.Entry);
                UnitsRemaining -= subjectAllocation.Entry.UnitsToAllocate;

                //Trace.WriteLine("Subject Entry Added");
            }
        }
    }
}
