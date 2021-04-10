using System.IO;
using System.Net;
using Lanchat.ClientCore;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.ClientCore
{
    [NonParallelizable]
    public class ConfigManagerTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            FileOperations.Prepare();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            FileOperations.CleanUp();
        }

        [Test]
        public void CreatingNewConfig()
        {
            var config = Storage.LoadConfig();
            Assert.IsTrue(config.Fresh);
        }

        [Test]
        public void ConfigLoading()
        {
            Storage.SaveConfig(new Config());
            var config = Storage.LoadConfig();
            Assert.IsFalse(config.Fresh);
        }

        [Test]
        public void ConfigSaving()
        {
            var config = Storage.LoadConfig();
            config.Nickname = "test";
            var loadedConfig = Storage.LoadConfig();
            Assert.AreEqual(config.Nickname, loadedConfig.Nickname);
        }
        
        [Test]
        public void ListSaving()
        {
            var config = Storage.LoadConfig();
            config.BlockedAddresses.Add(IPAddress.Loopback);
            config.SavedAddresses.Add(IPAddress.Loopback);
            var loadedConfig = Storage.LoadConfig();
            Assert.Contains(IPAddress.Loopback, loadedConfig.BlockedAddresses);
            Assert.Contains(IPAddress.Loopback, loadedConfig.SavedAddresses);
        }
        
        [Test]
        public void LoadingInvalidJson()
        {
            File.WriteAllText(Storage.ConfigPath, "not a json");
            var config = Storage.LoadConfig();
            Assert.IsTrue(config.Fresh);
        }
        
        [Test]
        public void CreatingDirectory()
        {
            Directory.Delete(Storage.DataPath, true);
            Storage.CreateStorageDirectoryIfNotExists();
            Assert.IsTrue(Directory.Exists(Storage.DataPath));
        }
    }
}