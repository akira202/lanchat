using System;
using Lanchat.Core.API;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
{
    public class JsonReaderTests
    {
        private JsonReader jsonReader;

        [SetUp]
        public void Setup()
        {
            jsonReader = new JsonReader();
        }

        [Test]
        public void KnownTypeDeserialize()
        {
            var model = new Model {Property = "test"};
            var serializedModel = NetworkOutput.Serialize(model);
            var deserializedModel = jsonReader.Deserialize<Model>(serializedModel);
            Assert.AreEqual(model.Property, deserializedModel.Property);
        }

        [Test]
        public void RegisteredTypeDeserialize()
        {
            jsonReader.KnownModels.Add(typeof(Model));
            var model = new Model {Property = "test"};
            var serializedModel = NetworkOutput.Serialize(model);
            var deserializedModel = (Model) jsonReader.Deserialize(serializedModel);
            Assert.AreEqual(model.Property, deserializedModel.Property);
        }

        [Test]
        public void NullWrapper()
        {
            Assert.Catch<InvalidOperationException>(() => jsonReader.Deserialize("{\"key\": \"value\"}"));
        }
    }
}