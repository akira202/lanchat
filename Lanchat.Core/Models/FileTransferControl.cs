namespace Lanchat.Core.Models
{
    internal class FileTransferControl
    {
        public RequestStatus RequestStatus { get; init; }
        public string FileName { get; init; }
        public long Parts { get; init; }
    }

    internal enum RequestStatus
    {
        Sending,
        Accepted,
        Rejected,
        Errored,
        Canceled,
        Finished
    }
}