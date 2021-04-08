using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Disconnect : ICommand
    {
        public string Alias { get; } = "disconnect";
        public int ArgsCount { get; } = 1;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
            if (node != null)
                node.Disconnect();
            else
                Ui.Log.AddError(Resources._UserNotFound);
        }
    }
}