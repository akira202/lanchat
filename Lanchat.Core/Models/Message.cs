using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class Message
    {
        [Required] [MaxLength(1500)] public string Content { get; init; }
        public bool Private { get; init; }
    }
}