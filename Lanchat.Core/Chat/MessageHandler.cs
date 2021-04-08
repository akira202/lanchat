using System;
using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.Chat
{
    internal class MessageHandler : ApiHandler<Message>
    {
        private readonly Messaging messaging;

        internal MessageHandler(Messaging messaging)
        {
            this.messaging = messaging;
        }

        protected override void Handle(Message message)
        {
            try
            {
                DecryptMessageAndRaiseEvent(message);
            }
            catch (CryptographicException)
            {
            }
            catch (FormatException)
            {
            }
        }

        private void DecryptMessageAndRaiseEvent(Message message)
        {
            var decryptedMessage = messaging.Encryption.DecryptString(message.Content);

            if (message.Private)
            {
                messaging.OnPrivateMessageReceived(decryptedMessage);
            }
            else
            {
                messaging.OnMessageReceived(decryptedMessage);
            }
        }
    }
}