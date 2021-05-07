using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace Lanchat.Core.Tcp
{
    internal class Session : TcpSession, IHost
    {
        internal Session(TcpServer server) : base(server)
        { }

        public IPEndPoint Endpoint => (IPEndPoint) Socket.RemoteEndPoint;
        public event EventHandler Disconnected;
        public event EventHandler<string> DataReceived;
        public event EventHandler<SocketError> SocketErrored;

        public bool IsSession => true;

        public new void Send(string text)
        {
            base.Send(text);
        }

        public void Close()
        {
            Disconnect();
            Dispose();
        }

        public event EventHandler Connected;

        protected override void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
            Trace.WriteLine($"Session {Id} connected");
        }

        protected override void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
            Trace.WriteLine($"Session {Id} disconnected");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var json = Encoding.UTF8.GetString(buffer, (int) offset, (int) size);
            DataReceived?.Invoke(this, json);
        }

        protected override void OnError(SocketError error)
        {
            SocketErrored?.Invoke(this, error);
            Trace.WriteLine($"Session {Id} errored: {error}");
        }
    }
}