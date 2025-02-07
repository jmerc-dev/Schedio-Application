﻿using Schedio_Application.MVVM.View.Windows;
using Schedio_Application.MVVM.ViewModel.Custom_Exceptions;
using Schedio_Application.MVVM.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.ScheduleElements
{
    public enum ScheduleElement
    {
        Person,
        Room,
        RoomType,
        Subject,
        Day,
        ClassSection
    }
    public class SubjectEntry : PropertyNotification
    {
        private Subject _Subject;
        private string? _StartTime;
        private string? _EndTime;
        private TimeFrame _TimeFrame;
        private double _UnitsToAllocate;
        private Room? _Room;
        private DayOfWeek? _DayAssigned;

        public Subject SubjectInfo 
        { 
            get {  return _Subject; } 
            set 
            {  
                _Subject = value;
                OnPropertyChanged();
            }
        }

        public TimeFrame TimeFrame
        {
            get => _TimeFrame;
            set
            {
                _TimeFrame = value;
                OnPropertyChanged();
            }
        }

        //public string? StartTime
        //{
        //    get { return _StartTime; }
        //    set 
        //    { 
        //        _StartTime = value;

        //        if (value != null)
        //        {
        //            DateTime endTime = DateTime.Parse(value).AddHours(UnitsToAllocate);
        //            DateTime endTime1 = DateTime.Parse(value).AddHours(3);
        //            EndTime = endTime.ToString("hh:mm tt");
        //        }
        //        OnPropertyChanged();
        //    }
        //}

        //public string? EndTime
        //{
        //    get { return _EndTime; }
        //    set
        //    {
        //        _EndTime = value;
        //        OnPropertyChanged();
        //    }
        //}

        public double UnitsToAllocate
        {
            get { return _UnitsToAllocate; }
            set 
            { 
                _UnitsToAllocate = value;
                OnPropertyChanged();
            }
        }

        public Room? RoomAllocated
        {
            get { return _Room; }
            set 
            { 
                _Room = value;
                OnPropertyChanged();
            }
        }

        public DayOfWeek? DayAssigned
        {
            get { return _DayAssigned; }
            set 
            { 
                _DayAssigned = value;
                OnPropertyChanged();
            }
        }


        public SubjectEntry(Subject subject)
        {
            _Subject = subject;
            _TimeFrame = new TimeFrame();
        }

        public SubjectEntry(Subject subject, TimeFrame tf, double units, Room room)
        {
            _Subject = subject;
            _TimeFrame = tf;
            _UnitsToAllocate = units;
            _Room = room;
        }

        public bool ValidateTimeframe(SubjectEntry mainEntry, SubjectEntry existingEntry)
        {
            if (existingEntry.DayAssigned == mainEntry.DayAssigned && mainEntry.RoomAllocated == existingEntry.RoomAllocated)
            {
                if (existingEntry.TimeFrame.WillConcurWith(mainEntry.TimeFrame))
                {
                    throw new TimeframeOverlapException(mainEntry, existingEntry);
                }
            }
            return true;
        }

        public bool ValidateParallelPersonnel(SubjectEntry mainEntry, SubjectEntry existingEntry)
        {
            if (mainEntry.DayAssigned == existingEntry.DayAssigned && mainEntry.RoomAllocated != existingEntry.RoomAllocated && mainEntry.SubjectInfo.AssignedPerson == existingEntry.SubjectInfo.AssignedPerson)
            {
                if (existingEntry.TimeFrame.WillConcurWith(mainEntry.TimeFrame))
                {
                    new MBox($"{mainEntry.SubjectInfo.AssignedPerson.Name} is currently assigned at the timeframe: {existingEntry.TimeFrame.StartTime} => {existingEntry.TimeFrame.EndTime} in {existingEntry.DayAssigned.ToString()}").ShowDialog();
                    throw new PersonnelOverlapException(mainEntry, existingEntry);
                }
            }
            return true;
        }

        public bool ValidateAvailability()
        {
            foreach (SubjectEntry se in Subject.subjectEntries)
            {
                if (this == se)
                    continue;

                if (!ValidateTimeframe(this, se))
                    return false;

                if (!ValidateParallelPersonnel(this, se))
                    return false;
            }
            return true;
        }

        public bool Update()
        {

            return true;
        }
    }
}
