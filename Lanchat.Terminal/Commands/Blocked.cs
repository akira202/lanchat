using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Blocked : ICommand
    {
        public string Alias { get; set; } = "blocked";
        public int ArgsCount { get; set; }

        public void Execute(string[] _)
        {
            Ui.Log.Add($"{Resources.Info_BlockedList} {Program.Config.BlockedAddresses.Count}");
            Program.Config.BlockedAddresses.ForEach(x => Ui.Log.Add($"{x}"));
        }
    }
}