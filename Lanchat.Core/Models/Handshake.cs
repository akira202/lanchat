namespace Lanchat.Core.Models
{
    public class Handshake
    {
        public string Nickname { get; set; }
        public PublicKey PublicKey { get; set; }
        public bool Relay { get; set; }
    }
}