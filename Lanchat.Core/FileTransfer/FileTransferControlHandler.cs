using System.Diagnostics;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileTransferControlHandler : ApiHandler<FileTransferControl>
    {
        private readonly FileReceiver fileReceiver;
        private readonly FileSender fileSender;

        internal FileTransferControlHandler(FileReceiver fileReceiver, FileSender fileSender)
        {
            this.fileReceiver = fileReceiver;
            this.fileSender = fileSender;
        }

        protected override void Handle(FileTransferControl request)
        {
            switch (request.RequestStatus)
            {
                case RequestStatus.Accepted:
                    fileSender.SendFile();
                    break;

                case RequestStatus.Rejected:
                    fileSender.HandleReject();
                    break;

                case RequestStatus.Canceled:
                    fileSender.HandleCancel();
                    break;

                case RequestStatus.Sending:
                    fileReceiver.HandleReceiveRequest(request);
                    break;

                case RequestStatus.Finished:
                    fileReceiver.FinishReceive();
                    break;

                case RequestStatus.Errored:
                    fileReceiver.HandleSenderError();
                    break;

                default:
                    Trace.Write("Node received file exchange request of unknown type.");
                    break;
            }
        }
    }
}