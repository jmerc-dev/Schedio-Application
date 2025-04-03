using Schedio_Application.MVVM.View.UserControls;
using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Commands;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public class Subject : PropertyNotification
    {
        private static int _IdCounter;

        private int _ID;
        private string _Name;
        private Person _AssignedPerson;
        private RoomType _RoomType;
        private double _Units;
        private ClassSection _ClassSection;

        private double _UnitsRemaining;
        private double _UnitsAllocated;
        private bool _IsAllocated;

        
        public Action<SubjectEntry, DataAction> SubjectOperation;

        private static ObservableCollection<SubjectEntry> _SubjectEntries = new ObservableCollection<SubjectEntry>();

        public RelayCommand AllocSubjectCommand => new RelayCommand(execute => AllocSubject());
        public RelayCommand DeAllocSubjectCommand => new RelayCommand(execute => DeallocSubject(execute));
        public RelayCommand AdjustSubjectCommand => new RelayCommand((execute) => AdjustSubjectCard(execute));

        public int PersonnelID {  get; set; }
        public int OwnerSectionID { get; set; }

        public int ID
        {
            get { return _ID; }
        }

        public static int IDCount
        {
            get => _IdCounter;
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

        public bool IsAllocated
        {
            get { return _IsAllocated; }
            set {  _IsAllocated = value; OnPropertyChanged(); }
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
                if (_Units == 0)
                {
                    UnitsRemaining = value;
                }
                else
                {
                    if (value > _Units)
                    {
                        UnitsRemaining += value - _Units;
                    }
                    else
                    {
                        if (value < _Units - UnitsRemaining)
                        {
                            new MBox($"You cannot decrease the units because it is allocated in the workshop.").ShowDialog();
                            return;
                        }

                        UnitsRemaining -= _Units - value;
                    }
                }
                
                _Units = value;


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
                if (_UnitsRemaining == value)
                {
                    return;
                }

                _UnitsRemaining = value;

                // Updates Allocated Subjects Indicator
                if (_UnitsRemaining == 0)
                {
                    OwnerSection.AllocatedSubjects += 1;
                    IsAllocated = true;
                }
                else
                {
                    if (IsAllocated)
                    {
                        OwnerSection.AllocatedSubjects -= 1;
                        IsAllocated = false;
                    }
                }
                OnPropertyChanged();
            }
        }

        public double UnitsAllocated
        {
            get => _UnitsAllocated;
            set => _UnitsAllocated = value;
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
            get => _SubjectEntries;
            set => _SubjectEntries = value;
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
            this._ID = Interlocked.Increment(ref _IdCounter);
        }

        public Subject()
        {
            this._ID = Interlocked.Increment(ref _IdCounter);
        }

        public Subject(ClassSection ownerSection, int id)
        {
            this._ID = id;
        }

        public Subject(Subject subject, ClassSection section)
        {
            this.Name = subject.Name;
            this.RoomType = subject.RoomType;
            this.AssignedPerson = subject.AssignedPerson;
            this.Units = subject.Units;
            this._ClassSection = section;
            this._ID = Interlocked.Increment(ref _IdCounter);
        }


        // Entries CRUD

        // Use delegate to add subjectCards
        private void AllocSubject()
        {
            SubjectAllocation subjectAllocation = new SubjectAllocation(this, State.New);

            if (this.IsAllocated)
            {
                new MBox("This subject has been fully allocated.", MBoxImage.Information).ShowDialog();
                return;
            }

            if (subjectAllocation.ShowDialog() == true)
            {

                _SubjectEntries.Add(subjectAllocation.Entry);
                UnitsRemaining -= subjectAllocation.Entry.UnitsToAllocate;

                // Updates Allocated Units Indicator
                if (this.OwnerSection != null)
                {
                    this.OwnerSection.AllocatedUnits += subjectAllocation.Entry.UnitsToAllocate;
                }
            }
        }

        private void DeallocSubject(object entry)
        {
            try
            {
                SubjectEntry subjectEntry = (SubjectEntry)entry;
            } catch (Exception ex)
            {
                new MBox("Cannot cast from object to subject entry").ShowDialog();
                return;
            }

            if (new MBox("Are you sure you want to deallocate this entry?", MBoxType.CancelOrOK).ShowDialog() == true)
            {
                SubjectEntry se = (SubjectEntry)entry;
                se.SubjectInfo.UnitsRemaining += se.UnitsToAllocate;
                _SubjectEntries.Remove(se);

                // Updates Allocated Units Indicator
                if (this.OwnerSection != null)
                {
                    this.OwnerSection.AllocatedUnits -= se.UnitsToAllocate;
                }

                new MBox("Subject entry deallocated", MBoxImage.Information).ShowDialog();
            }
            
        }

        private void AdjustSubjectCard(object entry)
        {
            // Should pass the entry here
            if (entry == null)
            {
                Trace.WriteLine("entry is null");
                return;
            }
            SubjectEntry subEntry;
            try
            {
                subEntry = (SubjectEntry)entry;
            }
            catch (Exception ex)
            {
                new MBox("Cannot cast from object to subject entry").ShowDialog();
                return;
            }

            double previousUnits = subEntry.UnitsToAllocate;
            this.OwnerSection.AllocatedUnits -= subEntry.UnitsToAllocate;

            SubjectAllocation subAllocObj = new SubjectAllocation(subEntry);
            if (subAllocObj.ShowDialog() == true)
            {
                
                if (subAllocObj.Entry.UnitsToAllocate > previousUnits)
                    subAllocObj.Entry.SubjectInfo.UnitsRemaining -= subAllocObj.Entry.UnitsToAllocate - previousUnits;
                else if (subAllocObj.Entry.UnitsToAllocate < previousUnits)
                    subAllocObj.Entry.SubjectInfo.UnitsRemaining += previousUnits - subAllocObj.Entry.UnitsToAllocate;

                this.OwnerSection.AllocatedUnits += subAllocObj.Entry.UnitsToAllocate;

                Subject._SubjectEntries[Subject._SubjectEntries.IndexOf(subEntry)] = subEntry;

            }
        }

        // For section and subjects only
        public static bool IsDataBeingUsed(ScheduleElement element, object obj)
        {
            switch (element)
            {
                case ScheduleElement.ClassSection:
                    ClassSection cs = (ClassSection)obj;
                    foreach (SubjectEntry se in _SubjectEntries)
                    {
                        if (se.SubjectInfo.OwnerSection == cs)
                        {
                            return true;
                        }
                    }
                    return false;
                case ScheduleElement.Subject:
                    Subject s = (Subject)obj;
                    foreach (SubjectEntry se in _SubjectEntries)
                    {
                        if (se.SubjectInfo == s)
                        {
                            return true;
                        }
                    }
                    return false;
                case ScheduleElement.Room:
                    Room room = (Room)obj;
                    foreach (SubjectEntry se in _SubjectEntries)
                    {
                        if (se.RoomAllocated == room)
                        {
                            return true;
                        }
                    }
                    return false;
                case ScheduleElement.Person:
                    Person person = (Person)obj;
                    foreach (SubjectEntry se in _SubjectEntries)
                    {
                        if (se.SubjectInfo.AssignedPerson == person)
                        {
                            return true;
                        }
                    }
                    return false;
                default: return false;
            }
        }
    }
}
