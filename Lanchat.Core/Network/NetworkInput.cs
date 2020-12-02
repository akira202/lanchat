using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
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
            Id = node.Id;
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

        internal Guid Id { get; }

        internal event EventHandler<Handshake> HandshakeReceived;
        internal event EventHandler<KeyInfo> KeyInfoReceived;
        internal event EventHandler<List<IPAddress>> NodesListReceived;
        internal event EventHandler<string> NicknameChanged;

        internal void ProcessReceivedData(object sender, string dataString)
        {
            foreach (var item in dataString.Replace("}{", "}|{").Split('|'))
            {
                try
                {
                    var json = JsonSerializer.Deserialize<Wrapper>(item, serializerOptions);
                    var content = json.Data?.ToString();

                    // If node isn't ready ignore every messages except handshake and key info.
                    if (!node.Ready && json.Type != DataTypes.Handshake && json.Type != DataTypes.KeyInfo)
                    {
                        return;
                    }

                    // Ignore handshake and key info is node was set as ready before.
                    if (node.Ready && (json.Type == DataTypes.Handshake || json.Type == DataTypes.KeyInfo))
                    {
                        return;
                    }

                    switch (json.Type)
                    {
                        case DataTypes.Message:
                            MessageReceived?.Invoke(this,
                                TruncateAndValidate(node.Encryption.Decrypt(content), CoreConfig.MaxMessageLenght));
                            break;

                        case DataTypes.PrivateMessage:
                            PrivateMessageReceived?.Invoke(this,
                                TruncateAndValidate(node.Encryption.Decrypt(content), CoreConfig.MaxMessageLenght));
                            break;

                        case DataTypes.Handshake:
                            Trace.WriteLine($"Node {node.Id} received handshake");
                            var handshake = JsonSerializer.Deserialize<Handshake>(content);
                            handshake.Nickname = TruncateAndValidate(handshake.Nickname, CoreConfig.MaxNicknameLenght);
                            HandshakeReceived?.Invoke(this, handshake);
                            break;

                        case DataTypes.KeyInfo:
                            Trace.WriteLine($"Node {node.Id} received key info");
                            var keyInfo = JsonSerializer.Deserialize<KeyInfo>(content);
                            KeyInfoReceived?.Invoke(this, keyInfo);
                            break;

                        case DataTypes.NodesList:
                            Trace.WriteLine($"Node {node.Id} received nodes list");
                            var stringList = JsonSerializer.Deserialize<List<string>>(content);
                            var list = new List<IPAddress>();

                            // Convert strings to ip addresses.
                            stringList.ForEach(x =>
                            {
                                if (IPAddress.TryParse(x, out var ipAddress))
                                {
                                    list.Add(ipAddress);
                                }
                            });

                            NodesListReceived?.Invoke(this, list);
                            break;

                        case DataTypes.NicknameUpdate:
                            Trace.WriteLine($"Node {node.Id} received nickname update");
                            NicknameChanged?.Invoke(this, TruncateAndValidate(content, CoreConfig.MaxNicknameLenght));
                            break;

                        case DataTypes.Goodbye:
                            Trace.WriteLine($"Node {node.Id} received goodbye");
                            node.NetworkElement.EnableReconnecting = false;
                            break;

                        default:
                            Trace.WriteLine($"Node {node.Id} received unknown data");
                            break;
                    }
                }

                // Input errors catching.
                catch (Exception ex)
                {
                    if (ex is JsonException || ex is ArgumentNullException)
                    {
                        Trace.WriteLine("Invalid data received");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private static string TruncateAndValidate(string value, int maxLength)
        {
            value = value.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException();
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }
    }
}