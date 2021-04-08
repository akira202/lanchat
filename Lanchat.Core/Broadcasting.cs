namespace Lanchat.Core
{
    /// <summary>
    ///     Send data to all nodes.
    /// </summary>
    public class Broadcasting
    {
        private readonly P2P network;

        internal Broadcasting(P2P network)
        {
            this.network = network;
        }

        /// <summary>
        ///     Broadcast message.
        /// </summary>
        /// <param name="message">Message content.</param>
        public void SendMessage(string message)
        {
            network.Nodes.ForEach(x => x.Messaging.SendMessage(message));
        }

        /// <summary>
        ///     Broadcast data.
        /// </summary>
        /// <param name="data"></param>
        public void SendData(object data)
        {
            network.Nodes.ForEach(x => x.NetworkOutput.SendData(data));
        }
    }
}