using System;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Tests.Mock
{
    public class NodeState : INodeState
    {
        public bool Ready { get; set; } = true;
        public Guid Id { get; } = Guid.NewGuid();
    }
}