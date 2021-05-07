using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     File sending.
    /// </summary>
    public class FileSender
    {
        private const int ChunkSize = 1024 * 1024;
        private readonly FileSendingControl fileSendingControl;
        private readonly IOutput output;
        private bool disposing;

        internal FileSender(IOutput output)
        {
            this.output = output;
            fileSendingControl = new FileSendingControl(output);
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
        public event EventHandler<FileTransferRequest> AcceptedByReceiver;

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
        /// <exception cref="InvalidOperationException">Only one file can be send at same time</exception>
        public void CreateSendRequest(string path)
        {
            if (Request != null)
            {
                throw new InvalidOperationException("File transfer already in progress");
            }

            var fileInfo = new FileInfo(Path.Combine(path));

            Request = new FileTransferRequest
            {
                FilePath = path,
                Parts = (fileInfo.Length + ChunkSize - 1) / ChunkSize
            };

            fileSendingControl.Request(Request);
        }

        internal void SendFile()
        {
            AcceptedByReceiver?.Invoke(this, Request);

            try
            {
                var file = new FileReader(Request.FilePath, ChunkSize);
                do
                {
                    var buffer = file.ReadChunk();
                    if (disposing)
                    {
                        OnFileTransferError(new FileTransferException(Request));
                        return;
                    }

                    var part = new FilePart
                    {
                        Data = Convert.ToBase64String(buffer.Take(file.BytesRead).ToArray())
                    };

                    output.SendData(part);
                    Request.PartsTransferred++;
                } while (file.BytesRead > 0);

                FileSendFinished?.Invoke(this, Request);
                fileSendingControl.Finished();
                file.Dispose();
                Request = null;
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
            }
        }

        internal void HandleReject()
        {
            if (Request == null)
            {
                return;
            }

            FileTransferRequestRejected?.Invoke(this, Request);
            Request = null;
        }

        internal void HandleCancel()
        {
            if (Request == null || Request.Accepted == false)
            {
                return;
            }

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

            OnFileTransferError(new FileTransferException(Request));
            fileSendingControl.Errored();
            Request = null;
            Trace.WriteLine("Cannot access file system");
        }
    }
}