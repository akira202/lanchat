using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class KeyInfoHandler : ApiHandler<KeyInfo>
    {
        private readonly Node node;

        internal KeyInfoHandler(Node node)
        {
            this.node = node;
            Privileged = true;
        }

        protected override void Handle(KeyInfo keyInfo)
        {
            if (node.Ready) return;
            if (!node.HandshakeReceived) return;

            if (keyInfo == null) return;

            try
            {
                node.SymmetricEncryption.ImportKey(keyInfo);
                node.Ready = true;
                node.OnConnected();
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
        }
    }
}