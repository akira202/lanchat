using Lanchat.Core.Encryption;
using Lanchat.Core.Json;
using Lanchat.Core.Network;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Api
{
    /// <summary>
    ///     Send data other of type not belonging to standard Lanchat.Core set.
    /// </summary>
    public class Output : IOutput
    {
        private readonly IModelEncryption encryption;
        private readonly IHost host;
        private readonly JsonUtils jsonUtils;
        private readonly INodeInternal node;

        internal Output(IHost host, INodeInternal node)
        {
            this.host = host;
            this.node = node;
            encryption = node.ModelEncryption;
            jsonUtils = new JsonUtils();
        }

        /// <summary>
        ///     Send data.
        /// </summary>
        /// <param name="content">Object to send.</param>
        public void SendData(object content)
        {
            if (!node.Ready)
            {
                return;
            }

            encryption.EncryptObject(content);
            host.Send(jsonUtils.Serialize(content));
        }

        /// <summary>
        ///     Send the data before marking the node as ready (Handshake, KeyInfo...).
        /// </summary>
        /// <param name="content">Object to send.</param>
        public void SendPrivilegedData(object content)
        {
            encryption.EncryptObject(content);
            host.Send(jsonUtils.Serialize(content));
        }
    }
}