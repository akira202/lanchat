using Lanchat.Core.Api;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.ApiHandlers
{
    internal class StatusUpdateHandler : ApiHandler<StatusUpdate>
    {
        private readonly INodeInternal node;

        internal StatusUpdateHandler(INodeInternal node)
        {
            this.node = node;
        }

        protected override void Handle(StatusUpdate status)
        {
            node.Status = status.NewStatus;
        }
    }
}