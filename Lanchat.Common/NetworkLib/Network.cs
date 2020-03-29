﻿using Lanchat.Common.Cryptography;
using Lanchat.Common.NetworkLib.Api;
using Lanchat.Common.NetworkLib.Exceptions;
using Lanchat.Common.NetworkLib.Host;
using Lanchat.Common.NetworkLib.Node;
using Lanchat.Common.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    ///  Main class of network lib.
    /// </summary>
    public class Network : IDisposable
    {
        private readonly HostInstance host;
        private string nickname;

        /// <summary>
        /// Network constructor.
        /// </summary>
        /// <param name="broadcastPort">UDP broadcast port</param>
        /// <param name="nickname">Self nickname</param>
        /// <param name="hostPort">TCP host port. Set to -1 to use free ephemeral port</param>
        /// <param name="heartbeatTimeout">Heartbeat lifetime in ms</param>
        public Network(int broadcastPort, string nickname, int hostPort = -1, int heartbeatTimeout = 5000)
        {
            Rsa = new Rsa();
            NodeList = new List<NodeInstance>();
            Nickname = nickname;
            PublicKey = Rsa.PublicKey;
            BroadcastPort = broadcastPort;
            HeartbeatTimeout = heartbeatTimeout;
            Id = Guid.NewGuid();

            if (hostPort == -1)
            {
                HostPort = FreeTcpPort();
            }
            else
            {
                HostPort = hostPort;
            }

            host = new HostInstance(BroadcastPort);
            _ = new HostEventsHandlers(this, host);
            Events = new Events();
            Methods = new Methods(this);

            Trace.WriteLine("[NETWORK] Network initialized");
        }

        /// <summary>
        /// Network API inputs class.
        /// </summary>
        public Events Events { get; set; }

        /// <summary>
        /// Network API outputs class.
        /// </summary>
        public Methods Methods { get; set; }

        /// <summary>
        /// Self nickname. On set it sends new nickname to connected client.
        /// </summary>
        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                ChangeNickname(value);
            }
        }

        /// <summary>
        /// All nodes here.
        /// </summary>
        public List<NodeInstance> NodeList { get; }

        internal int BroadcastPort { get; set; }

        internal int HeartbeatTimeout { get; set; }

        internal int HostPort { get; set; }

        internal Guid Id { get; set; }

        internal string PublicKey { get; set; }

        internal Rsa Rsa { get; set; }

        /// <summary>
        /// Start host, broadcast and listen.
        /// </summary>
        public void Start()
        {
            host.StartHost(HostPort);
            Events.OnHostStarted(HostPort);
            host.StartBroadcast(new Paperplane(HostPort, Id));
            host.ListenBroadcast();
        }

        internal void CheckNickcnameDuplicates(string nickname)
        {
            var nodes = NodeList.FindAll(x => x.ClearNickname == nickname);
            if (nodes.Count > 1)
            {
                var index = 1;
                foreach (var item in nodes)
                {
                    item.NicknameNum = index;
                    index++;
                }
            }
            else if (nodes.Count > 0)
            {
                nodes[0].NicknameNum = 0;
            }
        }

        internal void CloseNode(NodeInstance node)
        {
            var nickname = node.ClearNickname;
            Trace.WriteLine($"[NETWORK] Node disconnected ({node.Ip})");
            Events.OnNodeDisconnected(node);
            NodeList.Remove(node);
            node.Dispose();
            CheckNickcnameDuplicates(nickname);
        }

        internal void CreateNode(IPAddress ip = null, int port = 0, bool manual = false, Socket socket = null)
        {
            // Get ip form socket
            if (ip == null)
            {
                ip = IPAddress.Parse(((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
            }

            // Check is node with same ip alredy exist
            if (Methods.GetNode(ip) == null)
            {
                var node = new NodeInstance(ip, this);
                NodeList.Add(node);
                if (socket != null)
                {
                    node.Socket = socket;
                    node.StartProcess();
                }

                // Create a connection if the port is known
                if (port != 0)
                {
                    node.Port = port;
                    node.CreateConnection();
                    node.Client.SendHandshake(new Handshake(Nickname, PublicKey, HostPort));
                    node.Client.SendList(NodeList);
                }
                else
                {
                    Trace.WriteLine($"[NETWORK] One way connection. Waiting for handshake ({node.Ip})");
                }

                Trace.WriteLine($"[NETWORK] Node created successful ({node.Ip}:{node.Port.ToString(CultureInfo.CurrentCulture)})");
            }
            else
            {
                Trace.WriteLine($"[NETWORK] Node already exist ({ip})");
                if (manual)
                {
                    throw new NodeAlreadyExistException();
                }
            }
        }

        // Find free tcp port
        private static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        // Change nickname
        private void ChangeNickname(string value)
        {
            NodeList.ForEach(x =>
            {
                if (x.Client != null)
                {
                    x.Client.SendNickname(value);
                }
            });
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Dispose network.
        /// </summary>
        ~Network()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose network.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose network.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    host.Dispose();
                    Rsa.Dispose();
                }

                disposedValue = true;
            }
        }
        #endregion IDisposable Support
    }
}