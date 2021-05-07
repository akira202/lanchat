using System;
using System.Diagnostics;
using System.IO;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    internal class FilePartHandler : ApiHandler<FilePart>
    {
        private readonly FileReceiver fileReceiver;

        internal FilePartHandler(FileReceiver fileReceiver)
        {
            this.fileReceiver = fileReceiver;
        }

        protected override void Handle(FilePart filePart)
        {
            if (fileReceiver.Request is not {Accepted: true})
            {
                return;
            }

            try
            {
                SavePart(filePart);
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
            }
        }

        private void SavePart(FilePart filePart)
        {
            var base64Data = filePart.Data;
            var data = Convert.FromBase64String(base64Data);
            fileReceiver.WriteFileStream.Write(data);
            fileReceiver.Request.PartsTransferred++;
        }

        private void CatchFileSystemExceptions(Exception e)
        {
            if (e is not (
                DirectoryNotFoundException or
                FileNotFoundException or
                IOException or
                UnauthorizedAccessException))
            {
                throw e;
            }

            fileReceiver.CancelReceive(false);
            fileReceiver.OnFileTransferError();
            Trace.WriteLine("Cannot access file system");
        }
    }
}