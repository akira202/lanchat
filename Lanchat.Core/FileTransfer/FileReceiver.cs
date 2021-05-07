using System;
using System.IO;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File receiving.
    /// </summary>
    public class FileReceiver
    {
        private readonly FileReceivingControl fileReceivingControl;
        private readonly IFileSystem fileSystem;
        internal FileStream WriteFileStream;

        internal FileReceiver(IOutput output, IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            fileReceivingControl = new FileReceivingControl(output);
        }

        /// <summary>
        ///     Incoming file request.
        /// </summary>
        public FileTransferRequest Request { get; private set; }

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
            if (Request == null)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            Request.Accepted = true;
            WriteFileStream = fileSystem.OpenWriteStream(Request.FilePath);
            fileReceivingControl.Accept();
        }

        /// <summary>
        ///     Reject incoming file request.
        /// </summary>
        /// <exception cref="InvalidOperationException">No awaiting request</exception>
        public void RejectRequest()
        {
            if (Request == null)
            {
                throw new InvalidOperationException("No pending requests ");
            }

            Request = null;
            fileReceivingControl.Reject();
        }

        /// <summary>
        ///     Cancel current receive request.
        /// </summary>
        public void CancelReceive(bool deleteFile)
        {
            if (Request == null)
            {
                throw new InvalidOperationException("No file transfers in progress");
            }

            fileReceivingControl.Cancel();
            if (deleteFile)
            {
                fileSystem.DeleteIncompleteFile(Request.FilePath);
            }

            FileTransferError?.Invoke(this, new FileTransferException(Request));
            ResetRequest();
        }

        internal void FinishReceive()
        {
            if (Request == null)
            {
                return;
            }

            FileReceiveFinished?.Invoke(this, Request);
            ResetRequest();
        }

        internal void HandleReceiveRequest(FileTransferControl request)
        {
            if (Request != null)
            {
                return;
            }

            Request = new FileTransferRequest
            {
                FilePath = fileSystem.GetFilePath(request.FileName),
                Parts = request.Parts
            };
            FileTransferRequestReceived?.Invoke(this, Request);
        }

        internal void HandleSenderError()
        {
            if (Request == null)
            {
                return;
            }

            fileSystem.DeleteIncompleteFile(Request.FilePath);
            OnFileTransferError();
            ResetRequest();
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
    }
}