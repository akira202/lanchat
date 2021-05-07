using System.Linq;
using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Block : ICommand
    {
        public string Alias => "block";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            IPAddress ipAddress;

            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Help_block);
                return;
            }

            if (args[0].Length == 4)
            {
                var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
                ipAddress = node?.Host.Endpoint.Address;
                node?.Disconnect();
            }
            else if (IPAddress.TryParse(args[0], out ipAddress))
            {
                var node = Program.Network.Nodes.Find(x => Equals(x.Host.Endpoint.Address, ipAddress));
                node?.Disconnect();
            }
            else
            {
                Ui.Log.AddError(Resources._IncorrectValues);
                return;
            }

            if (Program.Config.BlockedAddresses.Any(x => Equals(x, ipAddress)))
            {
                Ui.Log.AddError(Resources._AlreadyBlocked);
                return;
            }

            Program.Config.BlockedAddresses.Add(ipAddress);
            Ui.Log.Add(string.Format(Resources._Blocked, ipAddress));
        }
    }
}