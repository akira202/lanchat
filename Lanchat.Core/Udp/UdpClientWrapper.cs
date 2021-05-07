using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lanchat.Core.Udp
{
    internal class UdpClientWrapper : IUdpClient
    {
        private readonly UdpClient udpClient;

        public UdpClientWrapper()
        {
            udpClient = new UdpClient();
        }

        public void Send(string data, IPEndPoint endPoint)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            udpClient.Send(dataBytes, dataBytes.Length, endPoint);
        }

        public string Receive(ref IPEndPoint endPoint)
        {
            var recvBuffer = udpClient.Receive(ref endPoint);
            return Encoding.UTF8.GetString(recvBuffer);
        }

        public void Bind(IPEndPoint endPoint)
        {
            udpClient.Client.Bind(endPoint);
        }
    }
}