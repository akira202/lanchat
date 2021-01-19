using System.Linq;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class PrivateMessage : ICommand
    {
        public string Alias { get; set; } = "m";
        public int ArgsCount { get; set; } = 2;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
            if (node == null)
            {
                Ui.Log.Add(Resources.Info_NotFound);
                return;
            }

            node.NetworkOutput.SendPrivateMessage(string.Join(" ", args.Skip(1)));
        }
    }
}