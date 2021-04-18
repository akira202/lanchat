using System;
using System.IO;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File receiving.
    /// </summary>
    public class FileReceiver
    {
        private readonly IConfig config;
        private readonly NetworkOutput networkOutput;
        internal FileStream WriteFileStream;

        internal FileReceiver(NetworkOutput networkOutput, IConfig config)
        {
            this.networkOutput = networkOutput;
            this.config = config;
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
            ResetRequest();
            return true;
        }
        
        internal void HandleReceiveRequest(FileTransferControl request)
        {
            Request = new FileTransferRequest
            {
                FilePath = GetUniqueFileName(Path.Combine(config.ReceivedFilesDirectory, request.FileName)),
                Parts = request.Parts
            };
            FileTransferRequestReceived?.Invoke(this, Request);
        }

        internal void HandleSenderError()
        {
            if (Request == null) return;
            File.Delete(Request.FilePath);
            OnFileTransferError();
            ResetRequest();
        }

        internal void OnFileTransferFinished(FileTransferRequest e)
        {
            FileReceiveFinished?.Invoke(this, e);
        }

        internal void OnFileTransferError()
        {
            FileTransferError?.Invoke(this, new FileTransferException(Request));
        }
        
        private void ResetRequest()
        {
            Request = null;
            WriteFileStream.Dispose();
        }
        
        private static string GetUniqueFileName(string file)
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
    }
}