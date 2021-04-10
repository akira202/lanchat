using System.IO;
using Lanchat.ClientCore;

namespace Lanchat.Tests.Mock
{
    public static class FileOperations
    {
        public static void Prepare()
        {
            Storage.DataPath = "data";
            Storage.DownloadsPath = "data";

            if (!Directory.Exists(Storage.DataPath)) return;
            Directory.Delete(Storage.DataPath, true);
            Storage.CreateStorageDirectoryIfNotExists();
        }
    }
}