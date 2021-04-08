using System;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;

namespace Lanchat.Tests.Mock
{
    public class NetworkMock : INetworkElement
    {
        public IPEndPoint Endpoint { get; } = new(IPAddress.Loopback, 1234);
        public Guid Id { get; } = Guid.NewGuid();

        public void Send(string text)
        {
            DataReceived?.Invoke(this, text);
        }

        public void Close()
        {
        }

        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event EventHandler<string> DataReceived;
    }
}