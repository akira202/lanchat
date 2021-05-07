using System;
using System.Text.Json;
using Lanchat.Core.Json;

namespace Lanchat.Core.Api
{
    internal class Input : IInput
    {
        private readonly JsonBuffer jsonBuffer;
        private readonly IResolver resolver;

        public Input(IResolver resolver)
        {
            jsonBuffer = new JsonBuffer();
            this.resolver = resolver;
        }

        public void OnDataReceived(object sender, string item)
        {
            jsonBuffer.AddToBuffer(item);
            try
            {
                jsonBuffer.ReadBuffer().ForEach(resolver.CallHandler);
            }
            catch (JsonException)
            { }
            catch (InvalidOperationException)
            { }
        }
    }
}