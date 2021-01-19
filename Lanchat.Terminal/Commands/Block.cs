using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Block : ICommand
    {
        public string Alias { get; set; } = "block";
        public int ArgsCount { get; set; } = 1;

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
                ipAddress = node?.Endpoint.Address;
                node?.Disconnect();
            }
            else if (IPAddress.TryParse(args[0], out ipAddress))
            {
                var node = Program.Network.Nodes.Find(x => Equals(x.Endpoint.Address, ipAddress));
                node?.Disconnect();
            }
            else
            {
                Ui.Log.Add(Resources.Info_IncorrectValues);
                return;
            }

            Program.Config.AddBlocked(ipAddress);
            Ui.Log.Add($"{ipAddress} {Resources.Info_Blocked}");
        }
    }
}