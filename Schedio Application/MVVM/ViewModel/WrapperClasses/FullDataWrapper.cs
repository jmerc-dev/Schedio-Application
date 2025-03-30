using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.WrapperClasses
{
    public class FullDataWrapper
    {
        public FileIdentifier? Identifier { get; set; }
        public PeopleGroup? PeopleGroup { get; set; }
        public RoomTypeGroup? RoomTypesGroup { get; set; }
        public RoomGroup? RoomsGroup { get; set; }
        public SectionGroup? SectionsGroup { get; set; }
        public SubjectEntriesGroup? SubjectEntriesGroup { get; set; }

    }
}
