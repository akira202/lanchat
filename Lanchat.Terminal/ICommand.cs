namespace Lanchat.Terminal
{
    public interface ICommand
    {
        public string Alias { get; }
        public int ArgsCount { get; }
        public void Execute(string[] args);
    }
}