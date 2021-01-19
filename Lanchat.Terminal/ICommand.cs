namespace Lanchat.Terminal
{
    public interface ICommand
    {
        public string Alias { get; set; }
        public int ArgsCount { get; set; }
        public void Execute(string[] args);
    }
}