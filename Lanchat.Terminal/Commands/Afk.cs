using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Afk : ICommand
    {
        public string Alias { get; } = "afk";
        public int ArgsCount { get; } = 0;

        public void Execute(string[] _)
        {
            Program.Config.Status = Status.AwayFromKeyboard;
            Ui.BottomBar.Status.Text = "AFK";
        }
    }
}