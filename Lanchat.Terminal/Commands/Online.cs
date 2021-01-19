using Lanchat.Core;
using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Online : ICommand
    {
        public string Alias { get; set; } = "online";
        public int ArgsCount { get; set; }

        public void Execute(string[] _)
        {
            CoreConfig.Status = Status.Online;
            Ui.Status.Text = "Online";
        }
    }
}