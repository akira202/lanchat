using System;
using ConsoleGUI.Controls;
using Lanchat.Core.FileTransfer;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface
{
    public class FileTransferMonitor : TextBlock
    {
        private long parts;
        private long totalProgress;

        public FileTransferMonitor()
        {
            Color = ConsoleColor.Gray;
            Text = string.Format(Resources._NoFileReceiveRequest);
        }

        public void OnAcceptedByReceiver(object sender, FileTransferRequest e)
        {
            parts += e.Parts;
            e.PropertyChanged += (_, _) =>
            {
                totalProgress++;
                Text = $"{totalProgress}/{parts}";
            };
        }

        public void OnFileReceiveFinished(object sender, FileTransferRequest e)
        {
            ResetCounter(e);
        }

        public void OnFileTransferError(object sender, FileTransferException e)
        {
            ResetCounter(e.Request);
        }

        private void ResetCounter(FileTransferRequest e)
        {
            totalProgress -= e.Parts;
            parts -= e.Parts;
            Text = $"{totalProgress}/{parts}";
            if (parts == 0)
            {
                Text = string.Format(Resources._NoFileReceiveRequest);
            }
        }
    }
}