using System;
using System.Diagnostics;

namespace Lanchat.ClientCore
{
    internal class FileTraceListener : TextWriterTraceListener
    {
        internal FileTraceListener(string fileName) : base(fileName)
        { }

        public override void WriteLine(string message)
        {
            base.WriteLine(DateTime.Now.ToString("[HH:mm:ss] ") + message);
        }
    }
}