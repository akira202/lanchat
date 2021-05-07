using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class ConnectionControl
    {
        [Required] public ConnectionStatus Status { get; init; }
    }

    internal enum ConnectionStatus
    {
        RemoteDisconnect
    }
}