using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Lanchat.ClientCore
{
    /// <summary>
    ///     Saving Trace messages to file.
    /// </summary>
    public static class Logger
    {
        private static FileTraceListener _fileTraceListener;

        /// <summary>
        ///     Create log file and start logging.
        /// </summary>
        public static void StartLogging()
        {
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;
            var logPath = $"{Storage.DataPath}/{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log";
            _fileTraceListener = new FileTraceListener(logPath);
            Trace.Listeners.Add(_fileTraceListener);
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("Logging started");
        }

        /// <summary>
        ///     Stop writing to log file.
        /// </summary>
        public static void StopLogging()
        {
            _fileTraceListener.Dispose();
            AppDomain.CurrentDomain.FirstChanceException -= OnFirstChanceException;
        }

        /// <summary>
        ///     Delete old log files.
        /// </summary>
        /// <param name="maxLogsCount">Number of log files to preserve.</param>
        public static void DeleteOldLogs(int maxLogsCount)
        {
            foreach (var fi in new DirectoryInfo(Storage.DataPath)
                .GetFiles("*.log")
                .OrderByDescending(x => x.LastWriteTime)
                .Skip(maxLogsCount))
            {
                fi.Delete();
            }
        }

        private static void OnFirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            if (e.Exception.Source != null && e.Exception.Source.StartsWith("Lanchat"))
            {
                Trace.WriteLine(e.Exception);
            }
        }
    }
}