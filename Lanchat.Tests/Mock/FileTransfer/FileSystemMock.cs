using System;
using System.IO;
using Lanchat.Core.FileTransfer;

namespace Lanchat.Tests.Mock.FileTransfer
{
    public class FileSystemMock : IFileSystem
    {
        public FileStream OpenWriteStream(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteIncompleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public string GetFilePath(string file)
        {
            throw new NotImplementedException();
        }
    }
}