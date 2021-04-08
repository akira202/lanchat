namespace Lanchat.Core.Models
{
    internal class KeyInfo
    {
        public byte[] AesKey { get; init; }
        public byte[] AesIv { get; init; }
    }
}