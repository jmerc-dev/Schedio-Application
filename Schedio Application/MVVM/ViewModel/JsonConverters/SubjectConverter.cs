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
        private SubjectsConverterContext? _SubjectConverterContext;

        public SubjectConverter() { }

        public SubjectConverter(SubjectsConverterContext subjectConverterContext)
        {
            this._SubjectConverterContext = subjectConverterContext;
        }

        public override Subject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Subject? sub = JsonSerializer.Deserialize<Subject>(ref reader);

            if (sub == null || _SubjectConverterContext == null || _SubjectConverterContext.SubjectsMap == null)
                throw new NullReferenceException();

            _SubjectConverterContext.SubjectsMap.Add(sub.ID, sub);

            return sub;
        }

        public override void Write(Utf8JsonWriter writer, Subject value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("ID", value.ID);
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

        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(Subject);
    }
}
