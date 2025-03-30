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
    public class RoomTypesConverter : JsonConverter<ObservableCollection<RoomType>>
    {
        private readonly RoomTypesConverterContext? _context;

        public RoomTypesConverter() { }

        public RoomTypesConverter(RoomTypesConverterContext? context)
        {
            this._context = context;
        }

        public override ObservableCollection<RoomType>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //foreach (JsonConverter jc in options.Converters)
            //{
            //    Trace.WriteLine(jc.Type.);
            //}
            var roomTypes = JsonSerializer.Deserialize<ObservableCollection<RoomType>>(ref reader);

            foreach (RoomType rt in roomTypes) 
            {
                Trace.WriteLine($"{rt.ID}, {rt.Name}");
            }
            // Store room types in dictionary and pass it through options
            Dictionary<int, RoomType> roomTypeMap = (Dictionary<int, RoomType>) roomTypes.ToDictionary(rt => rt.ID);
            this._context.RoomTypeMap = roomTypeMap;

            return roomTypes;
        }

        public override void Write(Utf8JsonWriter writer, ObservableCollection<RoomType> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (RoomType type in value)
            {
                JsonSerializer.Serialize(writer, type, options);
            }
            writer.WriteEndArray();
        }
    }
}
