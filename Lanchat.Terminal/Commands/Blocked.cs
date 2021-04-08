using Lanchat.Core.Extensions;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Blocked : ICommand
    {
        public string Alias { get; } = "blocked";
        public int ArgsCount { get; } = 0;

        public void Execute(string[] _)
        {
            Ui.Log.Add($"{Resources._BlockedList} {Program.Config.SavedAddresses.Count}");
            Program.Config.SavedAddresses.ForEach(x => Ui.Log.Add($"{x}"));
        }
    }
}