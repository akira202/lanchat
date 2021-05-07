using System;
using System.Threading.Tasks;

namespace Lanchat.Core.Network
{
    internal class Connection
    {
        private readonly INodeInternal nodeInternal;

        internal Connection(INodeInternal nodeInternal)
        {
            this.nodeInternal = nodeInternal;
            nodeInternal.Host.Disconnected += OnDisconnected;
        }

        internal void Initialize()
        {
            if (nodeInternal.IsSession)
            {
                nodeInternal.SendHandshake();
            }

            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!nodeInternal.Ready)
                {
                    nodeInternal.OnCannotConnect();
                }
            });
        }

        private void OnDisconnected(object sender, EventArgs _)
        {
            if (nodeInternal.Ready)
            {
                nodeInternal.OnDisconnected();
            }
            else
            {
                nodeInternal.OnCannotConnect();
            }
        }
    }
}