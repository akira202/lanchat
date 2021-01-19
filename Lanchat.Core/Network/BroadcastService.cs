using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;

// ReSharper disable FunctionNeverReturns

namespace Lanchat.Core.Network
{
    internal class BroadcastService
    {
        private readonly IPEndPoint endPoint;
        private readonly UdpClient udpClient;
        private readonly string uniqueId;

        internal EventHandler<Broadcast> BroadcastReceived;

        internal BroadcastService()
        {
            uniqueId = Guid.NewGuid().ToString();
            endPoint = new IPEndPoint(IPAddress.Broadcast, CoreConfig.BroadcastPort);
            udpClient = new UdpClient();
        }

        internal void Start()
        {
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, CoreConfig.BroadcastPort));
            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    try
                    {
                        var broadcast = JsonSerializer.Deserialize<Broadcast>(Encoding.UTF8.GetString(recvBuffer));
                        if (broadcast != null && broadcast.Guid != uniqueId)
                        {
                            broadcast.IpAddress = from.Address;
                            broadcast.Nickname = broadcast.Nickname.Truncate(CoreConfig.MaxNicknameLenght);
                            BroadcastReceived?.Invoke(this, broadcast);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is not JsonException) throw;
                    }
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    var json = JsonSerializer.Serialize(new Broadcast
                    {
                        Guid = uniqueId,
                        Nickname = CoreConfig.Nickname
                    });

                    var data = Encoding.UTF8.GetBytes(json);
                    udpClient.Send(data, data.Length, endPoint);
                    Thread.Sleep(2000);
                }
            });
        }
    }
}