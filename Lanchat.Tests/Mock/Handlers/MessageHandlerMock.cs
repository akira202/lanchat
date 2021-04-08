using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Tests.Mock.Handlers
{
    internal class MessageHandlerMock : ApiHandler<Message>
    {
        public bool Received;

        protected override void Handle(Message data)
        {
            Received = true;
        }
    }
}