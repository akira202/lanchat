using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Afk : ICommand
    {
        public string Alias => "afk";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            Program.Config.Status = Status.AwayFromKeyboard;
            Ui.BottomBar.Status.Text = "AFK";
        }
    }
}