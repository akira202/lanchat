using Lanchat.Core.Encryption;

namespace Lanchat.Tests.Mock.Models
{
    public class EncryptedModel
    {
        [Encrypt] public string Property { get; set; }
    }
}