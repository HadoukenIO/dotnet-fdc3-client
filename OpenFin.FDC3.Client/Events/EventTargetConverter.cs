using System;
using Newtonsoft.Json;

namespace OpenFin.FDC3.Events
{
    public class EventTargetConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String && serializer.Deserialize<string>(reader) == "default")
            {
                // TBD: How to handle events targeted at the 'default' emitter.
                // Currently using a "null, null" target to represent the default emitter.
                return new EventTransportTarget();
            }
            else
            {
                return serializer.Deserialize(reader, objectType);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // We don't need to send EventTarget objects to the provider, we will only ever receive them
            throw new NotImplementedException();
        }
    }
}