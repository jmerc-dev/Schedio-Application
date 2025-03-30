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
    public class RoomsConverter : JsonConverter<ObservableCollection<Room>>
    {
        private readonly RoomTypesConverterContext? _RoomTypesConverterContext;

        public RoomsConverter(RoomTypesConverterContext roomTypesConverterContext)
        {
            this._RoomTypesConverterContext = roomTypesConverterContext;
        }

        public RoomsConverter() { }

        public override ObservableCollection<Room>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ObservableCollection<Room>? rooms = JsonSerializer.Deserialize<ObservableCollection<Room>>(ref reader);

            if (rooms == null || _RoomTypesConverterContext == null || _RoomTypesConverterContext.RoomTypeMap == null)
            {
                return null;
            }

            foreach (Room r in rooms)
            {

                if (_RoomTypesConverterContext.RoomTypeMap.TryGetValue(r.RoomTypeID, out RoomType? type))
                {
                    if (type != null)
                    {
                        r.Type = type;
                    }
                }
            }

            return rooms;
        }

        public override void Write(Utf8JsonWriter writer, ObservableCollection<Room> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (Room r in value)
            {
                writer.WriteStartObject();
                writer.WriteNumber("ID", r.ID);
                writer.WriteString("Name", r.Name);
                writer.WriteNumber("RoomTypeID", r.Type.ID);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}
