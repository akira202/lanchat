using Lanchat.Core.Api;
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
            if (message.Private)
            {
                messaging.OnPrivateMessageReceived(message.Content);
            }
            else
            {
                messaging.OnMessageReceived(message.Content);
            }
        }
    }
}