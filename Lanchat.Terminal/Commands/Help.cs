using System.Globalization;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Help : ICommand
    {
        public string Alias { get; } = "help";
        public int ArgsCount { get; } = 0;

        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                Ui.Log.Add(Resources.Help);
            }
            else
            {
                var commandHelp = Resources.ResourceManager.GetString($"Help_{args[0]}", CultureInfo.CurrentCulture);
                Ui.Log.AddError(commandHelp ?? Resources._ManualNotFound);
            }
        }
    }
}