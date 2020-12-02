using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Relay : TcpServer
    {
        public Relay(IPAddress address, int port) : base(address, port)
        {
            IncomingConnections = new List<Node>();
        }

        public List<Node> IncomingConnections { get; }

        public event EventHandler<SocketError> ServerErrored;

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);

            session.Connected += (sender, args) =>
            {
                if (CoreConfig.BlockedAddresses.Contains(session.Endpoint.Address))
                {
                    Trace.WriteLine($"Connection from {session.Endpoint.Address} blocked");
                    session.Close();
                }
                else
                {
                    var node = new Node(session, true, true);
                    IncomingConnections.Add(node);
                    node.NetworkInput.MessageReceived += NodeOnMessageReceived;
                    node.HardDisconnect += OnHardDisconnected;
                    Trace.WriteLine($"Session for {session.Endpoint.Address} created. Session ID: {session.Id}");
                }
            };

            return session;
        }

        protected override void OnStarted()
        {
            Trace.WriteLine($"Relay listening on {Endpoint.Port}");
            base.OnStarted();
        }

        private void OnHardDisconnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            IncomingConnections.Remove(node);
            node.Dispose();
        }

        private void NodeOnMessageReceived(object sender, string e)
        {
            var nodeNetworkInput = (NetworkInput) sender;
            var node = IncomingConnections.First(x => x.Id == nodeNetworkInput.Id);
            Trace.WriteLine($"Broadcasting message from {nodeNetworkInput.Id}");

            IncomingConnections.Where(x => x.Id != nodeNetworkInput.Id).ToList().ForEach(x =>
            {
                x.NetworkOutput.SendMessage($"{node.Nickname}: {e}");
            });
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
            Trace.WriteLine($"Server errored: {error}");
        }
    }
}