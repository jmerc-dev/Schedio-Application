using Schedio_Application.MVVM.ViewModel.Converter_Contexts;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Schedio_Application.MVVM.ViewModel.JsonConverters
{
    public class SectionsConverter : JsonConverter<ObservableCollection<ClassSection>>
    {
        private readonly SectionsConverterContext? _sectionsContext;

        public SectionsConverter() { }
        public SectionsConverter(SectionsConverterContext sectionsContext)
        {
            this._sectionsContext = sectionsContext;
        }

        public override ObservableCollection<ClassSection>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (_sectionsContext == null)
                throw new NullReferenceException();

            ObservableCollection<ClassSection>? sections = JsonSerializer.Deserialize<ObservableCollection<ClassSection>>(ref reader);
            
            if (sections == null)
                throw new NullReferenceException();

            Dictionary<int, ClassSection> sectionMap = sections.ToDictionary(s => s.ID);
            _sectionsContext.ClassSectionMap = sectionMap;

            return sections;

        }

        public override void Write(Utf8JsonWriter writer, ObservableCollection<ClassSection> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (ClassSection section in value) 
            { 
                JsonSerializer.Serialize(writer, section, options);
            }
            writer.WriteEndArray();
        }
    }
}
