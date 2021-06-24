using System.Collections.Generic;
using Lanchat.Core.Channels.Models;

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
            Broadcast = new Channel("main");
            Channels.Add(Broadcast);
        }

        /// <inheritdoc />
        public Channel CreateChannel(string name)
        {
            var channel = new Channel(name);
            Channels.Add(channel);
            Broadcast.SendData(new ChannelOpened
            {
                Name = channel.Name,
                Id = channel.Id.ToString()
            });
            
            return channel;
        }
    }
}