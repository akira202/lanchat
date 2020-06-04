﻿using Lanchat.Common.Cryptography;
using Lanchat.Common.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Lanchat.Common.NetworkLib.Node
{
    /// <summary>
    /// Represents network node.
    /// </summary>
    public class NodeInstance : IDisposable
    {
        private Status _State;

        /// <summary>
        /// Node constructor with known port.
        /// </summary>
        /// <param name="ip">Node IP</param>
        /// <param name="network">Network</param>
        /// <param name="reconnect">Node is under reconnecting</param>
        internal NodeInstance(IPAddress ip, Network network, bool reconnect)
        {
            Handlers = new NodeHandlers(network, this);
            ConnectionTimer = new Timer { Interval = network.ConnectionTimeout, Enabled = false };
            HeartbeatSendTimer = new Timer { Interval = network.HeartbeatSendTimeout, Enabled = false };
            HeartbeatReceiveTimer = new Timer { Interval = network.HeartbeatReceiveTimeout, Enabled = false };
            ConnectionTimer.Elapsed += new ElapsedEventHandler(Handlers.OnConnectionTimerElapsed);
            HeartbeatSendTimer.Elapsed += new ElapsedEventHandler(Handlers.OnHeartbeatSendTimer);
            HeartbeatReceiveTimer.Elapsed += new ElapsedEventHandler(Handlers.OnHeartbeatReceiveTimer);
            Ip = ip;
            SelfAes = new Aes();
            NicknameNum = 0;
            State = Status.Waiting;
            Reconnect = reconnect;
            ConnectionTimer.Start();
        }

        /// <summary>
        /// Nickname without number.
        /// </summary>
        public string ClearNickname { get; private set; }

        /// <summary>
        /// Handshake.
        /// </summary>
        public Handshake Handshake { get; set; }

        /// <summary>
        /// Last heartbeat status.
        /// </summary>
        public bool Heartbeat { get; set; }

        /// <summary>
        /// Node IP.
        /// </summary>
        public IPAddress Ip { get; set; }

        /// <summary>
        /// Node mute value.
        /// </summary>
        public bool Mute { get; set; }

        /// <summary>
        /// Node nickname. If nicknames are duplicated returns nickname with number.
        /// </summary>
        public string Nickname
        {
            get
            {
                if (NicknameNum != 0)
                {
                    return ClearNickname + $"#{NicknameNum}";
                }
                else
                {
                    return ClearNickname;
                }
            }
            set
            {
                if (value != null)
                {
                    ClearNickname = value.Replace(" ", "_");
                }
            }
        }

        /// <summary>
        /// Node TCP port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Node <see cref="Status"/>.
        /// </summary>
        public Status State
        {
            get { return _State; }
            set
            {
                var previousState = State;
                _State = value;
                if (previousState != value)
                {
                    Handlers.OnStateChanged();
                }
            }
        }

        internal Client Client { get; set; }
        internal Timer ConnectionTimer { get; set; }
        internal NodeHandlers Handlers { get; set; }
        internal Timer HeartbeatReceiveTimer { get; set; }
        internal Timer HeartbeatSendTimer { get; set; }
        internal int NicknameNum { get; set; }
        internal bool Reconnect { get; set; }
        internal Aes RemoteAes { get; set; }
        internal Aes SelfAes { get; set; }
        internal Socket Socket { get; set; }

        /// <summary>
        /// Send private message.
        /// </summary>
        /// <param name="message">content</param>
        public void SendPrivate(string message)
        {
            Client.SendPrivate(message);
        }

        internal void AcceptHandshake(Handshake handshake)
        {
            Handshake = handshake;
            Nickname = handshake.Nickname;

            if (Port == 0)
            {
                Port = handshake.Port;
                CreateConnection();
                Handlers.OnHandshakeAccepted();
            }

            Client.SendKey(new Key(
                     Rsa.Encode(SelfAes.Key, Handshake.PublicKey),
                     Rsa.Encode(SelfAes.IV, Handshake.PublicKey)));
        }

        internal void CreateConnection()
        {
            Client = new Client(this);
            Client.Connect(Ip, Port);
        }

        internal void CreateRemoteAes(string key, string iv)
        {
            RemoteAes = new Aes(key, iv);
            MakeReady();
        }

        internal void MakeReady()
        {
            State = Status.Ready;
            Reconnect = false;
            HeartbeatReceiveTimer.Start();
            HeartbeatSendTimer.Start();
        }

        internal void Process()
        {
            byte[] streamBuffer;

            while (!disposedValue)
            {
                try
                {
                    // Read stream
                    streamBuffer = new byte[Socket.ReceiveBufferSize];
                    if (Socket.Receive(streamBuffer) == 0)
                    {
                        State = Status.Closed;
                    }

                    var respBytesList = new List<byte>(streamBuffer);
                    var data = Encoding.UTF8.GetString(respBytesList.ToArray());

                    JsonTextReader reader = new JsonTextReader(new StringReader(data))
                    {
                        SupportMultipleContent = true
                    };

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            HandleReceivedData(serializer.Deserialize<JObject>(reader));
                        }
                    }
                }

                catch (Exception e)
                {
                    if (e is ObjectDisposedException)
                    {
                        Trace.WriteLine($"[NODE] Socket already closed ({Ip})");
                        break;
                    }
                    else if (e is DecoderFallbackException)
                    {
                        Trace.WriteLine($"[NODE] Data processing error: utf8 decode gone wrong ({Ip})");
                    }
                    else if (e is JsonReaderException)
                    {
                        Trace.WriteLine($"([NODE] Data processing error: not vaild json ({Ip})");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        internal void StartProcess()
        {
            new System.Threading.Thread(() =>
           {
               try
               {
                   Process();
               }
               catch (SocketException)
               {
                   Trace.WriteLine($"[NODE] Socket exception. ({Ip})");
               }
           }).Start();
        }

        private void HandleReceivedData(JObject json)
        {
            var type = json.Properties().Select(p => p.Name).FirstOrDefault();
            var content = json.Properties().Select(p => p.Value).FirstOrDefault();

            if (type == "handshake")
            {
                Handlers.OnReceivedHandshake(content.ToObject<Handshake>());
            }

            if (type == "key")
            {
                Handlers.OnReceivedKey(content.ToObject<Key>());
            }

            if (type == "heartbeat")
            {
                Handlers.OnReceivedHeartbeat();
            }

            if (type == "message")
            {
                Handlers.OnReceivedMessage(content.ToString(), MessageTarget.Broadcast);
            }

            if (type == "private")
            {
                Handlers.OnReceivedMessage(content.ToString(), MessageTarget.Private);
            }

            if (type == "nickname")
            {
                Handlers.OnChangedNickname(content.ToString());
            }

            if (type == "list")
            {
                Handlers.OnReceivedList(content.ToObject<List<ListItem>>(), IPAddress.Parse(((IPEndPoint)Socket.LocalEndPoint).Address.ToString()));
            }
        }

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Destructor.
        /// </summary>
        ~NodeInstance()
        {
            Dispose(false);
        }

        /// <summary>
        /// Node dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Node dispose.
        /// </summary>
        /// <param name="disposing"> Free any other managed objects</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (HeartbeatReceiveTimer != null)
                    {
                        HeartbeatReceiveTimer.Dispose();
                    }
                    if (HeartbeatSendTimer != null)
                    {
                        HeartbeatSendTimer.Dispose();
                    }
                    if (Client != null)
                    {
                        Client.Dispose();
                    }
                    if (Socket != null)
                    {
                        Socket.Close();
                    }
                }
                disposedValue = true;
            }
        }

        #endregion IDisposable Support
    }
}