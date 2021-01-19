using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Models;

namespace Lanchat.Core
{
    /// <summary>
    ///     Lanchat configuration.
    /// </summary>
    public static class CoreConfig
    {
        private static string _nickname = "user";
        private static Status _status = Status.Online;

        /// <summary>
        ///     User nickname.
        /// </summary>
        public static string Nickname
        {
            get => _nickname;
            set
            {
                if (_nickname == value) return;
                _nickname = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     User status.
        /// </summary>
        public static Status Status
        {
            get => _status;
            set
            {
                if (_status == value) return;
                _status = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Server port.
        /// </summary>
        public static int ServerPort { get; set; } = 3645;

        /// <summary>
        ///     Broadcast port.
        /// </summary>
        public static int BroadcastPort { get; set; } = 3646;

        /// <summary>
        ///     Max message lenght. Longer incoming messages will be trimmed.
        /// </summary>
        public static int MaxMessageLenght { get; set; } = 2000;

        /// <summary>
        ///     Max nickname lenght. Longer nicknames will be trimmed.
        /// </summary>
        public static int MaxNicknameLenght { get; set; } = 20;

        /// <summary>
        ///     Enable automatic connecting to nodes from received list.
        /// </summary>
        public static bool AutomaticConnecting { get; set; } = true;

        /// <summary>
        ///     Blocked IP addresses.
        /// </summary>
        public static List<IPAddress> BlockedAddresses { get; set; } = new();

        // Internal configurations
        internal static JsonSerializerOptions JsonSerializerOptions =>
            new()
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

        /// <summary>
        ///     Invoked for properties like nickname or status.
        /// </summary>
        public static event PropertyChangedEventHandler PropertyChanged;

        private static void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}