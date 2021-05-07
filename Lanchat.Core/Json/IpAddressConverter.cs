using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.Core.Json
{
    /// <inheritdoc />
    public class IpAddressConverter : JsonConverter<IPAddress>
    {
        /// <inheritdoc />
        public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return IPAddress.Parse(reader.GetString() ?? throw new InvalidOperationException());
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}