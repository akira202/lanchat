using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using Lanchat.ClientCore;
using Lanchat.Core;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public static class Program
    {
        public static P2P Network { get; private set; }
        public static Config Config { get; private set; }

        private static void Main(string[] args)
        {
            Config = Storage.LoadConfig();

            // Load resources
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(Config.Language);
            }
            catch
            {
                Trace.WriteLine("Cannot load translation. Using default.");
            }

            Resources.Culture = CultureInfo.CurrentCulture;

            // Initialize p2p mode and ui
            try
            {
                Ui.Start();
                Network = new P2P(Config);
                Network.NodeCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };

                if (args.Contains("--no-saved") || args.Contains("-a")) Config.ConnectToSaved = false;
                if (args.Contains("--no-udp") || args.Contains("-b")) Config.NodesDetection = false;
                if (args.Contains("--no-server") || args.Contains("-n")) Config.StartServer = false;
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    Ui.Log.AddWarning(Resources._PortBusy);
                else
                    throw;
            }

            Ui.SetupNetworkEvents();

            // Show logs in console
            if (args.Contains("--debug") || args.Contains("-d") || Debugger.IsAttached)
            {
                Trace.Listeners.Add(new TerminalTraceListener());
            }

            // Don't check updates in debug mode
            else
            {
                var newVersion = UpdateChecker.CheckUpdates();
                if (newVersion != null) Ui.StatusBar.Text = Ui.StatusBar.Text += $" - Update available ({newVersion})";
            }

            Logger.StartLogging();
            Network.Start();
            Logger.DeleteOldLogs(5);
        }
    }
}