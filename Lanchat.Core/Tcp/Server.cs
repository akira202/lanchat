using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Config;
using Lanchat.Core.Network;
using NetCoreServer;

namespace Lanchat.Core.Tcp
{
    internal class Server : TcpServer
    {
        private readonly IConfig config;
        private readonly NodesControl nodesControl;

        internal Server(IPAddress address, int port, IConfig config, NodesControl nodesControl) : base(address, port)
        {
            this.config = config;
            this.nodesControl = nodesControl;
            OptionDualMode = true;
        }

        protected override void OnStarted()
        {
            Trace.WriteLine($"Server listening on {Endpoint.Port}");
            base.OnStarted();
        }

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            session.Connected += SessionOnConnected;
            return session;
        }

        private void SessionOnConnected(object sender, EventArgs e)
        {
            var session = (Session) sender;
            if (config.BlockedAddresses.Contains(session.Endpoint.Address))
            {
                Trace.WriteLine($"Connection from {session.Endpoint.Address} blocked");
                session.Close();
                return;
            }

            nodesControl.CreateNode(session);
            Trace.WriteLine($"Session for {session.Endpoint.Address} created. Session ID: {session.Id}");
        }

        protected override void OnError(SocketError error)
        {
            Trace.WriteLine($"Server errored: {error}");
        }
    }
}