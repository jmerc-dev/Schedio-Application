using Schedio_Application.MVVM.ViewModel.Converter_Contexts;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.JsonConverters
{
    public class SubjectEntryConverter : JsonConverter<SubjectEntry>
    {

        private SubjectsConverterContext? _SubjectsConverterContext;
        private RoomsConverterContext? _RoomsConverterContext;

        public SubjectEntryConverter() { }

        public SubjectEntryConverter(SubjectsConverterContext scc, RoomsConverterContext rcc)
        {
            this._SubjectsConverterContext = scc;
            this._RoomsConverterContext = rcc;
        }

        public override SubjectEntry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            SubjectEntry? subjectEntry = JsonSerializer.Deserialize<SubjectEntry>(ref reader);

            if (subjectEntry == null)
                throw new NullReferenceException();

            if (_RoomsConverterContext == null || _RoomsConverterContext.RoomsMap == null)
                return null;

            if (_SubjectsConverterContext == null || _SubjectsConverterContext.SubjectsMap == null)
                return null;

            

            if (_SubjectsConverterContext.SubjectsMap.TryGetValue(subjectEntry.SubjectID, out Subject? subject))
                subjectEntry.SubjectInfo = subject;
            else
                throw new NullReferenceException();

            if (_RoomsConverterContext.RoomsMap.TryGetValue(subjectEntry.RoomID, out Room? room))
                subjectEntry.RoomAllocated = room;
            else
                throw new NullReferenceException();

            return subjectEntry;
        }

        public override void Write(Utf8JsonWriter writer, SubjectEntry value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("ID", value.ID);
            writer.WriteNumber("SubjectID", value.SubjectInfo.ID);
            writer.WritePropertyName("TimeFrame");
            writer.WriteRawValue(JsonSerializer.Serialize(value.TimeFrame));
            writer.WriteNumber("UnitsToAllocate", value.UnitsToAllocate);
            writer.WriteNumber("RoomID", value.RoomAllocated.ID);
            writer.WriteNumber("DayAssigned", (int) value.DayAssigned);
            writer.WriteEndObject();
        }
    }
}
