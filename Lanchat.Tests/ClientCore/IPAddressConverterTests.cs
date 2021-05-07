using System.Net;
using System.Text.Json;
using Lanchat.Core.Json;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.ClientCore
{
    public class IpAddressConverterTests
    {
        private const string SerializedModel = "{\"IpAddress\":\"127.0.0.1\"}";

        private static readonly IpAddressModel Model = new()
        {
            IpAddress = IPAddress.Loopback
        };

        private static JsonSerializerOptions JsonSerializerOptions =>
            new()
            {
                Converters =
                {
                    new IpAddressConverter()
                }
            };

        [Test]
        public void Serialize()
        {
            var json = JsonSerializer.Serialize(Model, JsonSerializerOptions);
            Assert.AreEqual(SerializedModel, json);
        }

        [Test]
        public void Deserialize()
        {
            var json = JsonSerializer.Deserialize<IpAddressModel>(SerializedModel, JsonSerializerOptions);
            Assert.AreEqual(Model.IpAddress, json?.IpAddress);
        }
    }
}