using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class NicknameUpdate
    {
        [MaxLength(20)] public string NewNickname { get; init; }
    }
}