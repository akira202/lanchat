using System.Diagnostics;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal
{
    public class TraceListener : TextWriterTraceListener
    {
        private readonly DebugView debugView;
        public TraceListener()
        {
            debugView = Program.Window.TabsManager.AddDebugView();
        }
        
        public override void WriteLine(string message)
        {
            debugView.AddToLog(message);
        } 
    }
}