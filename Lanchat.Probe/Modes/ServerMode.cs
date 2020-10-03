﻿using System;
using System.Net;
using Lanchat.Core;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe.Modes
{
    public class ServerMode
    {
        public ServerMode(int port)
        {
            Config.Nickname = "Server";
            var server = new Server(IPAddress.Any, port);
            _ = new ServerEventsHandlers(server);

            server.Start();
            Console.WriteLine($"Server started on port {server.Endpoint.Port}");

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                server.BroadcastMessage(input);
            }

            Console.WriteLine("Stopping");
            server.Stop();
        }
    }
}