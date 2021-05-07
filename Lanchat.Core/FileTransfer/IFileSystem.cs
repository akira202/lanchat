using System.IO;

namespace Lanchat.Core.FileTransfer
{
    internal interface IFileSystem
    {
        FileStream OpenWriteStream(string path);
        void DeleteIncompleteFile(string path);
        string GetFilePath(string file);
    }
}