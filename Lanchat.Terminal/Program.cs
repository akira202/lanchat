using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public static class Program
    {
        public static IP2P Network { get; private set; }
        public static Config Config { get; private set; }

        private static void Main(string[] args)
        {
            Config = Storage.LoadConfig();

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(Config.Language);
            }
            catch
            {
                Trace.WriteLine("Cannot load translation. Using default.");
            }

            Resources.Culture = CultureInfo.CurrentCulture;

            Ui.Start();
            Network = new P2P(Config);
            Network.NodeCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };
            Ui.SetupNetworkEvents();

            if (args.Contains("--no-saved") || args.Contains("-a"))
            {
                Config.ConnectToSaved = false;
            }

            if (args.Contains("--no-udp") || args.Contains("-b"))
            {
                Config.NodesDetection = false;
            }

            if (args.Contains("--no-server") || args.Contains("-n"))
            {
                Config.StartServer = false;
            }

            if (args.Contains("--debug") || args.Contains("-d") || Debugger.IsAttached)
            {
                Trace.Listeners.Add(new TerminalTraceListener());
            }
            else
            {
                var newVersion = UpdateChecker.CheckUpdates();
                if (newVersion != null)
                {
                    Ui.StatusBar.Text = Ui.StatusBar.Text += $" - Update available ({newVersion})";
                }
            }

            Logger.StartLogging();

            try
            {
                Network.Start();
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode != SocketError.AddressAlreadyInUse)
                {
                    throw;
                }

                Ui.Log.AddWarning(Resources._PortBusy);
            }

            Logger.DeleteOldLogs(5);
        }
    }
}