﻿using System;
using System.Linq;
using System.Net;
using Lanchat.Core;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe
{
    public class P2PMode
    {
        public P2PMode()
        {
            var p2p = new P2P(3645);
            _ = new ServerEventsHandlers(p2p.Server);
            p2p.Server.Start();

            while (true)
            {
                Console.Write("IP Address or message: ");
                var input = Console.ReadLine();

                if (IPAddress.TryParse(input!, out _))
                {
                    var client = p2p.Connect(input);
                    _ = new ClientEventsHandlers(client);
                }
                else
                {
                    p2p.Server.Multicast(input);
                    foreach (var node in p2p.Nodes.Where(node => node.Client != null))
                    {
                        node.Client.SendAsync(input);
                    }
                }

                Console.WriteLine(p2p.Nodes.Count);
            }
        }
    }
}