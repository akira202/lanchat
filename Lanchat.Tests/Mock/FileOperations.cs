using System;
using System.IO;
using Lanchat.ClientCore;

namespace Lanchat.Tests.Mock
{
    public static class FileOperations
    {
        private static Guid _guid;
        public static void Prepare()
        {
            _guid = Guid.NewGuid();
            Storage.DataPath = $"test-{_guid}";
            Storage.DownloadsPath = Storage.DataPath;
            Storage.CreateStorageDirectoryIfNotExists();
        }

        public static void CleanUp()
        {
            Directory.Delete(Storage.DataPath, true);
        }
    }
}