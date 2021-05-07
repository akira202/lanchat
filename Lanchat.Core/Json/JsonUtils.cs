using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.Core.Json
{
    internal class JsonUtils
    {
        internal readonly List<Type> KnownModels = new();
        private readonly JsonSerializerOptions serializerOptions;

        public JsonUtils()
        {
            serializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter(),
                    new IpAddressConverter()
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

        internal string Serialize(object content)
        {
            var data = new Dictionary<string, object> {{content.GetType().Name, content}};
            return JsonSerializer.Serialize(data, serializerOptions);
        }
    }
}