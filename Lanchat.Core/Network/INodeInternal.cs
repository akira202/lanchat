using System;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    internal interface INodeInternal
    {
        string Nickname { set; }

        Status Status { set; }

        Guid Id { get; }

        bool Ready { get; set; }

        bool IsSession { get; }

        IHost Host { get; }

        IModelEncryption ModelEncryption { get; }

        void SendHandshake();

        void OnConnected();

        void OnDisconnected();

        void OnCannotConnect();
    }
}