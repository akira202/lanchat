using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core;
using Lanchat.Gtk.Views;

namespace Lanchat.Gtk
{
    public class NodeEventsHandlers
    {
        private readonly MainWindow mainWindow;
        private readonly Node node;

        public NodeEventsHandlers(Node node, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.node = node;
            node.NetworkInput.MessageReceived += OnMessageReceived;
            node.NetworkInput.PrivateMessageReceived += OnPrivateMessageReceived;
            node.Connected += OnConnected;
            node.Disconnected += OnDisconnected;
            node.HardDisconnect += OnHardDisconnected;
            node.SocketErrored += OnSocketErrored;
            node.PropertyChanged += OnPropertyChanged;
            node.CannotConnect += OnCannotConnect;
        }

        private void OnConnected(object sender, EventArgs e)
        {
            mainWindow.SideBarWidget.AddConnected(node.Nickname, node.Id);
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            mainWindow.SideBarWidget.RemoveConnected(node.Id);
        }

        private void OnHardDisconnected(object sender, EventArgs e)
        {
            mainWindow.SideBarWidget.RemoveConnected(node.Id);
        }

        private void OnMessageReceived(object sender, string e)
        {
            mainWindow.ChatWidget.AddChatEntry(node.Nickname, e);
        }

        private void OnPrivateMessageReceived(object sender, string e)
        {
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Nickname") throw new NotImplementedException();
        }

        private void OnCannotConnect(object sender, EventArgs e)
        {
        }
    }
}