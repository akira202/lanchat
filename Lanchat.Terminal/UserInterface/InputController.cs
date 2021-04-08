using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Lanchat.Terminal.Commands;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface
{
    public class InputController : IInputListener
    {
        private readonly IList<ICommand> commands = new List<ICommand>();
        private readonly TextBox input;

        public InputController(TextBox input)
        {
            commands.Add(new Afk());
            commands.Add(new Block());
            commands.Add(new Blocked());
            commands.Add(new Connect());
            commands.Add(new Disconnect());
            commands.Add(new Dnd());
            commands.Add(new Exit());
            commands.Add(new Help());
            commands.Add(new List());
            commands.Add(new Nick());
            commands.Add(new Online());
            commands.Add(new PrivateMessage());
            commands.Add(new Unblock());
            commands.Add(new SendFile());
            commands.Add(new Accept());
            commands.Add(new Reject());
            commands.Add(new Cancel());
            
            this.input = input;
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Enter) return;

            if (!string.IsNullOrWhiteSpace(input.Text))
            {
                if (input.Text.StartsWith("/", StringComparison.CurrentCulture))
                {
                    ExecuteCommand(input.Text.Split(' '));
                }
                else
                {
                    Ui.Log.AddMessage(input.Text, Program.Config.Nickname, false);
                    Program.Network.Broadcasting.SendMessage(input.Text);
                }
            }

            input.Text = string.Empty;
            inputEvent.Handled = true;
        }

        private void ExecuteCommand(string[] args)
        {
            var commandAlias = args[0].Substring(1);
            args = args.Skip(1).ToArray();
            var command = commands.FirstOrDefault(x => x.Alias == commandAlias);

            if (command == null)
            {
                Ui.Log.AddError(Resources._InvalidCommand);
                return;
            }
            
            if (args.Length < command.ArgsCount)
            {
                var help = Resources.ResourceManager.GetString($"Help_{commandAlias}", CultureInfo.CurrentCulture);
                if (help != null) Ui.Log.Add(help);
                return;
            }

            command.Execute(args);
        }
    }
}