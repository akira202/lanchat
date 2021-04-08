using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Network;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.API
{
    /// <summary>
    ///     Send data other of type not belonging to standard Lanchat.Core set.
    /// </summary>
    public class NetworkOutput
    {
        private readonly INetworkElement networkElement;
        private readonly INodeState nodeState;
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        internal NetworkOutput(INetworkElement networkElement, INodeState nodeState)
        {
            this.networkElement = networkElement;
            this.nodeState = nodeState;
        }

        /// <summary>
        ///     Send data.
        /// </summary>
        /// <param name="content">Object to send.</param>
        public void SendData(object content)
        {
            if (!nodeState.Ready) return;
            networkElement.Send(Serialize(content));
        }
        
        /// <summary>
        ///     Send the data before marking the node as ready (Handshake, KeyInfo...).
        /// </summary>
        /// <param name="content">Object to send.</param>
        public void SendPrivilegedData(object content)
        {
            networkElement.Send(Serialize(content));
        }

        internal static string Serialize(object content)
        {
            var data = new Dictionary<string, object> {{content.GetType().Name, content}};
            return JsonSerializer.Serialize(data, SerializerOptions);
        }
    }
}