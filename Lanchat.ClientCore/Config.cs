using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Lanchat.Core;
using Lanchat.Core.Models;

namespace Lanchat.ClientCore
{
    public class Config : IConfig
    {
        private static int _port = 3645;
        private static int _broadcastPort = 3646;
        private static string _nickname = ConfigValues.GetNickname();
        private static bool _automatic = true;
        private static bool _useIPv6;
        private static string _language = "default";
        private static string _filesDownloadDirectory = ConfigValues.GetDownloadsDirectory();
        private static Status _status = Status.Online;
        private static ObservableCollection<IPAddress> _blockedAddresses = new();
        private static ObservableCollection<IPAddress> _savedAddresses = new();

        [JsonIgnore] public bool Fresh { get; set; }
        [JsonIgnore] public bool ConnectToSaved { get; set; } = true;
        [JsonIgnore] public bool NodesDetection { get; set; } = true;
        [JsonIgnore] public bool StartServer { get; set; } = true;
        
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }
        
        public ObservableCollection<IPAddress> BlockedAddresses
        {
            get => _blockedAddresses;
            set
            {
                _blockedAddresses = value;
                OnPropertyChanged(nameof(BlockedAddresses));
            }
        }

        public ObservableCollection<IPAddress> SavedAddresses
        {
            get => _savedAddresses;
            set
            {
                _savedAddresses = value;
                OnPropertyChanged(nameof(SavedAddresses));
            }
        }

        public Status Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public int ServerPort
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(ServerPort));
            }
        }

        public int BroadcastPort
        {
            get => _broadcastPort;
            set
            {
                _broadcastPort = value;
                OnPropertyChanged(nameof(BroadcastPort));
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        public bool ConnectToReceivedList
        {
            get => _automatic;
            set
            {
                _automatic = value;
                OnPropertyChanged(nameof(ConnectToReceivedList));
            }
        }

        public string ReceivedFilesDirectory
        {
            get => _filesDownloadDirectory;
            set
            {
                _filesDownloadDirectory = value;
                OnPropertyChanged(nameof(ReceivedFilesDirectory));
            }
        }

        public bool UseIPv6
        {
            get => _useIPv6;
            set
            {
                _useIPv6 = value;
                OnPropertyChanged(nameof(UseIPv6));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}