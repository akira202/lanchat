using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.Core.API
{
    internal class JsonReader
    {
        internal readonly List<Type> KnownModels = new();
        private readonly JsonSerializerOptions serializerOptions;

        public JsonReader()
        {
            serializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
        }

        internal T Deserialize<T>(string item)
        {
            var wrapper = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item, serializerOptions);
            return JsonSerializer.Deserialize<T>(wrapper!.Values.First().ToString()!, serializerOptions);
        }

        internal object Deserialize(string item)
        {
            var wrapper = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item, serializerOptions);
            var type = KnownModels.First(x => x.Name == wrapper?.Keys.First());
            var serializedContent = wrapper?.Values.First().ToString();
            return JsonSerializer.Deserialize(serializedContent ?? string.Empty, type, serializerOptions);
        }
    }
}