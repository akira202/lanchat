using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using Lanchat.Core;
using Lanchat.Core.FileTransfer;
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
            node.Messaging.MessageReceived += OnMessageReceived;
            node.Messaging.PrivateMessageReceived += OnPrivateMessageReceived;

            node.FileReceiver.FileReceiveFinished += OnFileReceiveFinished;
            node.FileReceiver.FileTransferError += OnFileTransferError;
            node.FileReceiver.FileTransferRequestReceived += OnFileTransferHandlerRequestReceived;

            node.FileSender.FileTransferRequestAccepted += OnFileTransferHandlerRequestAccepted;
            node.FileSender.FileTransferRequestRejected += OnFileTransferHandlerRequestRejected;
            node.FileSender.FileSendFinished += OnFileSendFinished;
            node.FileSender.FileTransferError += OnFileTransferError;

            node.FileSender.FileTransferRequestAccepted += Ui.FileTransferMonitor.OnFileTransferRequestAccepted;
            node.FileSender.FileSendFinished += Ui.FileTransferMonitor.OnFileReceiveFinished;
            node.FileSender.FileTransferError += Ui.FileTransferMonitor.OnFileTransferError;
            node.FileReceiver.FileTransferRequestAccepted += Ui.FileTransferMonitor.OnFileTransferRequestAccepted;
            node.FileReceiver.FileReceiveFinished += Ui.FileTransferMonitor.OnFileReceiveFinished;
            node.FileReceiver.FileTransferError += Ui.FileTransferMonitor.OnFileTransferError;

            node.Connected += OnConnected;
            node.Disconnected += OnDisconnected;
            node.SocketErrored += OnSocketErrored;
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
                    Ui.Log.Add(string.Format(Resources._StatusChange, node.Nickname, status));
                    break;

                case "Nickname":
                    if (!node.Ready) return;
                    Ui.Log.Add(string.Format(Resources._NicknameChanged, node.PreviousNickname, node.Nickname));
                    break;
            }
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Ui.Log.Add(string.Format(Resources._Connected, node.Nickname));
            Ui.BottomBar.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Ui.Log.Add(string.Format(Resources._Disconnected, node.Nickname));
            Ui.BottomBar.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnMessageReceived(object sender, string e)
        {
            Ui.Log.AddMessage(e, node.Nickname, false);
        }

        private void OnPrivateMessageReceived(object sender, string e)
        {
            Ui.Log.AddMessage(e, node.Nickname, true);
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            Ui.Log.AddError(string.Format(Resources._ConnectionError, node.Id, e));
        }

        private void OnFileTransferHandlerRequestReceived(object sender, FileTransferRequest e)
        {
            Ui.Log.Add(string.Format(Resources._FileRequest, node.Nickname, e.FileName));
        }

        private void OnFileReceiveFinished(object sender, FileTransferRequest e)
        {
            Ui.Log.Add(string.Format(Resources._FileReceived, node.Nickname, Path.GetFullPath(e.FilePath)));
        }

        private void OnFileSendFinished(object sender, FileTransferRequest e)
        {
            Ui.Log.Add(string.Format(Resources._FileTransferFinished, node.Nickname));
        }

        private void OnFileTransferError(object sender, FileTransferException e)
        {
            Ui.Log.AddError(string.Format(Resources._FileTransferError, node.Nickname));
        }

        private void OnFileTransferHandlerRequestAccepted(object sender, FileTransferRequest e)
        {
            Ui.Log.Add(string.Format(Resources._FileRequestAccepted, node.Nickname));
        }

        private void OnFileTransferHandlerRequestRejected(object sender, FileTransferRequest e)
        {
            Ui.Log.Add(string.Format(Resources._FileRequestRejected, node.Nickname));
        }
    }
}