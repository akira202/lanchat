using System;
using System.Linq;
using ConsoleGUI.Controls;
using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class List : ICommand
    {
        public string Alias { get; } = "list";
        public int ArgsCount { get; } = 0;

        public void Execute(string[] _)
        {
            Program.Network.Nodes.ForEach(x =>
            {
                var status = new TextBlock();

                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (x.Status)
                {
                    case Status.Online:
                        status.Text = "Online";
                        status.Color = ConsoleColor.Green;
                        break;

                    case Status.AwayFromKeyboard:
                        status.Text = "Afk";
                        status.Color = ConsoleColor.Yellow;
                        break;

                    case Status.DoNotDisturb:
                        status.Text = "Dnd";
                        status.Color = ConsoleColor.Red;
                        break;
                }

                var line = new[]
                {
                    new TextBlock {Text = $"{x.Nickname} (", Color = ConsoleColor.White},
                    status,
                    new TextBlock {Text = ")", Color = ConsoleColor.White}
                };

                Ui.Log.AddTextLine(line);
            });

            Program.Network.NodesDetection.DetectedNodes.ToList().ForEach(x =>
            {
                var line = new[]
                {
                    new TextBlock {Text = $"{x.Nickname} - {x.IpAddress}(", Color = ConsoleColor.White},
                    new TextBlock {Text = "LAN", Color = ConsoleColor.DarkCyan},
                    new TextBlock {Text = ")", Color = ConsoleColor.White}
                };

                Ui.Log.AddTextLine(line);
            });
        }
    }
}