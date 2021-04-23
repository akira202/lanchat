using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Lanchat.Core.Encryption;
using Lanchat.Core.Json;
using Lanchat.Core.Node;

namespace Lanchat.Core.Api
{
    /// <summary>
    ///     Class used to handle received data.
    /// </summary>
    public class Resolver : IResolver
    {
        private readonly IModelEncryption encryption;
        private readonly List<IApiHandler> handlers = new();
        private readonly JsonBuffer jsonBuffer;
        private readonly JsonUtils jsonUtils;
        private readonly INodeInternal node;

        internal Resolver(INodeInternal node)
        {
            this.node = node;
            encryption = node.ModelEncryption;
            jsonUtils = new JsonUtils();
            jsonBuffer = new JsonBuffer();
        }

        /// <summary>
        ///     Add data handler for specific model type.
        /// </summary>
        /// <param name="apiHandler">ApiHandler object.</param>
        public void RegisterHandler(IApiHandler apiHandler)
        {
            handlers.Add(apiHandler);
            jsonUtils.KnownModels.Add(apiHandler.HandledType);
        }

        public void OnDataReceived(object sender, string item)
        {
            jsonBuffer.AddToBuffer(item);
            try
            {
                jsonBuffer.ReadBuffer().ForEach(CallHandler);
            }
            catch (JsonException)
            { }
            catch (InvalidOperationException)
            { }
        }

        internal void CallHandler(string item)
        {
            var data = jsonUtils.Deserialize(item);
            var handler = GetHandler(data.GetType());
            if (!CheckPreconditions(handler, data))
            {
                return;
            }

            encryption.DecryptObject(data);
            Trace.WriteLine($"Node {node.Id} received {handler.HandledType.Name}");
            handler.Handle(data);
        }

        private bool CheckPreconditions(IApiHandler handler, object data)
        {
            if (!node.Ready && handler.Privileged == false)
            {
                Trace.WriteLine($"{node.Id} must be ready to handle this type of data.");
                return false;
            }

            if (!Validator.TryValidateObject(data, new ValidationContext(data), new List<ValidationResult>()))
            {
                Trace.WriteLine($"Node {node.Id} received invalid json");
                return false;
            }

            return true;
        }

        private IApiHandler GetHandler(Type jsonType)
        {
            return handlers.First(x => x.HandledType == jsonType);
        }
    }
}