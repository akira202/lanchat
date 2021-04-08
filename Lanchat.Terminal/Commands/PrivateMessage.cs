using System.Linq;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class PrivateMessage : ICommand
    {
        public string Alias { get; } = "m";
        public int ArgsCount { get; } = 2;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
            if (node == null)
            {
                Ui.Log.AddError(Resources._UserNotFound);
                return;
            }

            node.Messaging.SendPrivateMessage(string.Join(" ", args.Skip(1)));
        }
    }
}