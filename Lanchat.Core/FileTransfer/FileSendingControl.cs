using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileSendingControl
    {
        private readonly IOutput output;

        internal FileSendingControl(IOutput output)
        {
            this.output = output;
        }

        internal void Request(FileTransferRequest request)
        {
            output.SendData(new FileTransferControl
            {
                FileName = request.FileName,
                Parts = request.Parts,
                RequestStatus = RequestStatus.Sending
            });
        }

        internal void Finished()
        {
            output.SendData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Finished
            });
        }

        internal void Errored()
        {
            output.SendData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Errored
            });
        }
    }
}