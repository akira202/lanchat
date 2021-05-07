using System;
using System.Security.Cryptography;
using Lanchat.Core.Api;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.ApiHandlers
{
    internal class KeyInfoHandler : ApiHandler<KeyInfo>
    {
        private readonly ISymmetricEncryption encryption;
        private readonly INodeInternal node;

        internal KeyInfoHandler(ISymmetricEncryption encryption, INodeInternal node)
        {
            this.encryption = encryption;
            this.node = node;
            Privileged = true;
        }

        protected override void Handle(KeyInfo keyInfo)
        {
            try
            {
                encryption.ImportKey(keyInfo);
                node.Ready = true;
                Disabled = true;
                node.OnConnected();
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
            catch (ArgumentNullException)
            {
                node.OnCannotConnect();
            }
        }
    }
}