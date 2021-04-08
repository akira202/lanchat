using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Lanchat.ClientCore
{
    public static class Logging
    {
        public static void StartLogging()
        {
            AppDomain.CurrentDomain.FirstChanceException += (_, eventArgs) =>
            {
                // Console error logging is disabled because a lot is thrown when the window is resized.
                if (eventArgs.Exception.Source != "System.Console") Trace.WriteLine(eventArgs.Exception);
            };

            Trace.Listeners.Add(
                new FileTraceListener($"{ConfigManager.DataPath}/{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("Logging started");
        }

        public static void DeleteOldLogs()
        {
            foreach (var fi in new DirectoryInfo(ConfigManager.DataPath)
                .GetFiles("*.log")
                .OrderByDescending(x => x.LastWriteTime)
                .Skip(5))
                fi.Delete();
        }
    }
}