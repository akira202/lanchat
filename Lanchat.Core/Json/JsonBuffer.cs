using System;
using System.Collections.Generic;
using System.Linq;

namespace Lanchat.Core.Json
{
    internal class JsonBuffer
    {
        private string buffer;
        private string currentJson;

        internal void AddToBuffer(string dataString)
        {
            buffer += dataString;
        }

        internal List<string> ReadBuffer()
        {
            var index = buffer.LastIndexOf("}", StringComparison.Ordinal);
            if (index < 0)
            {
                return new List<string>();
            }

            currentJson = buffer[..(index + 1)];
            buffer = buffer[(index + 1)..];
            return currentJson.Replace("}{", "}|{").Split('|').ToList();
        }
    }
}