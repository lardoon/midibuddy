using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace MidiBuddyWeb.Controllers
{
    public class DrumKit
    {
        public string Note { get; set; }
        [JsonProperty("Octave (Middle C3)")]
        public string OctaveMiddleC3 { get; set; }
        [JsonProperty("Octave (Middle C4)")]
        public string OctaveMiddleC4 { get; set; }

        [JsonConverter(typeof(DictionaryToJsonObjectConverter))]
        public Dictionary<string, string> Kits { get; set; }
    }

    public class DictionaryToJsonObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Dictionary<string, string>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Dictionary<string, string> dict = null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                dict = new Dictionary<string, string>();
                reader.Read();
                while (reader.TokenType != JsonToken.EndObject)
                {
                    var key = reader.Value.ToString();
                    reader.Read();
                    var value = reader.Value?.ToString();
                    dict.Add(key, value);
                    reader.Read();
                }
            }
           
            return dict;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(JsonConvert.SerializeObject(value, Formatting.Indented));
        }
    }
}