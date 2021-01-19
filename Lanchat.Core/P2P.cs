using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class P2P
    {
        private readonly BroadcastService broadcastService;
        private readonly List<Broadcast> detectedNodes;
        private readonly List<Node> outgoingConnections;
        private readonly Server server;

        /// <summary>
        ///     Initialize p2p mode.
        /// </summary>
        public P2P()
        {
            outgoingConnections = new List<Node>();
            detectedNodes = new List<Broadcast>();

            server = new Server(IPAddress.IPv6Any, CoreConfig.ServerPort);
            server.SessionCreated += OnSessionCreated;

            CoreConfig.PropertyChanged += CoreConfigOnPropertyChanged;

            broadcastService = new BroadcastService();
            broadcastService.BroadcastReceived += BroadcastReceived;
        }

        /// <summary>
        ///     List of connected nodes.
        /// </summary>
        public List<Node> Nodes
        {
            get
            {
                var nodes = new List<Node>();
                nodes.AddRange(outgoingConnections);
                nodes.AddRange(server.IncomingConnections);
                return nodes.Where(x => x.Ready).ToList();
            }
        }

        /// <summary>
        ///     List of detected nodes.
        /// </summary>
        public List<Broadcast> DetectedNodes
        {
            get
            {
                var list = new List<Broadcast>();
                detectedNodes.ForEach(x =>
                {
                    if (!Nodes.Any(y => Equals(y.Endpoint.Address, x.IpAddress))) list.Add(x);
                });
                return list;
            }
        }

        private void CoreConfigOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nickname":
                    Nodes.ForEach(x => x.NetworkOutput.SendNicknameUpdate(CoreConfig.Nickname));
                    break;

                case "Status":
                    Nodes.ForEach(x => x.NetworkOutput.SendStatusUpdate(CoreConfig.Status));
                    break;
            }
        }

        /// <summary>
        ///     New node connected. After receiving this handlers for node events can be created.
        /// </summary>
        public event EventHandler<Node> ConnectionCreated;

        /// <summary>
        ///     New node detected in network.
        /// </summary>
        public event EventHandler<Broadcast> NodeDetected;

        /// <summary>
        ///     Detected node has changed its nickname.
        /// </summary>
        public event EventHandler<Broadcast> DetectedNodeChanged;

        /// <summary>
        ///     Detected node doesn't send broadcasts.
        /// </summary>
        public event EventHandler<Broadcast> DetectedNodeDisappeared;

        /// <summary>
        ///     Start server.
        /// </summary>
        public void StartServer()
        {
            server.Start();
        }

        /// <summary>
        ///     Start broadcasting.
        /// </summary>
        public void StartBroadcast()
        {
            broadcastService.Start();
        }

        /// <summary>
        ///     Send message to all nodes.
        /// </summary>
        /// <param name="message">Message content.</param>
        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.NetworkOutput.SendMessage(message));
        }

        /// <summary>
        ///     Connect to node.
        /// </summary>
        /// <param name="ipAddress">Node IP address.</param>
        /// <param name="port">Node port.</param>
        public void Connect(IPAddress ipAddress, int? port = null)
        {
            // Use port from config if it's null
            port ??= CoreConfig.ServerPort;

            // Throw if node is blocked
            if (CoreConfig.BlockedAddresses.Contains(ipAddress)) throw new ArgumentException("Node blocked");

            // Throw if node already connected
            if (Nodes.Any(x => x.Endpoint.Address.Equals(ipAddress)))
                throw new ArgumentException("Already connected to this node");

            // Throw if local address
            var host = Dns.GetHostEntry(Dns.GetHostName());
            if (host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Contains(ipAddress))
                throw new ArgumentException("Illegal IP address. Cannot connect");

            var client = new Client(ipAddress, port.Value);
            var node = new Node(client, false);

            outgoingConnections.Add(node);
            node.Connected += OnConnected;
            node.HardDisconnect += OnHardDisconnect;
            node.CannotConnect += OnCannotConnect;
            node.NetworkInput.NodesListReceived += OnNodesListReceived;
            ConnectionCreated?.Invoke(this, node);
            client.ConnectAsync();
        }

        // Exchange nodes list
        private void OnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var nodesList = Nodes.Where(x => x.Id != node.Id).Select(x => x.Endpoint.Address).ToList();
            node.NetworkOutput.SendNodesList(nodesList);
        }

        // Send new node event after new session and wait for nodes list
        private void OnSessionCreated(object sender, Node node)
        {
            ConnectionCreated?.Invoke(this, node);
            node.NetworkInput.NodesListReceived += OnNodesListReceived;
            node.Connected += OnConnected;
        }

        // Dispose node after hard disconnection
        private void OnHardDisconnect(object sender, EventArgs e)
        {
            var node = (Node) sender;
            outgoingConnections.Remove(node);
            node.Dispose();
        }

        // Dispose when connection cannot be established
        private void OnCannotConnect(object sender, EventArgs e)
        {
            var node = (Node) sender;
            outgoingConnections.Remove(node);
            node.Dispose();
        }

        // Try connect to every node from list
        private void OnNodesListReceived(object sender, List<IPAddress> list)
        {
            if (!CoreConfig.AutomaticConnecting) return;
            list.ForEach(x =>
            {
                try
                {
                    Connect(x);
                }
                catch (Exception e)
                {
                    if (e is not ArgumentException)
                    {
                        throw;
                    }
                }
            });
        }

        // UDP broadcast received
        private void BroadcastReceived(object sender, Broadcast e)
        {
            var alreadyDetected = detectedNodes.FirstOrDefault(x => Equals(x.IpAddress, e.IpAddress));
            if (alreadyDetected == null)
            {
                detectedNodes.Add(e);
                e.Active = true;
                NodeDetected?.Invoke(this, e);

                var timer = new Timer
                {
                    Interval = 2500,
                    Enabled = true
                };

                timer.Elapsed += (_, _) =>
                {
                    if (e.Active)
                    {
                        e.Active = false;
                    }
                    else
                    {
                        timer.Dispose();
                        DetectedNodeDisappeared?.Invoke(this, e);
                        detectedNodes.Remove(e);
                    }
                };
            }
            else
            {
                alreadyDetected.Active = true;
                if (alreadyDetected.Nickname == e.Nickname) return;
                alreadyDetected.Nickname = e.Nickname;
                DetectedNodeChanged?.Invoke(this, alreadyDetected);
            }
        }
    }
}