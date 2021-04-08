using Lanchat.Core.API;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Handlers;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
{
    public class NetworkInputTests
    {
        private JsonBuffer jsonBuffer;
        private Resolver resolver;
        private ModelHandlerMock modelHandlerMock;

        [SetUp]
        public void Setup()
        {
            modelHandlerMock = new ModelHandlerMock();
            resolver = new Resolver(new NodeState());
            resolver.RegisterHandler(modelHandlerMock);
            resolver.RegisterHandler(new ModelWithValidationHandlerMock());
            jsonBuffer = new JsonBuffer();
        }

        [Test]
        public void JsonSplit()
        {
            var fullString = NetworkOutput.Serialize(new Model());
            var firstPart = fullString.Substring(0, 10);
            var secondPart = fullString.Substring(10);
            jsonBuffer.AddToBuffer(firstPart);
            jsonBuffer.ReadBuffer().ForEach(resolver.CallHandler);
            jsonBuffer.AddToBuffer(secondPart);
            jsonBuffer.ReadBuffer().ForEach(resolver.CallHandler);
            Assert.True(modelHandlerMock.Handled);
        }
    }
}