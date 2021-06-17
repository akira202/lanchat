using System.Collections.Generic;
using Lanchat.Core.Network;

namespace Lanchat.Core.Channels
{
    /// <summary>
    ///     Send data to specified nodes.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        ///     Broadcast message.
        /// </summary>
        /// <param name="message">Message content.</param>
        void SendMessage(string message);

        /// <summary>
        ///     Broadcast data.
        /// </summary>
        /// <param name="data"></param>
        void SendData(object data);
        
        /// <summary>
        ///     Nodes in channel.
        /// </summary>
        public List<INode> Nodes { get; }
    }
}