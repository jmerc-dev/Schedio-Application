using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.Utilities
{
    public class SubjectEntryConverter : JsonConverter<SubjectEntry>
    {
        public override SubjectEntry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, SubjectEntry value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            
            writer.WriteEndObject();
        }
    }
}
