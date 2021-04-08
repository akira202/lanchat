using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Core.Network
{
    /// <summary>
    ///     Common TCP client and session stuff.
    /// </summary>
    public interface INetworkElement
    {
        /// <summary>
        ///     IP endpoint (address + port).
        /// </summary>
        IPEndPoint Endpoint { get; }

        /// <summary>
        ///     Session or client ID.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     Send data.
        /// </summary>
        /// <param name="text">Content.</param>
        void Send(string text);

        /// <summary>
        ///     Close client or session.
        /// </summary>
        void Close();

        /// <summary>
        ///     Network element disconnected.
        /// </summary>
        event EventHandler Disconnected;

        /// <summary>
        ///     Network element socket errored.
        /// </summary>
        event EventHandler<SocketError> SocketErrored;

        /// <summary>
        ///     Network element received data.
        /// </summary>
        event EventHandler<string> DataReceived;
    }
}