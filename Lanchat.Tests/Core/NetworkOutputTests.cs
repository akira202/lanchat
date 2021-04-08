using Lanchat.Core.API;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core
{
    public class NetworkOutputTests
    {
        private NetworkOutput networkOutput;
        private NetworkMock networkMock;
        private NodeState nodeState;

        [SetUp]
        public void Setup()
        {
            networkMock = new NetworkMock();
            nodeState = new NodeState();
            networkOutput = new NetworkOutput(networkMock, nodeState);
        }

        [Test]
        public void SendToReadyNode()
        {
            var dataReceived = false;
            networkMock.DataReceived += (_, _) => { dataReceived = true; };
            networkOutput.SendData(new Model());
            Assert.IsTrue(dataReceived);
        }

        [Test]
        public void SendToNotReadyNode()
        {
            var dataReceived = false;
            networkMock.DataReceived += (_, _) => { dataReceived = true; };
            nodeState.Ready = false;
            networkOutput.SendData(new Model());
            Assert.IsFalse(dataReceived);
        }

        [Test]
        public void SendPrivilegedData()
        {
            var dataReceived = false;
            networkMock.DataReceived += (_, _) => { dataReceived = true; };
            nodeState.Ready = false;
            networkOutput.SendPrivilegedData(new Model());
            Assert.IsTrue(dataReceived);
        }
    }
}