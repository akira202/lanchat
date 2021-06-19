using System.Collections.Generic;

namespace Lanchat.Core.Channels
{
    /// <inheritdoc />
    public class ChannelsControl : IChannelsControl
    {
        /// <inheritdoc />
        public IChannel Broadcast { get; }
        
        /// <inheritdoc />
        public List<IChannel> Channels { get; } = new();

        internal ChannelsControl()
        {
            Broadcast = new Channel(false, "main");
            Channels.Add(Broadcast);
        }
    }
}