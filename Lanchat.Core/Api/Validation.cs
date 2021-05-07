using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Lanchat.Core.Network;

namespace Lanchat.Core.Api
{
    internal class Validation
    {
        private readonly INodeInternal node;

        public Validation(INodeInternal node)
        {
            this.node = node;
        }

        internal bool CheckPreconditions(IApiHandler handler, object data)
        {
            if (!node.Ready && handler.Privileged == false)
            {
                Trace.WriteLine($"{node.Id} must be ready to handle this type of data.");
                return false;
            }

            if (handler.Disabled)
            {
                Trace.WriteLine("Handler disabled");
                return false;
            }

            if (!Validator.TryValidateObject(data, new ValidationContext(data), new List<ValidationResult>()))
            {
                Trace.WriteLine($"Node {node.Id} received invalid json");
                return false;
            }

            return true;
        }
    }
}