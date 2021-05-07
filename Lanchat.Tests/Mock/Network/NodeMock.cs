using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.Tcp;
using Lanchat.Tests.Mock.Encryption;
using Lanchat.Tests.Mock.FileTransfer;
using Lanchat.Tests.Mock.Tcp;

namespace Lanchat.Tests.Mock.Network
{
    internal class NodeMock : INodeInternal, INode
    {
        public NodeMock(IOutput outputMock = null)
        {
            Host = new HostMock();
            Output = outputMock;
            ShortId = "9999";
            Nickname = "test";
            PreviousNickname = "test";
            ModelEncryption = new ModelEncryptionMock();
            Resolver = new Resolver(this);
            Messaging = new Messaging(Output);
            FileSender = new FileSender(Output);
            FileReceiver = new FileReceiver(Output, new FileSystemMock());
        }

        public bool HandshakeSent { get; private set; }
        public bool ConnectedEvent { get; private set; }
        public bool CannotConnectEvent { get; private set; }

        public string ShortId { get; }
        public Messaging Messaging { get; }
        public FileReceiver FileReceiver { get; }
        public FileSender FileSender { get; }
        public IOutput Output { get; }
        public IResolver Resolver { get; }
        public string PreviousNickname { get; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool Ready { get; set; }
        public bool IsSession => false;
        public IHost Host { get; }
        public string Nickname { get; set; }
        public Status Status { get; set; } = Status.Online;
        public Guid Id { get; } = Guid.NewGuid();
        public IModelEncryption ModelEncryption { get; }

        public void SendHandshake()
        {
            HandshakeSent = true;
        }

        public void OnConnected()
        {
            ConnectedEvent = true;
        }

        public void OnDisconnected()
        {
            throw new NotImplementedException();
        }

        public void OnCannotConnect()
        {
            CannotConnectEvent = true;
        }
    }
}