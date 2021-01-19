using Lanchat.Core;
using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Dnd : ICommand
    {
        public string Alias { get; set; } = "dnd";
        public int ArgsCount { get; set; }

        public void Execute(string[] _)
        {
            CoreConfig.Status = Status.DoNotDisturb;
            Ui.Status.Text = "Do not disturb";
        }
    }
}