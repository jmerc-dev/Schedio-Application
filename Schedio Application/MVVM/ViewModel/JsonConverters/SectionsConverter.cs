using Schedio_Application.MVVM.ViewModel.Converter_Contexts;
using Schedio_Application.MVVM.ViewModel.ScheduleElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Schedio_Application.MVVM.ViewModel.JsonConverters
{
    public class SectionsConverter : JsonConverter<ObservableCollection<ClassSection>>
    {
        private readonly PeopleConverterContext? _peopleContext;

        public SectionsConverter() { }
        public SectionsConverter(PeopleConverterContext peopleContext)
        {
            this._peopleContext = peopleContext;
        }

        public override ObservableCollection<ClassSection>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (_peopleContext == null || _peopleContext.PeopleMap == null)
                throw new NullReferenceException();

            ObservableCollection<ClassSection>? sections = JsonSerializer.Deserialize<ObservableCollection<ClassSection>>(ref reader);

            if (sections == null)
                throw new NullReferenceException();

            foreach (ClassSection section in sections)
            {
                foreach (Subject subject in section.Subjects)
                {
                    // Class Section reference resolve
                    subject.OwnerSection = section;

                    // Assigned personnel reference resolve
                    if (_peopleContext.PeopleMap.TryGetValue(subject.PersonnelID, out Person? person))
                    {
                        if (person == null)
                            throw new NullReferenceException();

                        subject.AssignedPerson = person;
                    }
                }
            }

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
