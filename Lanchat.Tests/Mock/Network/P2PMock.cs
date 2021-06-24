using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lanchat.Core.Api;
using Lanchat.Core.Channels;
using Lanchat.Core.Network;
using Lanchat.Core.NodesDetection;
using Lanchat.Tests.Mock.Config;

namespace Lanchat.Tests.Mock.Network
{
    public class P2PMock : IP2P
    {
        public P2PMock()
        {
            NodesDetection = new NodesDetector(new ConfigMock());
            Channels = new ChannelsControl();
        }

        public List<IPAddress> Connected { get; } = new();
        public NodesDetector NodesDetection { get; }
        public List<INode> Nodes { get; } = new();
        public ChannelsControl Channels { get; }

        public event EventHandler<INode> NodeCreated;

        public void Start()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Connect(IPAddress ipAddress, int? port = null)
        {
            if (Connected.Contains(ipAddress))
            {
                throw new ArgumentException("Already connected");
            }

            Connected.Add(ipAddress);
            return new Task<bool>(() => true);
        }
    }
}