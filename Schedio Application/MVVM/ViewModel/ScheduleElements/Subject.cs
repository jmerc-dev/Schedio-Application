﻿using Schedio_Application.MVVM.View.UserControls;
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
        private bool _IsAllocated;

        
        public Action<SubjectEntry, DataAction> SubjectOperation;

        public static ObservableCollection<SubjectEntry> subjectEntries = new ObservableCollection<SubjectEntry>();

        public RelayCommand AllocSubjectCommand => new RelayCommand(execute => AllocSubject());
        public RelayCommand DeAllocSubjectCommand => new RelayCommand(execute => DeallocSubject(execute));

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
                    _Units = value;
                    return;
                }
                
                if (value > _Units)
                {
                    UnitsRemaining += value - _Units;
                }
                else
                {
                    if (value < _Units - UnitsRemaining)
                    {
                        new MBox($"You cannot decrease the units because it is allocated in the workshop or it will result in negative value.").ShowDialog();
                        return;
                    }

                    UnitsRemaining -= _Units - value;
                }
                _Units = value;


                if (this.OwnerSection != null)
                {
                    this.OwnerSection.TotalUnits = this.OwnerSection.GetTotalUnits();
                }

                Trace.WriteLine($"{this.Name}: Units - {this.Units}, RemUnits - {this.UnitsRemaining}");

                OnPropertyChanged();
            }
        }

        public double UnitsRemaining
        {
            get { return _UnitsRemaining; }
            set
            {

                // Updates Allocated Units Indicator
                if (this.OwnerSection != null)
                {
                    this.OwnerSection.AllocatedUnits -= _Units - _UnitsRemaining;
                    this.OwnerSection.AllocatedUnits += _Units - value;
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

            if (this.IsAllocated)
            {
                new MBox("This subject has been fully allocated.", MBoxImage.Information).ShowDialog();
                return;
            }

            if (subjectAllocation.ShowDialog() == true)
            {

                //foreach (SubjectEntry entry in Subject.subjectEntries)
                //{
                //    if ((subjectAllocation.Entry.DayAssigned == entry.DayAssigned) && (subjectAllocation.Entry.RoomAllocated == entry.RoomAllocated))
                //    {
                        
                //        if (subjectAllocation.Entry.TimeFrame.StartTime == null || subjectAllocation.Entry.TimeFrame.EndTime == null)
                //        {
                //            new MBox("StartTime or EndTime is null.").ShowDialog();
                //            return;
                //        }

                //        if (entry.TimeFrame.IsOverlap(subjectAllocation.Entry.TimeFrame.StartTime) || entry.TimeFrame.IsOverlap(subjectAllocation.Entry.TimeFrame.EndTime) || entry.TimeFrame.IsContainedBy(subjectAllocation.Entry.TimeFrame))
                //        {
                //            new MBox($"You cannot allocate this subject because it is conflicting with:\n{entry.SubjectInfo.OwnerSection.Name}: {entry.TimeFrame.StartTime} => {entry.TimeFrame.EndTime} in {entry.DayAssigned.ToString()}").ShowDialog();
                //            return;
                //        }
                //    }
                //}

                subjectEntries.Add(subjectAllocation.Entry);
                UnitsRemaining -= subjectAllocation.Entry.UnitsToAllocate;

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
            }
            if (new MBox("Are you sure you want to deallocate this entry?", MBoxType.CancelOrOK).ShowDialog() == true)
            {
                SubjectEntry se = (SubjectEntry)entry;
                se.SubjectInfo.UnitsRemaining += se.UnitsToAllocate;
                subjectEntries.Remove(se);
                
                new MBox("Subject entry deallocated", MBoxImage.Information).ShowDialog();
            }
            
        }

        // For section and subjects only
        public static bool IsDataBeingUsed(ScheduleElement element, object obj)
        {
            switch (element)
            {
                case ScheduleElement.ClassSection:
                    ClassSection cs = (ClassSection)obj;
                    foreach (SubjectEntry se in subjectEntries)
                    {
                        if (se.SubjectInfo.OwnerSection == cs)
                        {
                            return true;
                        }
                    }
                    return false;
                case ScheduleElement.Subject:
                    Subject s = (Subject)obj;
                    foreach (SubjectEntry se in subjectEntries)
                    {
                        if (se.SubjectInfo == s)
                        {
                            return true;
                        }
                    }
                    return false;
                case ScheduleElement.Room:
                    Room room = (Room)obj;
                    foreach (SubjectEntry se in subjectEntries)
                    {
                        if (se.RoomAllocated == room)
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
