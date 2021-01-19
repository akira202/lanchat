namespace Lanchat.Core.Models
{
    public class Handshake
    {
        public string Nickname { get; set; }
        public Status Status { get; set; }
        public PublicKey PublicKey { get; set; }
    }
}