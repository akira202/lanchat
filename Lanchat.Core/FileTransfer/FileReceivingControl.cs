using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FileReceivingControl
    {
        private readonly IOutput output;

        internal FileReceivingControl(IOutput output)
        {
            this.output = output;
        }

        internal void Accept()
        {
            output.SendData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Accepted
            });
        }

        internal void Reject()
        {
            output.SendData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Rejected
            });
        }

        internal void Cancel()
        {
            output.SendData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Canceled
            });
        }
    }
}