using System;
using System.IO;
using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File receiving.
    /// </summary>
    public class FileReceiver
    {
        private readonly IConfig config;
        internal readonly SymmetricEncryption Encryption;
        private readonly NetworkOutput networkOutput;
        internal FileStream WriteFileStream;

        internal FileReceiver(NetworkOutput networkOutput, SymmetricEncryption encryption, IConfig config)
        {
            this.networkOutput = networkOutput;
            this.config = config;
            Encryption = encryption;
        }

        /// <summary>
        ///     Incoming file request.
        /// </summary>
        public FileTransferRequest Request { get; internal set; }

        /// <summary>
        ///     File transfer finished.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileReceiveFinished;

        /// <summary>
        ///     File transfer errored.
        /// </summary>
        public event EventHandler<FileTransferException> FileTransferError;

        /// <summary>
        ///     File receive request received.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileTransferRequestReceived;

        /// <summary>
        ///     File send request accepted. File transfer in progress.
        /// </summary>
        public event EventHandler<FileTransferRequest> FileTransferRequestAccepted;

        /// <summary>
        ///     Accept incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void AcceptRequest()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            Request.Accepted = true;
            WriteFileStream = new FileStream(Request.FilePath, FileMode.Append);
            networkOutput.SendData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Accepted
            });
            FileTransferRequestAccepted?.Invoke(this, Request);
        }

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void RejectRequest()
        {
            if (Request == null) throw new InvalidOperationException("No receive request");
            Request = null;
            networkOutput.SendData(new FileTransferControl
            {
                RequestStatus = RequestStatus.Rejected
            });
        }

        /// <summary>
        ///     Cancel current receive request.
        /// </summary>
        public bool CancelReceive()
        {
            if (Request == null) return false;
            networkOutput.SendData(
                new FileTransferControl
                {
                    RequestStatus = RequestStatus.Canceled
                });

            File.Delete(Request.FilePath);
            FileTransferError?.Invoke(this, new FileTransferException(Request));
            Request = null;
            WriteFileStream.Dispose();
            return true;
        }

        internal void HandleReceiveRequest(FileTransferControl request)
        {
            Request = new FileTransferRequest
            {
                FilePath = MakeUnique(Path.Combine(config.ReceivedFilesDirectory, request.FileName)),
                Parts = request.Parts
            };
            FileTransferRequestReceived?.Invoke(this, Request);
        }

        internal void HandleSenderError()
        {
            if (Request == null) return;
            WriteFileStream.Dispose();
            File.Delete(Request.FilePath);
            OnFileTransferError();
            Request = null;
        }

        private static string MakeUnique(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var fileExt = Path.GetExtension(file);

            for (var i = 1;; ++i)
            {
                if (!File.Exists(file))
                    return file;
                file = $"{fileName}({i}){fileExt}";
            }
        }

        internal void OnFileTransferFinished(FileTransferRequest e)
        {
            FileReceiveFinished?.Invoke(this, e);
        }

        internal void OnFileTransferError()
        {
            FileTransferError?.Invoke(this, new FileTransferException(Request));
        }
    }
}