using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodesDetection
{
    internal class AnnounceSender
    {
        private readonly IConfig config;
        private readonly IPEndPoint endPoint;
        private readonly UdpClient udpClient;
        private readonly string uniqueId;
        private bool continueSendingAnnouncements = true;

        public AnnounceSender(IConfig config, UdpClient udpClient, string uniqueId)
        {
            this.udpClient = udpClient;
            this.uniqueId = uniqueId;
            this.config = config;
            endPoint = new IPEndPoint(IPAddress.Broadcast, config.BroadcastPort);
        }

        internal void Start()
        {
            Task.Run(() =>
            {
                while (continueSendingAnnouncements)
                {
                    var json = NetworkOutput.Serialize(new Announce
                    {
                        Guid = uniqueId,
                        Nickname = config.Nickname
                    });

                    var data = Encoding.UTF8.GetBytes(json);
                    udpClient.Send(data, data.Length, endPoint);
                    Thread.Sleep(2000);
                }
            });
        }

        internal void Stop()
        {
            continueSendingAnnouncements = false;
        }
    }
}