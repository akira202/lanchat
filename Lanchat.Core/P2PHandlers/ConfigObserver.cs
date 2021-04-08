using System.ComponentModel;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2PHandlers
{
    internal class ConfigObserver
    {
        private readonly P2P network;

        public ConfigObserver(P2P network)
        {
            this.network = network;
            network.Config.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nickname":
                    network.Broadcasting.SendData(new NicknameUpdate {NewNickname = network.Config.Nickname});
                    break;

                case "Status":
                    network.Broadcasting.SendData(new StatusUpdate {NewStatus = network.Config.Status});
                    break;
            }
        }
    }
}