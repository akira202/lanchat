using System;
using System.Diagnostics;
using System.Linq;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2PHandlers
{
    internal class P2PInternalHandlers
    {
        private readonly P2P network;

        internal P2PInternalHandlers(P2P network)
        {
            this.network = network;
            network.Server.SessionCreated += OnSessionCreated;
        }

        internal void CloseNode(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var id = node.Id;
            network.OutgoingConnections.Remove(node);
            node.Dispose();
            Trace.WriteLine($"Node {id} disposed");
        }

        internal void OnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var nodesList = new NodesList();
            nodesList.AddRange(network.Nodes.Where(x => x.Id != node.Id)
                .Select(x => x.NetworkElement.Endpoint.Address.ToString()));
            node.NetworkOutput.SendData(nodesList);
            
            if (!network.Config.SavedAddresses.Contains(node.NetworkElement.Endpoint.Address))
            {
                network.Config.SavedAddresses.Add(node.NetworkElement.Endpoint.Address);
            }
        }

        private void OnSessionCreated(object sender, Node node)
        {
            network.OnNodeCreated(node);
            node.Resolver.RegisterHandler(new NodesListHandler(network));
            node.Connected += OnConnected;
        }
    }
}