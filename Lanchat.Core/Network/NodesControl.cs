using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Lanchat.Core.Channels;
using Lanchat.Core.Config;
using Lanchat.Core.Network.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network
{
    internal class NodesControl
    {
        private readonly IChannel broadcastChannel;
        private readonly IConfig config;
        private readonly IContainer container;

        internal NodesControl(IChannel broadcastChannel, IConfig config, IContainer container)
        {
            this.broadcastChannel = broadcastChannel;
            this.config = config;
            this.container = container;
            Nodes = new List<INodeInternal>();
        }

        internal List<INodeInternal> Nodes { get; }

        internal INodeInternal CreateNode(IHost host)
        {
            var scope = container.BeginLifetimeScope(b => { b.RegisterInstance(host).As<IHost>(); });
            var node = scope.Resolve<INodeInternal>();
            Nodes.Add(node);
            node.Connected += OnConnected;
            node.CannotConnect += (sender, args) =>
            {
                CloseNode(sender, args);
                scope.Dispose();
            };

            node.Disconnected += (sender, args) =>
            {
                CloseNode(sender, args);
                scope.Dispose();
            };
            node.Start();
            return node;
        }

        private void CloseNode(object sender, EventArgs e)
        {
            var node = (INodeInternal) sender;
            broadcastChannel.Nodes.Remove((INode)node);
            
            var id = node.Id;
            Nodes.Remove(node);
            node.Connected -= OnConnected;
            node.CannotConnect -= CloseNode;
            node.Disconnected -= CloseNode;
            Trace.WriteLine($"Node {id} disposed");
        }

        private void OnConnected(object sender, EventArgs e)
        {
            var node = (INodeInternal) sender;
            broadcastChannel.Nodes.Add((INode)node);
            
            var nodesList = new NodesList();
            nodesList.AddRange(Nodes
                .Where(x => x.Id != node.Id)
                .Select(x => x.Host.Endpoint.Address));
            node.Output.SendData(nodesList);

            if (!config.SavedAddresses.Contains(node.Host.Endpoint.Address))
            {
                config.SavedAddresses.Add(node.Host.Endpoint.Address);
            }
        }
    }
}