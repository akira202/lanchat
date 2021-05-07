using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;
using Lanchat.Tests.Mock.Network;
using Lanchat.Tests.Mock.Tcp;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Api
{
    public class OutputTests
    {
        private HostMock hostMock;
        private NodeMock nodeMock;
        private Output output;

        [SetUp]
        public void Setup()
        {
            hostMock = new HostMock();
            nodeMock = new NodeMock
            {
                Ready = true
            };
            output = new Output(hostMock, nodeMock);
        }

        [Test]
        public void SendToReadyNode()
        {
            var dataReceived = false;
            hostMock.DataReceived += (_, _) => { dataReceived = true; };
            output.SendData(new Model());
            Assert.IsTrue(dataReceived);
        }

        [Test]
        public void SendToNotReadyNode()
        {
            var dataReceived = false;
            hostMock.DataReceived += (_, _) => { dataReceived = true; };
            nodeMock.Ready = false;
            output.SendData(new Model());
            Assert.IsFalse(dataReceived);
        }

        [Test]
        public void SendPrivilegedData()
        {
            var dataReceived = false;
            hostMock.DataReceived += (_, _) => { dataReceived = true; };
            nodeMock.Ready = false;
            output.SendPrivilegedData(new Model());
            Assert.IsTrue(dataReceived);
        }
    }
}