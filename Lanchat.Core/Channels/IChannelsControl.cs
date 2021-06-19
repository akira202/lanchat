using System.Collections.Generic;

namespace Lanchat.Core.Channels
{
    /// <summary>
    ///     Nodes groups management.
    /// </summary>
    public interface IChannelsControl
    {
        /// <summary>
        ///     Main channel with all connected nodes.
        /// </summary>
        IChannel Broadcast { get; }

        /// <summary>
        ///     Channels list.
        /// </summary>
        List<IChannel> Channels { get; }
    }
}