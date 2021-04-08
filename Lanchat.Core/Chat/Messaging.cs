using System;
using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.Chat
{
    /// <summary>
    ///     Basic chat features.
    /// </summary>
    public class Messaging
    {
        internal readonly SymmetricEncryption Encryption;
        private readonly NetworkOutput networkOutput;

        internal Messaging(NetworkOutput networkOutput, SymmetricEncryption encryption)
        {
            this.networkOutput = networkOutput;
            Encryption = encryption;
        }

        /// <summary>
        ///     Message received.
        /// </summary>
        public event EventHandler<string> MessageReceived;

        /// <summary>
        ///     Private message received.
        /// </summary>
        public event EventHandler<string> PrivateMessageReceived;

        /// <summary>
        ///     Send message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendMessage(string content)
        {
            networkOutput.SendData(new Message {Content = Encryption.EncryptString(content)});
        }

        /// <summary>
        ///     Send private message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendPrivateMessage(string content)
        {
            networkOutput.SendData(new Message
            {
                Content = Encryption.EncryptString(content),
                Private = true
            });
        }

        internal void OnMessageReceived(string e)
        {
            MessageReceived?.Invoke(this, e);
        }

        internal void OnPrivateMessageReceived(string e)
        {
            PrivateMessageReceived?.Invoke(this, e);
        }
    }
}