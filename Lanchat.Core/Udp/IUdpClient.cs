using System.Net;

namespace Lanchat.Core.Udp
{
    internal interface IUdpClient
    {
        void Send(string data, IPEndPoint endPoint);
        void Bind(IPEndPoint endPoint);
        string Receive(ref IPEndPoint endPoint);
    }
}