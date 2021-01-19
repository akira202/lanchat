using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Nick : ICommand
    {
        public string Alias { get; set; } = "nick";
        public int ArgsCount { get; set; } = 1;

        public void Execute(string[] args)
        {
            var nickname = args[0].Trim();
            if (nickname.Length >= 20 || string.IsNullOrWhiteSpace(nickname))
            {
                Ui.Log.Add(Resources.Info_WrongNickname);
            }
            else
            {
                Program.Config.Nickname = nickname;
                Ui.Log.Add(Resources.Info_SelfNicknameChanged);
                Ui.PromptIndicator.Text = $"[{Program.Config.Nickname}] ";
            }
        }
    }
}