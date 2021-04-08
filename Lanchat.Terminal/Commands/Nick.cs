using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Nick : ICommand
    {
        public string Alias { get; } = "nick";
        public int ArgsCount { get; } = 1;

        public void Execute(string[] args)
        {
            var nickname = args[0].Trim();
            if (nickname.Length >= 20 || string.IsNullOrWhiteSpace(nickname))
            {
                Ui.Log.AddError(Resources._WrongNickname);
            }
            else
            {
                Program.Config.Nickname = nickname;
                Ui.Log.Add(Resources._SelfNicknameChanged);
                Ui.PromptIndicator.Text = $"[{Program.Config.Nickname}] ";
            }
        }
    }
}