using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core;
using Lanchat.Core.Models;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public class NodeEventsHandlers
    {
        private readonly Node node;

        public NodeEventsHandlers(Node node)
        {
            this.node = node;
            node.NetworkInput.MessageReceived += OnMessageReceived;
            node.NetworkInput.PrivateMessageReceived += OnPrivateMessageReceived;
            node.NetworkInput.PongReceived += OnPongReceived;
            node.Connected += OnConnected;
            node.Disconnected += OnDisconnected;
            node.HardDisconnect += OnHardDisconnected;
            node.SocketErrored += OnSocketErrored;
            node.CannotConnect += OnCannotConnect;
            node.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Status":
                    var status = node.Status switch
                    {
                        Status.Online => "online",
                        Status.AwayFromKeyboard => "afk",
                        Status.DoNotDisturb => "dnd",
                        _ => ""
                    };
                    Ui.Log.Add($"{node.Nickname} changed status to {status}");
                    break;

                case "Nickname":
                    if (!node.Ready) return;
                    Ui.Log.Add($"{node.PreviousNickname} is now {node.Nickname}");
                    break;
            }
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Ui.Log.Add($"{node.Nickname} {Resources.Info_Connected}");
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Ui.Log.Add($"{node.Nickname} {Resources.Info_Reconnecting}");
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnHardDisconnected(object sender, EventArgs e)
        {
            Ui.Log.Add($"{node.Nickname} {Resources.Info_Disconnected}");
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnMessageReceived(object sender, string e)
        {
            Ui.Log.AddMessage(e, node.Nickname);
        }

        private void OnPrivateMessageReceived(object sender, string e)
        {
            Ui.Log.AddPrivateMessage(e, node.Nickname);
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            Ui.Log.Add($"{Resources.Info_ConnectionError}: {node.Nickname} / {e}");
        }

        private void OnCannotConnect(object sender, EventArgs e)
        {
            Ui.Log.Add($"{Resources.Info_CannotConnect}: {node.Endpoint}");
        }

        private void OnPongReceived(object sender, TimeSpan? e)
        {
            if (e != null)
                Ui.Log.Add($"Ping to {node.Nickname} is {e.Value.Milliseconds}ms");
        }
    }
}