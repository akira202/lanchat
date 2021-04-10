using System.IO;
using Lanchat.ClientCore;

namespace Lanchat.Tests.Mock
{
    public static class FileOperations
    {
        private static int _counter;
        public static void Prepare()
        {
            Storage.DataPath = $"data_{_counter}";
            Storage.DownloadsPath = Storage.DataPath;
            Storage.CreateStorageDirectoryIfNotExists();
            _counter++;
        }

        public static void CleanUp()
        {
            if (!Directory.Exists(Storage.DataPath)) return;
            Directory.Delete(Storage.DataPath, true);
        }
    }
}