using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    var node = new Node(session, true);
                    IncomingConnections.Add(node);
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

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
            Trace.WriteLine($"Server errored: {error}");
        }
    }
}