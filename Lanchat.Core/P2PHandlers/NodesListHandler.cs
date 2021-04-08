using System;
using System.Collections.Generic;
using System.Net;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2PHandlers
{
    internal class NodesListHandler : ApiHandler<NodesList>
    {
        private readonly P2P network;

        internal NodesListHandler(P2P network)
        {
            this.network = network;
        }

        protected override void Handle(NodesList stringList)
        {
            var list = new List<IPAddress>();

            // Convert strings to ip addresses.
            stringList?.ForEach(x =>
            {
                if (IPAddress.TryParse(x, out var ipAddress)) list.Add(ipAddress);
            });

            if (!network.Config.ConnectToReceivedList) return;
            list.ForEach(x =>
            {
                try
                {
                    network.Connect(x).ConfigureAwait(false);
                }
                catch (ArgumentException)
                {
                }
            });
        }
    }
}