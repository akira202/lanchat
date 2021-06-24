using System;
using Lanchat.Core.Api;
using Lanchat.Core.Channels.Models;

namespace Lanchat.Core.Channels.Handlers
{
    internal class ChannelOpenedHandler : ApiHandler<ChannelOpened>
    {
        private readonly IChannelsControl channelsControl;

        public ChannelOpenedHandler(IChannelsControl channelsControl)
        {
            this.channelsControl = channelsControl;
        }
        
        protected override void Handle(ChannelOpened data)
        {
            var remoteChannel = new Channel(data.Name, Guid.Parse(data.Id));
            channelsControl.Channels.Add(remoteChannel);
        }
    }
}