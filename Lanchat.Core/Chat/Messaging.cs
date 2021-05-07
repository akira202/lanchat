using System;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.Chat
{
    /// <summary>
    ///     Basic chat features.
    /// </summary>
    public class Messaging
    {
        private readonly IOutput output;

        internal Messaging(IOutput output)
        {
            this.output = output;
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
            output.SendData(new Message {Content = content});
        }

        /// <summary>
        ///     Send private message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendPrivateMessage(string content)
        {
            output.SendData(new Message
            {
                Content = content,
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