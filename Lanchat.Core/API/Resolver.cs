using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.API
{
    /// <summary>
    ///     Class used to handle received data.
    /// </summary>
    public class Resolver
    {
        private readonly List<IApiHandler> handlers = new();
        private readonly INodeState nodeState;
        private readonly JsonReader jsonReader;
        private readonly JsonBuffer jsonBuffer;

        internal Resolver(INodeState nodeState)
        {
            this.nodeState = nodeState;
            jsonReader = new JsonReader();
            jsonBuffer = new JsonBuffer();
        }

        /// <summary>
        ///     Add data handler for specific model type.
        /// </summary>
        /// <param name="apiHandler">ApiHandler object.</param>
        public void RegisterHandler(IApiHandler apiHandler)
        {
            handlers.Add(apiHandler);
            jsonReader.KnownModels.Add(apiHandler.HandledType);
        }

        internal void OnDataReceived(object sender, string item)
        {
            jsonBuffer.AddToBuffer(item);
            try
            {
                jsonBuffer.ReadBuffer().ForEach(CallHandler);
            }
            catch (JsonException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }

        internal void CallHandler(string item)
        {
            var data = jsonReader.Deserialize(item);
            var handler = GetHandler(data.GetType());
            if (!CheckPreconditions(handler, data)) return;
            Trace.WriteLine($"Node {nodeState.Id} received {handler.HandledType.Name}");
            handler.Handle(data);
        }

        private bool CheckPreconditions(IApiHandler handler, object data)
        {
            if (!nodeState.Ready && handler.Privileged == false)
            {
                Trace.WriteLine($"{nodeState.Id} must be ready to handle this type of data.");
                return false;
            }

            if (!Validator.TryValidateObject(data, new ValidationContext(data), new List<ValidationResult>()))
            {
                Trace.WriteLine($"Node {nodeState.Id} received invalid json");
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