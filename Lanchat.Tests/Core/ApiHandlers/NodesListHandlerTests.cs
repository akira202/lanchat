using System.Net;
using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock.Config;
using Lanchat.Tests.Mock.Network;
using NUnit.Framework;

namespace Lanchat.Tests.Core.ApiHandlers
{
    public class NodesListHandlerTests
    {
        private ConfigMock configMock;
        private P2PMock network;
        private NodesListHandler nodesListHandler;

        [SetUp]
        public void Setup()
        {
            network = new P2PMock();
            configMock = new ConfigMock {ConnectToReceivedList = true};
            nodesListHandler = new NodesListHandler(configMock, network);
        }

        [Test]
        public void IgnoreLoopback()
        {
            var nodesList = new NodesList {IPAddress.Loopback};
            nodesListHandler.Handle(nodesList);

            Assert.IsFalse(network.Connected.Contains(IPAddress.Loopback));
        }

        [Test]
        public void ConnectToList()
        {
            var address1 = IPAddress.Parse("1.1.1.1");
            var address2 = IPAddress.Parse("1.1.1.2");
            var nodesList = new NodesList {address1, address2};
            nodesListHandler.Handle(nodesList);

            Assert.IsTrue(network.Connected.Contains(address1));
            Assert.IsTrue(network.Connected.Contains(address2));
        }

        [Test]
        public void PreventDuplicates()
        {
            var address1 = IPAddress.Parse("1.1.1.1");
            var address2 = IPAddress.Parse("1.1.1.1");
            var nodesList = new NodesList {address1, address2};
            nodesListHandler.Handle(nodesList);

            Assert.IsTrue(network.Connected.Contains(address1));
            Assert.AreEqual(1, network.Connected.Count);
        }

        [Test]
        public void ConnectToListDisabled()
        {
            configMock.ConnectToReceivedList = false;
            var address1 = IPAddress.Parse("1.1.1.1");
            var nodesList = new NodesList {address1};
            nodesListHandler.Handle(nodesList);

            Assert.IsFalse(network.Connected.Contains(address1));
        }
    }
}