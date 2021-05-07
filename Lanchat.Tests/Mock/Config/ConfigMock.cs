using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using Lanchat.Core.Config;
using Lanchat.Core.Models;

namespace Lanchat.Tests.Mock.Config
{
    public class ConfigMock : IConfig
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Nickname { get; set; }
        public int ServerPort { get; set; }
        public int BroadcastPort { get; set; }
        public ObservableCollection<IPAddress> BlockedAddresses { get; set; }
        public ObservableCollection<IPAddress> SavedAddresses { get; set; }
        public Status Status { get; set; }
        public bool UseIPv6 { get; set; }
        public bool ConnectToReceivedList { get; set; }
        public bool ConnectToSaved { get; set; }
        public bool NodesDetection { get; set; }
        public bool StartServer { get; set; }
        public string ReceivedFilesDirectory { get; set; }
    }
}