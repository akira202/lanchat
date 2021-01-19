using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class NetworkInput
    {
        private readonly Node node;
        private readonly JsonSerializerOptions serializerOptions;

        internal NetworkInput(Node node)
        {
            this.node = node;
            serializerOptions = CoreConfig.JsonSerializerOptions;
        }

        /// <summary>
        ///     Message received.
        /// </summary>
        public event EventHandler<string> MessageReceived;

        /// <summary>
        ///     Private message received.
        /// </summary>
        public event EventHandler<string> PrivateMessageReceived;

        /// <summary>
        ///     Ping pong.
        /// </summary>
        public event EventHandler<TimeSpan?> PongReceived;

        internal event EventHandler<Handshake> HandshakeReceived;
        internal event EventHandler<KeyInfo> KeyInfoReceived;
        internal event EventHandler<List<IPAddress>> NodesListReceived;

        internal void ProcessReceivedData(object sender, string dataString)
        {
            foreach (var item in dataString.Replace("}{", "}|{").Split('|'))
                try
                {
                    var json = JsonSerializer.Deserialize<Wrapper>(item, serializerOptions);
                    var content = json.Data?.ToString();

                    // If node isn't ready ignore every messages except handshake and key info.
                    if (!node.Ready && json.Type != DataTypes.Handshake && json.Type != DataTypes.KeyInfo) return;

                    // Ignore handshake and key info is node was set as ready before.
                    if (node.Ready && (json.Type == DataTypes.Handshake || json.Type == DataTypes.KeyInfo)) return;

                    Trace.WriteLine($"Node {node.Id} received {json.Type}");

                    switch (json.Type)
                    {
                        case DataTypes.Message:
                            var decryptedMessage = node.Encryption.Decrypt(content);
                            if (decryptedMessage == null) return;
                            MessageReceived?.Invoke(this, decryptedMessage.Truncate(CoreConfig.MaxMessageLenght));
                            break;

                        case DataTypes.PrivateMessage:
                            var decryptedPrivateMessage = node.Encryption.Decrypt(content);
                            if (decryptedPrivateMessage == null) return;
                            PrivateMessageReceived?.Invoke(this,
                                decryptedPrivateMessage.Truncate(CoreConfig.MaxMessageLenght));
                            break;

                        case DataTypes.Handshake:
                            var handshake = JsonSerializer.Deserialize<Handshake>(content, serializerOptions);
                            HandshakeReceived?.Invoke(this, handshake);
                            break;

                        case DataTypes.KeyInfo:
                            var keyInfo = JsonSerializer.Deserialize<KeyInfo>(content);
                            KeyInfoReceived?.Invoke(this, keyInfo);
                            break;

                        case DataTypes.NodesList:
                            var stringList = JsonSerializer.Deserialize<List<string>>(content);
                            var list = new List<IPAddress>();

                            // Convert strings to ip addresses.
                            stringList.ForEach(x =>
                            {
                                if (IPAddress.TryParse(x, out var ipAddress)) list.Add(ipAddress);
                            });

                            NodesListReceived?.Invoke(this, list);
                            break;

                        case DataTypes.NicknameUpdate:
                            node.Nickname = content.Truncate(CoreConfig.MaxNicknameLenght);
                            break;

                        case DataTypes.Goodbye:
                            node.NetworkElement.EnableReconnecting = false;
                            break;

                        case DataTypes.StatusUpdate:
                            if (Enum.TryParse<Status>(content, out var status)) node.Status = status;
                            break;

                        case DataTypes.Ping:
                            node.NetworkOutput.SendPong();
                            break;

                        case DataTypes.Pong:
                            if (node.PingSendTime == null) return;
                            node.Ping = DateTime.Now - node.PingSendTime;
                            node.PingSendTime = null;
                            PongReceived?.Invoke(this, node.Ping);
                            break;

                        default:
                            Trace.WriteLine($"Node {node.Id} received data of unknown type.");
                            break;
                    }
                }

                // Input errors catching.
                catch (Exception ex)
                {
                    if (ex is not JsonException && ex is not ArgumentNullException) throw;
                }
        }
    }
}