using System;
using Lanchat.Core.API;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Handlers;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
{
    public class ResolverTests
    {
        private MessageHandlerMock messageHandlerMock;
        private NodeState nodeState;
        private PrivilegedHandler privilegedHandler;
        private Resolver resolver;

        [SetUp]
        public void Setup()
        {
            nodeState = new NodeState();
            resolver = new Resolver(nodeState);
            messageHandlerMock = new MessageHandlerMock();
            privilegedHandler = new PrivilegedHandler();
            resolver.RegisterHandler(messageHandlerMock);
            resolver.RegisterHandler(privilegedHandler);
        }

        [Test]
        public void UnknownModel()
        {
            Assert.Catch<InvalidOperationException>(() =>
            {
                resolver.CallHandler(NetworkOutput.Serialize(new Model()));
            });
        }

        [Test]
        public void NoHandler()
        {
            Assert.Catch<InvalidOperationException>(() =>
            {
                resolver.CallHandler(NetworkOutput.Serialize(new Handshake()));
            });
        }

        [Test]
        public void NodeNotReady()
        {
            nodeState.Ready = false;
            resolver.CallHandler(NetworkOutput.Serialize(new Message()));
            Assert.IsFalse(messageHandlerMock.Received);
        }

        [Test]
        public void PrivilegedHandler()
        {
            nodeState.Ready = false;
            resolver.CallHandler(NetworkOutput.Serialize(new PrivilegedModel()));
            Assert.IsTrue(privilegedHandler.Received);
        }

        [Test]
        public void ValidModel()
        {
            resolver.OnDataReceived(this, NetworkOutput.Serialize(new Message {Content = "test"}));
            Assert.IsTrue(messageHandlerMock.Received);
        }

        [Test]
        public void InvalidModel()
        {
            resolver.OnDataReceived(this, NetworkOutput.Serialize(new Message {Content = null}));
            Assert.IsFalse(messageHandlerMock.Received);
        }

        [Test]
        public void InvalidOperationExceptionCatch()
        {
            resolver.OnDataReceived(this, "{}");
        }

        [Test]
        public void JsonExceptionCatch()
        {
            resolver.OnDataReceived(this, "{\"key\": invalid format}");
        }

        [Test]
        public void ValidationExceptionCatch()
        {
            resolver.OnDataReceived(this, NetworkOutput.Serialize(new ModelWithValidation()));
        }
    }
}