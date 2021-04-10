using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Online : ICommand
    {
        public string Alias => "online";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            Program.Config.Status = Status.Online;
            Ui.BottomBar.Status.Text = "Online";
        }
    }
}