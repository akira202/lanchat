using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    /// <summary>
    ///     Connected user.
    /// </summary>
    public interface INode : INotifyPropertyChanged
    {
        /// <summary>
        ///     Node user nickname.
        /// </summary>
        string Nickname { get; }

        /// <summary>
        ///     Nickname before last change.
        /// </summary>
        string PreviousNickname { get; }

        /// <summary>
        ///     ID of TCP client or session.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     Short ID.
        /// </summary>
        string ShortId { get; }

        /// <summary>
        ///     Node ready. If set to false node won't send or receive messages.
        /// </summary>
        bool Ready { get; }

        /// <summary>
        ///     Node user status.
        /// </summary>
        Status Status { get; }

        /// <see cref="IHost" />
        IHost Host { get; }

        /// <see cref="Lanchat.Core.Chat.Messaging" />
        Messaging Messaging { get; }

        /// <see cref="Lanchat.Core.FileTransfer.FileReceiver" />
        FileReceiver FileReceiver { get; }

        /// <see cref="Lanchat.Core.FileTransfer.FileSender" />
        FileSender FileSender { get; }

        /// <see cref="Lanchat.Core.Api.Output" />
        IOutput Output { get; }

        /// <see cref="Lanchat.Core.Api.Resolver" />
        IResolver Resolver { get; }

        /// <summary>
        ///     Node successful connected and ready to data exchange.
        /// </summary>
        event EventHandler Connected;

        /// <summary>
        ///     Node disconnected. Cannot reconnect.
        /// </summary>
        event EventHandler Disconnected;

        /// <summary>
        ///     TCP session or client returned error.
        /// </summary>
        event EventHandler<SocketError> SocketErrored;

        /// <summary>
        ///     Disconnect from node.
        /// </summary>
        void Disconnect();
    }
}