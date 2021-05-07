using System;
using System.Collections.ObjectModel;
using Lanchat.Core.Config;
using Lanchat.Core.Models;
using Lanchat.Core.Udp;

namespace Lanchat.Core.NodesDetection
{
    /// <summary>
    ///     Detecting nodes by UDP broadcasts.
    /// </summary>
    public class NodesDetector
    {
        private readonly AnnounceListener announceListener;
        private readonly AnnounceSender announceSender;

        internal NodesDetector(IConfig config)
        {
            var uniqueId = Guid.NewGuid().ToString();
            var udpClient = new UdpClientWrapper();

            announceListener = new AnnounceListener(config, udpClient, uniqueId, DetectedNodes);
            announceSender = new AnnounceSender(config, udpClient, uniqueId);
        }

        /// <summary>
        ///     Detected nodes.
        /// </summary>
        public ObservableCollection<Announce> DetectedNodes { get; } = new();

        internal void Start()
        {
            announceListener.Start();
            announceSender.Start();
        }
    }
}