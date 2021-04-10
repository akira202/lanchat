using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using static System.Int32;

namespace Lanchat.Terminal.Commands
{
    public class Connect : ICommand
    {
        public string Alias => "connect";
        public int ArgsCount => 1;

        public async void Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Help_connect);
                return;
            }

            var addressArgument = args[0].Trim();
            try
            {
                // If input cannot be parsed as IP try get address from dns
                if (!IPAddress.TryParse(addressArgument, out var ipAddress))
                    ipAddress = (await Dns.GetHostAddressesAsync(addressArgument)).FirstOrDefault();

                // Use port from argument or config
                var port = 0;
                if (args.Length > 1) port = Parse(args[1]);
                if (port == 0) port = Program.Config.ServerPort;
                Ui.Log.Add(string.Format(Resources._ConnectionAttempt, addressArgument));
                var result = await Program.Network.Connect(ipAddress, port);
                if (!result) Ui.Log.AddError(string.Format(Resources._CannotConnect, ipAddress));
            }
            catch (FormatException)
            {
                Ui.Log.AddError(Resources._IncorrectValues);
            }
            catch (SocketException)
            {
                Ui.Log.AddError(Resources._IncorrectValues);
            }
            catch (ArgumentException e)
            {
                Ui.Log.AddError(e.Message);
            }
        }
    }
}