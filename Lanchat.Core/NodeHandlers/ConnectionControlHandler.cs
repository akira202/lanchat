using System.Diagnostics;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class ConnectionControlHandler : ApiHandler<ConnectionControl>
    {
        private readonly Node node;

        internal ConnectionControlHandler(Node node)
        {
            this.node = node;
        }

        protected override void Handle(ConnectionControl connectionControl)
        {
            switch (connectionControl.Status)
            {
                case ConnectionControlStatus.RemoteClose:
                    node.NetworkElement.Close();
                    break;

                default:
                    Trace.WriteLine("Invalid status received");
                    break;
            }
        }
    }
}