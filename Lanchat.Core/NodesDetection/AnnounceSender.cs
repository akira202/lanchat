using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Lanchat.Core.Config;
using Lanchat.Core.Json;
using Lanchat.Core.Models;
using Lanchat.Core.Udp;

namespace Lanchat.Core.NodesDetection
{
    internal class AnnounceSender
    {
        private readonly IConfig config;
        private readonly IPEndPoint endPoint;
        private readonly JsonUtils jsonUtils;
        private readonly IUdpClient udpClient;
        private readonly string uniqueId;
        private bool continueSendingAnnouncements = true;

        public AnnounceSender(IConfig config, IUdpClient udpClient, string uniqueId)
        {
            this.udpClient = udpClient;
            this.uniqueId = uniqueId;
            this.config = config;
            jsonUtils = new JsonUtils();
            endPoint = new IPEndPoint(IPAddress.Broadcast, config.BroadcastPort);
        }

        internal void Start()
        {
            Task.Run(() =>
            {
                while (continueSendingAnnouncements)
                {
                    var json = jsonUtils.Serialize(new Announce
                    {
                        Guid = uniqueId,
                        Nickname = config.Nickname
                    });

                    udpClient.Send(json, endPoint);
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