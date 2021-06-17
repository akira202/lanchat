using System.Collections.Generic;
using Lanchat.Core.Network;

namespace Lanchat.Core.Channels
{
    /// <inheritdoc />
    public class Channel : IChannel
    {
        /// <inheritdoc />
        public List<INode> Nodes { get; } = new();

        internal Channel(bool isPrivate, string name)
        {
            Name = name;
            Private = isPrivate;
        }
        
        public string Name { get; set; }
        public bool Private { get; }
        
        /// <inheritdoc />
        public void SendMessage(string message)
        {
            Nodes.ForEach(x => x.Messaging.SendMessage(message));
        }
        
        /// <inheritdoc />
        public void SendData(object data)
        {
            Nodes.ForEach(x => x.Output.SendData(data));
        }
    }
}