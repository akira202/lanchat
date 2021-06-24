using System;
using System.Collections.Generic;
using Lanchat.Core.Network;

namespace Lanchat.Core.Channels
{
    /// <inheritdoc />
    public class Channel : IChannel
    {
        /// <inheritdoc />
        public Guid Id { get; }

        /// <inheritdoc />
        public List<INode> Nodes { get; } = new();
        
        internal Channel(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
        
        internal Channel(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        /// <inheritdoc />
        public string Name { get; }

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