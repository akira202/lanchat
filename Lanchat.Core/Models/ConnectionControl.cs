using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class ConnectionControl
    {
        [Required] public ConnectionControlStatus Status { get; init; }
    }

    internal enum ConnectionControlStatus
    {
        RemoteClose
    }
}