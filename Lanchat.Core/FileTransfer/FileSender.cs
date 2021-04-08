using System;
using System.IO;
using System.Linq;
using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    // TODO: Refactor
    /// <summary>
    ///     File sending.
    /// </summary>
    public class FileSender
    {
        private const int ChunkSize = 1024 * 1024;
        private readonly SymmetricEncryption encryption;
        private readonly NetworkOutput networkOutput;
        private bool disposing;

        internal FileSender(NetworkOutput networkOutput, SymmetricEncryption encryption)
        {
            this.networkOutput = networkOutput;
            this.encryption = encryption;
        }

        /// <summary>
        ///     Outgoing file request.
        /// </summary>
        public FileTransferRequest Request { get; private set; }

        /// <summary>
        ///     File send returned error.
        /// </summary>
        public event EventHandler<FileTransferException> FileTransferError;

        /// <summary>
        ///     File send request accepted. File transfer in progress.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileTransferRequestAccepted;

        /// <summary>
        ///     File send request accepted.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileTransferRequestRejected;

        /// <summary>
        ///     File transfer finished.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileSendFinished;

        /// <summary>
        ///     Send file exchange request.
        /// </summary>
        /// <param name="path">File path</param>
        public void CreateSendRequest(string path)
        {
            if (Request != null) throw new InvalidOperationException("File transfer in progress");

            var fileInfo = new FileInfo(Path.Combine(path));

            Request = new FileTransferRequest
            {
                FilePath = path,
                Parts = (fileInfo.Length + ChunkSize - 1) / ChunkSize
            };

            networkOutput.SendData(
                new FileTransferControl
                {
                    FileName = Request.FileName,
                    RequestStatus = RequestStatus.Sending,
                    Parts = Request.Parts
                });
        }

        internal void SendFile()
        {
            FileTransferRequestAccepted?.Invoke(this, Request);

            try
            {
                var buffer = new byte[ChunkSize];
                int bytesRead;
                using var file = File.OpenRead(Request.FilePath);
                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (disposing)
                    {
                        OnFileTransferError(new FileTransferException(Request));
                        return;
                    }

                    var part = new FilePart
                    {
                        Data = encryption.EncryptBytes(buffer.Take(bytesRead).ToArray())
                    };

                    if (bytesRead < ChunkSize) part.Last = true;
                    networkOutput.SendData(part);
                    Request.PartsTransferred++;
                }

                FileSendFinished?.Invoke(this, Request);
                Request = null;
            }
            catch
            {
                OnFileTransferError(new FileTransferException(Request));
                networkOutput.SendData(
                    new FileTransferControl
                    {
                        RequestStatus = RequestStatus.Errored
                    });
                Request = null;
            }
        }

        internal void HandleReject()
        {
            FileTransferRequestRejected?.Invoke(this, Request);
            Request = null;
        }

        internal void HandleCancel()
        {
            if (Request == null) return;
            OnFileTransferError(new FileTransferException(Request));
            Request = null;
        }

        internal void Dispose()
        {
            disposing = true;
        }

        private void OnFileTransferError(FileTransferException e)
        {
            FileTransferError?.Invoke(this, e);
        }
    }
}