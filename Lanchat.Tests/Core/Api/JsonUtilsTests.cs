using System;
using Lanchat.Core.Json;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Api
{
    public class JsonReaderTests
    {
        private JsonUtils jsonUtils;

        [SetUp]
        public void Setup()
        {
            jsonUtils = new JsonUtils();
        }

        [Test]
        public void KnownTypeDeserialize()
        {
            var model = new Model {Property = "test"};
            var serializedModel = jsonUtils.Serialize(model);
            var deserializedModel = jsonUtils.Deserialize<Model>(serializedModel);
            Assert.AreEqual(model.Property, deserializedModel.Property);
        }

        [Test]
        public void RegisteredTypeDeserialize()
        {
            jsonUtils.KnownModels.Add(typeof(Model));
            var model = new Model {Property = "test"};
            var serializedModel = jsonUtils.Serialize(model);
            var deserializedModel = (Model) jsonUtils.Deserialize(serializedModel);
            Assert.AreEqual(model.Property, deserializedModel.Property);
        }

        [Test]
        public void NullWrapper()
        {
            Assert.Catch<InvalidOperationException>(() => jsonUtils.Deserialize("{\"key\": \"value\"}"));
        }
    }
}