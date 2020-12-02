using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class Node : IDisposable
    {
        private string nickname;
        private readonly IPEndPoint firstEndPoint;

        internal readonly Encryption Encryption;
        internal readonly INetworkElement NetworkElement;
        
        public readonly NetworkInput NetworkInput;
        public readonly NetworkOutput NetworkOutput;

        /// <summary>
        ///     Initialize node.
        /// </summary>
        /// <param name="networkElement">TCP client or session.</param>
        /// <param name="sendHandshake">Send handshake immediately</param>
        /// <param name="relayNode">Is node created in relay</param>
        public Node(INetworkElement networkElement, bool sendHandshake, bool relayNode = false)
        {
            NetworkElement = networkElement;
            NetworkOutput = new NetworkOutput(this);
            NetworkInput = new NetworkInput(this);
            Encryption = new Encryption();
            
            firstEndPoint = networkElement.Endpoint;

            networkElement.Disconnected += OnDisconnected;
            networkElement.SocketErrored += OnSocketErrored;
            networkElement.DataReceived += NetworkInput.ProcessReceivedData;

            NetworkInput.HandshakeReceived += OnHandshakeReceived;
            NetworkInput.KeyInfoReceived += OnKeyInfoReceived;
            NetworkInput.NicknameChanged += OnNicknameChanged;

            if (sendHandshake)
            {
                SendHandshakeAndWait(relayNode);
            }
            else
            {
                networkElement.Connected += OnConnected;
            }
        }

        /// <summary>
        ///     Node nickname.
        /// </summary>
        public string Nickname
        {
            get => RelayNode ? $"{nickname}#RELAY" : $"{nickname}#{ShortId}";
            private set => nickname = value;
        }

        /// <summary>
        ///     Node ready. If set to false node won't send or receive messages.
        /// </summary>
        public bool Ready { get; private set; }
        
        public bool RelayNode { get; internal set; }

        /// <summary>
        ///     ID of TCP client or session.
        /// </summary>
        public Guid Id => NetworkElement.Id;

        /// <summary>
        ///     Short ID.
        /// </summary>
        public string ShortId => Id.GetHashCode().ToString().Substring(1, 4);

        /// <summary>
        ///     IP address of node.
        /// </summary>
        public IPEndPoint Endpoint
        {
            get
            {
                // Return endpoint from network element.
                try
                {
                    return NetworkElement.Endpoint;
                }
                
                // Or from local variable if network element is disposed.
                catch (ObjectDisposedException)
                {
                    return firstEndPoint;
                }
            }
        }

        /// <summary>
        ///     Is node reconnecting.
        /// </summary>
        public bool UnderReconnecting { get; private set; }

        /// <summary>
        ///     Node successful connected and ready.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        ///     Node disconnected. Trying reconnect.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        ///     Node disconnected. Cannot reconnect.
        /// </summary>
        public event EventHandler HardDisconnect;

        /// <summary>
        ///     Raise when connection try failed.
        /// </summary>
        public event EventHandler CannotConnect;
            
        /// <summary>
        ///     User changed nickname of node. Returns previous nickname in parameter.
        /// </summary>
        public event EventHandler<string> NicknameChanged;

        /// <summary>
        ///     TCP session or client for this node returned error.
        /// </summary>
        public event EventHandler<SocketError> SocketErrored;

        /// <summary>
        /// Disconnect from node.
        /// </summary>
        public void Disconnect()
        {
            NetworkOutput.SendGoodbye();
            NetworkElement.Close();
        }

        // Network elements events.

        private void OnConnected(object sender, EventArgs e)
        {
            SendHandshakeAndWait();
        }

        private void OnDisconnected(object sender, bool hardDisconnect)
        {
            UnderReconnecting = !hardDisconnect;

            // Raise event only if node was ready before.
            if (hardDisconnect && !Ready)
            {
                Trace.WriteLine($"Cannot connect {Id} / {Endpoint}");
                CannotConnect?.Invoke(this, EventArgs.Empty);
            }
            else if (hardDisconnect && Ready)
            {
                HardDisconnect?.Invoke(this, EventArgs.Empty);
            }
            else if (Ready)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }

            Ready = false;
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            SocketErrored?.Invoke(this, e);
        }

        // Network Input events.

        private void OnHandshakeReceived(object sender, Handshake handshake)
        {
            Nickname = handshake.Nickname;
            Encryption.ImportPublicKey(handshake.PublicKey);
            NetworkOutput.SendKey();
        }

        private void OnKeyInfoReceived(object sender, KeyInfo e)
        {
            Encryption.ImportAesKey(e);
            Ready = true;
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void OnNicknameChanged(object sender, string e)
        {
            if (e == Nickname)
            {
                return;
            }

            var previousNickname = Nickname;
            Nickname = e;
            NicknameChanged?.Invoke(this, previousNickname);
        }

        private void SendHandshakeAndWait(bool relayNode = false)
        {
            NetworkOutput.SendHandshake(relayNode);

            // Check is connection established successful after timeout.
            Task.Delay(5000).ContinueWith(t =>
            {
                if (!Ready && !UnderReconnecting)
                {
                    NetworkElement.Close();
                }
            });
        }

        public void Dispose()
        {
            NetworkElement.Close();
            Encryption.Dispose();
        }
    }
}