using Schedio_Application.MVVM.ViewModel.Converter_Contexts;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Schedio_Application.MVVM.ViewModel.JsonConverters
{
    public class SubjectConverter : JsonConverter<Subject>
    {
        private SectionsConverterContext? _SectionsContext;
        private PeopleConverterContext? _PeopleContext;

        public SubjectConverter() { }

        public SubjectConverter(SectionsConverterContext sectionsConverterContext, PeopleConverterContext peopleContext)
        {
            this._SectionsContext = sectionsConverterContext;
            this._PeopleContext = peopleContext;
        }

        public override Subject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Subject? sub = JsonSerializer.Deserialize<Subject>(ref reader);

            if (sub == null || _SectionsContext == null || _PeopleContext == null)
                throw new NullReferenceException();

            if (_PeopleContext.PeopleMap == null || _SectionsContext.ClassSectionMap == null)
                throw new NullReferenceException();

            // Assigned Personnel Resolving
            if (_PeopleContext.PeopleMap.TryGetValue(sub.PersonnelID, out Person? person))
            {
                if (person == null)
                    throw new NullReferenceException();
                sub.AssignedPerson = person;
            }

            if (_SectionsContext.ClassSectionMap.TryGetValue(sub.OwnerSectionID, out ClassSection? section))
            {
                if (section == null)
                    throw new NullReferenceException();
                sub.OwnerSection = section;
            }

            return sub;
        }

        public override void Write(Utf8JsonWriter writer, Subject value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("PersonnelID", value.AssignedPerson.ID);
            writer.WriteNumber("OwnerSectionID", value.OwnerSection.ID);
            writer.WriteString("Name", value.Name);
            writer.WriteBoolean("IsAllocated", value.IsAllocated);
            writer.WriteNumber("RoomTypeID", value.RoomType.ID);
            writer.WriteNumber("Units", value.Units);
            writer.WriteNumber("UnitsRemaining", value.UnitsRemaining);
            writer.WriteNumber("UnitsAllocated", value.UnitsAllocated);
            writer.WriteEndObject();
        }

        
    }
}
