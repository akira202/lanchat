namespace Lanchat.Core.Models
{
    internal class PublicKey
    {
        public byte[] RsaModulus { get; init; }
        public byte[] RsaExponent { get; init; }
    }
}