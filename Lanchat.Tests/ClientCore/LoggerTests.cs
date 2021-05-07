using System.Diagnostics;
using System.IO;
using System.Linq;
using Lanchat.ClientCore;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.ClientCore
{
    [NonParallelizable]
    public class LoggerTests
    {
        [SetUp]
        public void Setup()
        {
            FileOperations.Prepare();
        }

        [TearDown]
        public void TearDown()
        {
            FileOperations.CleanUp();
        }

        [Test]
        public void AddToLog()
        {
            Logger.StartLogging();
            Trace.WriteLine("test");
            Logger.StopLogging();
            var files = Directory.GetFiles(Storage.DataPath, "*.log");
            var logFile = File.ReadAllLines(files.Last());
            var test = logFile.Last().EndsWith("test");
            Assert.IsTrue(test);
        }

        [Test]
        public void ClearLogsDirectory()
        {
            Logger.StartLogging();
            Logger.StopLogging();
            Logger.DeleteOldLogs(0);
            Assert.AreEqual(0, new DirectoryInfo(Storage.DataPath).GetFiles().Length);
        }
    }
}