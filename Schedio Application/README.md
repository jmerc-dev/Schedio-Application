# Schedio
### Video Demo:  https://www.youtube.com/watch?v=imhc14qv9-U
### Description: 
Schedio is an schedule management system that targets universities and colleges. It provides functionalities that cater to schedule manager's needs. Providing an efficient allocation with guidance to improve how they manage each class' schedules. Also, this project aims to be capable of automatically allocating subjects in the future. 

### Project Details:
#### Architecture:
Though not clearly done, the project is aimed to have the Model-View-ViewModel (MVVM) design pattern. The developer is still improving it from time to time.
#### Design:
Schedio is designed to have a modern look which consists of flat user interfaces with mostly rounded corners for easy look. It comes with a very simple UI that caters to the complex tasks.
#### Vision:
Upon creating this project, the developer visualizes it as automated scheduler system because it provides greater efficiency for university managers. However, manual allocation must also be done for some adjustments. Therefore, in the future, the developer should consider using complex algorithms to provide better experiences to users. 

### About the App:
#### Workflow:
In Schedio, there are two phases: the Data Entry phase and Allocation phase.
- In Data Entry Phase, the users should fill up the schedule elements: People, Rooms, Room Types, Sections, and Subjects. This section must be filled out as it will be used on the allocation phase later on.
- In Allocation phase, the users should only focus on allocating subjects and filling up the weekly schedule. It's like a game where you have to deplete all remaining units for each subject!

#### Schedule Elements:
There are five (6) main elements that can be used to work with Schedio. These are the following: 
- Personnel
This represents human entities in a university setting such as professors, instructors, etc. Each can be assigned on various subjects later on.
- Room Type
A room type is a category of room for appropriate use. For example, chemistry classes requires laboratory rooms that can be categorized as 'Lab'.
- Room
A room is simply a room that has a name and room type. It is required to have a designated room type for organizing classes and subjects.
- Class Section
This represents a set of students, subjects can be added to each class section to organize and easily distinguish them.
- Subject
A subject is a part of a course' curriculum, e.g. Calculus I, Modelling and Simulation, etc. These data are needed to create a subject: Personnel, and Room Type.
- Subject Entry
The subject entry is the data used when allocating subjects in our schedule. It requires the following data: Room, and Subject.

#### Features:
- Local file save/load
Save and load your files easily with JSON format. Continue where you left off!
- Smart Warnings
Forget being messy, the system will warn you if you try to allocate invalid entries in parallel. For example, two entries of a section within the same time.
- Interactive Allocating Space
Visualize your schedule using the draggable space (also called as Time Table). This ensures that you will be able to see all rooms from 12:00 AM to 11:59 PM.
- Status reminders
Track your progress with the built-in monitoring system. This includes allocated/total subjects, units, or if a subject has been fully allocated.
- Exporting files (TBD)
Export through pdf files with custom sections only for easier visualizations.

#### Controls:
- Counter: This component has numerical textbox and up & down buttons. The buttons increments/decrements the textbox value by 1.
- Time Input: A control that lets users input time with validations. It is specifically made to have 12-hour system with AM/PM notations.
- Start/End Time Input: An input that has both start time and end time input.
- TimeTable: This control is the interactive user interface for allocating subjects. It has multiple canvas containing room header, time container, and entries container.
- Subject Card: Cards are used to visualize entries within the Time Table control. It can be dynamically sized vertically. However, it has a fixed width of 200 units.
- Subject Item: Subject item is used for adding subjects in the Data phase. It consists of 4 controls: name, room type, assigned personnel, and units. Also, it has a delete button for destroying its own entry.

#### Windows:
- Main Window: The landing page of the application. It should show recent projects, creat new project, or load file from file manager.
- Work Shop: This window is where the work begins and constantly stays. It consists of Data phase and Allocation phase.
- MBox (Custom Message Box): A message box that shows errors, warnings, suggestions, etc.
- Personnel Add Form: Requires a person object and manipulates the person's properties.
- Room Add Form: Requires a room object where users can change a room's name and type.
- Section Add Form: Accepts a Class Section object. Section Name and subjects can be edited here.
- Room Type Setup: Contains CRUD operations for Room Types.
- Section Explorer: Includes the list of sections containing their statuses.
- Time Schedule Add Form: The base schedule for each day can be edited here. Not used in this project.
- Warning Confirmation: Additional Custom message box only used when deleting.


#### Tools:
Color Adjustment: Adjust color for each section to easily identify them.
Subjects Viewer: Choose subjects within a selected section only.

 