using System.Collections.Generic;
using System.Linq;
using Lanchat.Core.Api;
using Lanchat.Core.Channels;
using Lanchat.Core.Network;
using Lanchat.Tests.Mock.Api;
using Lanchat.Tests.Mock.Models;
using Lanchat.Tests.Mock.Network;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Api
{
    public class ChannelTests
    {
        private readonly List<INodeInternal> nodes = new();
        private readonly List<OutputMock> outputMocks = new();
        private IChannel channel;

        [OneTimeSetUp]
        public void Setup()
        {
            outputMocks.Add(new OutputMock());
            outputMocks.Add(new OutputMock());
            nodes.Add(new NodeMock(outputMocks.ElementAt(0)));
            nodes.Add(new NodeMock(outputMocks.ElementAt(1)));
            channel = new Channel("test");
            channel.Nodes.AddRange(nodes.Cast<INode>());
        }

        [Test]
        public void SendMessage()
        {
            channel.SendMessage("test");
            Assert.IsTrue(outputMocks.All(x => x.LastOutput != null));
        }

        [Test]
        public void SendData()
        {
            channel.SendData(new Model());
            Assert.IsTrue(outputMocks.All(x => x.LastOutput != null));
        }
    }
}