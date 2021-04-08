using System;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Reject : ICommand
    {
        public string Alias { get; } = "reject";
        public int ArgsCount { get; } = 1;

        public void Execute(string[] args)
        {
            var node = Program.Network.Nodes.Find(x => x.ShortId == args[0]);
            if (node == null)
            {
                Ui.Log.AddError(Resources._UserNotFound);
                return;
            }

            try
            {
                node.FileReceiver.RejectRequest();
            }
            catch (InvalidOperationException)
            {
                Ui.Log.AddError(Resources._NoFileReceiveRequest);
            }
        }
    }
}