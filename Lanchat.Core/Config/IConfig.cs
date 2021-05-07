// ReSharper disable UnusedMemberInSuper.Global

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using Lanchat.Core.Models;

namespace Lanchat.Core.Config
{
    /// <summary>
    ///     Lanchat.Core configuration.
    /// </summary>
    public interface IConfig : INotifyPropertyChanged
    {
        /// <summary>
        ///     User nickname.
        /// </summary>
        string Nickname { get; set; }

        /// <summary>
        ///     Server port.
        /// </summary>
        int ServerPort { get; set; }

        /// <summary>
        ///     Broadcast port.
        /// </summary>
        int BroadcastPort { get; set; }

        /// <summary>
        ///     Blocked IP addresses.
        /// </summary>
        ObservableCollection<IPAddress> BlockedAddresses { get; set; }

        /// <summary>
        ///     Addresses of previously connected nodes.
        /// </summary>
        ObservableCollection<IPAddress> SavedAddresses { get; set; }

        /// <summary>
        ///     User status.
        /// </summary>
        Status Status { get; set; }

        /// <summary>
        ///     Use IPv6 instead IPv4.
        /// </summary>
        bool UseIPv6 { get; set; }

        /// <summary>
        ///     Try connecting with nodes from received list.
        /// </summary>
        bool ConnectToReceivedList { get; set; }

        /// <summary>
        ///     Try connecting with nodes from SavedAddresses
        /// </summary>
        bool ConnectToSaved { get; set; }

        /// <summary>
        ///     Use UDP broadcasting to announce self presence and detect other nodes.
        /// </summary>
        bool NodesDetection { get; set; }

        /// <summary>
        ///     Disable only in debug mode.
        /// </summary>
        bool StartServer { get; set; }

        /// <summary>
        ///     Files download directory.
        /// </summary>
        string ReceivedFilesDirectory { get; set; }
    }
}