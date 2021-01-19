using System;

namespace Lanchat.Terminal.Commands
{
    public class Exit : ICommand
    {
        public string Alias { get; set; } = "exit";
        public int ArgsCount { get; set; }

        public void Execute(string[] _)
        {
            Console.ResetColor();
            Console.Clear();
            Environment.Exit(0);
        }
    }
}