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
                    node.Connected += NodeOnConnected;
                    node.HardDisconnect += OnHardDisconnected;
                    node.NicknameChanged += NodeOnNicknameChanged;
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

        private void NodeOnMessageReceived(object sender, string e)
        {
            var nodeNetworkInput = (NetworkInput) sender;
            var node = IncomingConnections.First(x => x.Id == nodeNetworkInput.Id);
            Broadcast(node.Id,$"{node.Nickname}: {e}");
        }
        
        private void NodeOnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            Broadcast(node.Id,$"{node.Nickname} connected");
        }
        
        private void OnHardDisconnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            Broadcast(node.Id,$"{node.Nickname} disconnected");
            IncomingConnections.Remove(node);
            node.Dispose();
        }
        
        private void NodeOnNicknameChanged(object sender, string e)
        {
            var node = (Node) sender;
            Broadcast(node.Id,$"{e} changed nickname to {node.Nickname}");
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
            Trace.WriteLine($"Server errored: {error}");
        }

        private void Broadcast(Guid senderId, string message)
        {
            IncomingConnections.Where(x => x.Id != senderId).ToList().ForEach(x =>
            {
                x.NetworkOutput.SendMessage(message);
            });
        }
    }
}