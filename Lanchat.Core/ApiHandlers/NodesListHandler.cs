using System;
using System.Net;
using Lanchat.Core.Api;
using Lanchat.Core.Config;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.ApiHandlers
{
    internal class NodesListHandler : ApiHandler<NodesList>
    {
        private readonly IConfig config;
        private readonly IP2P network;

        internal NodesListHandler(IConfig config, IP2P network)
        {
            this.config = config;
            this.network = network;
        }

        protected override void Handle(NodesList nodesList)
        {
            if (!config.ConnectToReceivedList)
            {
                return;
            }

            nodesList.RemoveAll(x => x.Equals(IPAddress.Loopback));
            nodesList.ForEach(x =>
            {
                try
                {
                    network.Connect(x).ConfigureAwait(false);
                }
                catch (ArgumentException)
                { }
            });
        }
    }
}