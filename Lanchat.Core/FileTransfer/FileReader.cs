using System;
using System.Collections.Generic;
using System.IO;

namespace Lanchat.Core.FileTransfer
{
    internal class FileReader : IDisposable
    {
        private readonly byte[] buffer;
        private readonly FileStream fileStream;

        internal FileReader(string filePath, int chunkSize)
        {
            fileStream = File.OpenRead(filePath);
            buffer = new byte[chunkSize];
        }

        internal int BytesRead { get; private set; }

        public void Dispose()
        {
            fileStream?.Dispose();
        }

        internal IEnumerable<byte> ReadChunk()
        {
            BytesRead = fileStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}