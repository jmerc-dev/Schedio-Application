using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using Schedio_Application.MVVM.ViewModel.WrapperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{
    public class DummyData
    {
        public DummyData(bool isSimple, ObservableCollection<RoomType> RoomTypes, ObservableCollection<Room> Rooms, ObservableCollection<Person> Personnel, ObservableCollection<ClassSection> Sections) 
        {
            if (isSimple)
            {
                RoomTypes.Add(new RoomType("Classic"));
                RoomTypes.Add(new RoomType("Lab"));
                RoomTypes.Add(new RoomType("Court"));
                return;
            }

            //RoomTypes = new ObservableCollection<RoomType>();
            RoomTypes.Add(new RoomType("Classic"));
            RoomTypes.Add(new RoomType("Lab"));
            RoomTypes.Add(new RoomType("Court"));

            Rooms.Add(new Room("101", RoomTypes[0]));
            Rooms.Add(new Room("102", RoomTypes[1]));
            Rooms.Add(new Room("103", RoomTypes[2]));
            Rooms.Add(new Room("104", RoomTypes[1]));
            Rooms.Add(new Room("105", RoomTypes[1]));
            Rooms.Add(new Room("106", RoomTypes[1]));
            Rooms.Add(new Room("107", RoomTypes[1]));
            Rooms.Add(new Room("108", RoomTypes[1]));
            Rooms.Add(new Room("109", RoomTypes[2]));
            Rooms.Add(new Room("110", RoomTypes[1]));
            Rooms.Add(new Room("201", RoomTypes[2]));
            Rooms.Add(new Room("202", RoomTypes[1]));
            Rooms.Add(new Room("203", RoomTypes[0]));
            Rooms.Add(new Room("204", RoomTypes[0]));
            Rooms.Add(new Room("205", RoomTypes[0]));

            Person[] people = [
                new Person { Name = "Jose Protacio Rizal", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "02:00 PM"},
                new Person { Name = "Emilio Aguinaldo", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "05:00 PM"},
                new Person { Name = "Apolinario Mabini", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "03:00 PM"},
                new Person { Name = "Arthur McArthur", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "01:00 PM"},
                new Person { Name = "Andres Bonifacio", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "07:00 PM"},
                new Person { Name = "Lapu Lapu", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "05:00 PM"},
                new Person { Name = "Apo Whang-od", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "02:00 PM"},
                new Person { Name = "Tomoyuki Yamashita", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "01:00 PM"},
                new Person { Name = "Gabriela Silang", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "09:00 PM"},
                new Person { Name = "Bong Bong Marcos", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "08:00 PM"},
                new Person { Name = "Benigno Aquino", IsConstant = true, ConstTime_Start = "12:00 AM", ConstTime_End = "03:00 PM"}
                ];

            foreach (Person p in people)
            {
                p.Initialize();
            }

            people[0].SetAvailableDay(DayOfWeek.Saturday, true);
            people[0].SetAvailableDay(DayOfWeek.Monday, true);

            people[1].SetAvailableDay(DayOfWeek.Monday, true);
            people[1].SetAvailableDay(DayOfWeek.Tuesday, true);
            people[1].SetAvailableDay(DayOfWeek.Wednesday, true);

            people[2].SetAvailableDay(DayOfWeek.Tuesday, true);
            people[2].SetAvailableDay(DayOfWeek.Wednesday, true);

            people[3].SetAvailableDay(DayOfWeek.Tuesday, true);

            people[4].SetAvailableDay(DayOfWeek.Thursday, true);
            people[4].SetAvailableDay(DayOfWeek.Friday, true);

            people[5].SetAvailableDay(DayOfWeek.Monday, true);
            people[5].SetAvailableDay(DayOfWeek.Friday, true);

            people[6].SetAvailableDay(DayOfWeek.Monday, true);
            people[6].SetAvailableDay(DayOfWeek.Tuesday, true);
            people[6].SetAvailableDay(DayOfWeek.Thursday, true);
            people[6].SetAvailableDay(DayOfWeek.Saturday, true);

            people[7].SetAvailableDay(DayOfWeek.Tuesday, true);
            people[7].SetAvailableDay(DayOfWeek.Wednesday, true);
            people[7].SetAvailableDay(DayOfWeek.Friday, true);

            people[8].SetAvailableDay(DayOfWeek.Monday, true);
            people[8].SetAvailableDay(DayOfWeek.Thursday, true);
            people[8].SetAvailableDay(DayOfWeek.Saturday, true);

            people[9].SetAvailableDay(DayOfWeek.Tuesday, true);

            people[10].SetAvailableDay(DayOfWeek.Monday, true);
            people[10].SetAvailableDay(DayOfWeek.Saturday, true);


            foreach (Person person in people)
            {
                Personnel.Add(person);
            }


            ClassSection[] DummySection = new ClassSection[10];
            for (int i = 0; i < DummySection.Length; i++)
            {
                string[] courses = ["IT", "CS", "HRS", "CE", "P"];
                DummySection[i] = new ClassSection();
                if (i < 2)
                    DummySection[i].Name = $"{courses[0]}{i}0A";
                else if (i < 4)
                    DummySection[i].Name = $"{courses[1]}{i}0A";
                else if (i < 6)
                    DummySection[i].Name = $"{courses[2]}{i}0A";
                else if (i < 8)
                    DummySection[i].Name = $"{courses[3]}{i}0A";
                else
                    DummySection[i].Name = $"{courses[4]}{i}0A";

                Random rnd = new Random();

                DummySection[i].Subjects.Add(new Subject
                {
                    Name = "NSTP II",
                    AssignedPerson = people[0],
                    RoomType = RoomTypes[0],
                    Units = 1,
                    OwnerSection = DummySection[i]
                });

                DummySection[i].Subjects.Add(new Subject
                {
                    Name = "Computer Programming",
                    AssignedPerson = people[1],
                    RoomType = RoomTypes[0],
                    Units = 3,
                    OwnerSection = DummySection[i]
                });
                DummySection[i].Subjects.Add(new Subject
                {
                    Name = "Computer Programming Lab",
                    AssignedPerson = people[2],
                    RoomType = RoomTypes[1],
                    Units = 2,
                    OwnerSection = DummySection[i]
                });
                DummySection[i].Subjects.Add(new Subject
                {
                    Name = "Entrepreneurship",
                    AssignedPerson = people[3],
                    RoomType = RoomTypes[0],
                    Units = 2,
                    OwnerSection = DummySection[i]
                });
                DummySection[i].Subjects.Add(new Subject
                {
                    Name = "Automata Theory",
                    AssignedPerson = people[4],
                    RoomType = RoomTypes[0],
                    Units = 3,
                    OwnerSection = DummySection[i]
                });
                DummySection[i].Subjects.Add(new Subject
                {
                    Name = "PE",
                    AssignedPerson = people[5],
                    RoomType = RoomTypes[2],
                    Units = 2,
                    OwnerSection = DummySection[i]
                });
                DummySection[i].Subjects.Add(new Subject
                {
                    Name = "Design and Analysis of Algorithms",
                    AssignedPerson = people[6],
                    RoomType = RoomTypes[0],
                    Units = 3,
                    OwnerSection = DummySection[i]
                });
                Sections.Add(DummySection[i]);
            }
        }
    }
}
