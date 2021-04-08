using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Dnd : ICommand
    {
        public string Alias { get; } = "dnd";
        public int ArgsCount { get; } = 0;

        public void Execute(string[] _)
        {
            Program.Config.Status = Status.DoNotDisturb;
            Ui.BottomBar.Status.Text = "DND";
        }
    }
}