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

namespace Schedio_Application.MVVM.ViewModel.JsonConverters
{
    public class PeopleConverter : JsonConverter<ObservableCollection<Person>>
    {
        private readonly PeopleConverterContext? _peopleContext;

        public PeopleConverter() { }

        public PeopleConverter(PeopleConverterContext peopleContext)
        {
            this._peopleContext = peopleContext;
        }

        public override ObservableCollection<Person>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (_peopleContext == null)
                throw new NullReferenceException();

            ObservableCollection<Person>? people = JsonSerializer.Deserialize<ObservableCollection<Person>>(ref reader);

            if (people == null)
                throw new NullReferenceException();

            Dictionary<int, Person> peopleMap = people.ToDictionary(p => p.ID);
            this._peopleContext.PeopleMap = peopleMap;
             
            return people;
        }

        public override void Write(Utf8JsonWriter writer, ObservableCollection<Person> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (Person p in value)
            {
                JsonSerializer.Serialize(writer, p, options);
            }
            writer.WriteEndArray();
        }
    }
}
