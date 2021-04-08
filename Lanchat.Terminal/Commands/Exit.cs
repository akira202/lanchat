using System;

namespace Lanchat.Terminal.Commands
{
    public class Exit : ICommand
    {
        public string Alias { get; } = "exit";
        public int ArgsCount { get; } = 0;

        public void Execute(string[] _)
        {
            Console.ResetColor();
            Console.Clear();
            Environment.Exit(0);
        }
    }
}