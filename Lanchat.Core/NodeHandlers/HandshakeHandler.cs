using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly Node node;

        internal HandshakeHandler(Node node)
        {
            this.node = node;
            Privileged = true;
        }

        protected override void Handle(Handshake handshake)
        {
            if (node.HandshakeReceived) return;
            node.Nickname = handshake.Nickname;

            if (!node.IsSession) node.SendHandshake();

            try
            {
                node.PublicKeyEncryption.ImportKey(handshake.PublicKey);
                node.Status = handshake.Status;
                node.NetworkOutput.SendPrivilegedData(node.SymmetricEncryption.ExportKey());
                node.HandshakeReceived = true;
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
        }
    }
}