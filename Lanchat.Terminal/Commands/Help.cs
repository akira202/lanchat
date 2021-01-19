using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Help : ICommand
    {
        public string Alias { get; set; } = "help";
        public int ArgsCount { get; set; }

        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                Ui.Log.Add(Resources.Help);
            }
            else
            {
                var commandHelp = Resources.ResourceManager.GetString($"Help_{args[0]}");
                Ui.Log.Add(commandHelp ?? Resources.Info_ManualNotFound);
            }
        }
    }
}