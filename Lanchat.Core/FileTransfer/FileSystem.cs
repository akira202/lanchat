using System.IO;
using Lanchat.Core.Config;

namespace Lanchat.Core.FileTransfer
{
    internal class FileSystem : IFileSystem
    {
        private readonly IConfig config;

        internal FileSystem(IConfig config)
        {
            this.config = config;
        }

        public FileStream OpenWriteStream(string path)
        {
            return new(path, FileMode.Append);
        }

        public void DeleteIncompleteFile(string path)
        {
            File.Delete(path);
        }

        public string GetFilePath(string file)
        {
            var path = Path.Combine(config.ReceivedFilesDirectory, file);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);

            for (var i = 1;; ++i)
            {
                if (!File.Exists(path))
                {
                    return path;
                }

                path = Path.Combine(config.ReceivedFilesDirectory, $"{fileName}({i}){fileExt}");
            }
        }
    }
}