using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    // TODO: Refactor
    internal class FilePartHandler : ApiHandler<FilePart>
    {
        private readonly FileReceiver fileReceiver;

        public FilePartHandler(FileReceiver fileReceiver)
        {
            this.fileReceiver = fileReceiver;
        }

        protected override void Handle(FilePart filePart)
        {
            if (fileReceiver.Request is not {Accepted: true}) return;

            try
            {
                var data = fileReceiver.Encryption.DecryptBytes(filePart.Data);
                fileReceiver.WriteFileStream.Write(data, 0, data.Length);
                fileReceiver.Request.PartsTransferred++;
                if (!filePart.Last) return;
                fileReceiver.OnFileTransferFinished(fileReceiver.Request);
                fileReceiver.WriteFileStream.Dispose();
                fileReceiver.Request = null;
            }
            catch
            {
                fileReceiver.CancelReceive();
                fileReceiver.OnFileTransferError();
            }
        }
    }
}