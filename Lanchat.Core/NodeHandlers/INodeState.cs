using System;

namespace Lanchat.Core.NodeHandlers
{
    internal interface INodeState
    {
        bool Ready { get; }
        Guid Id { get; }
    }
}