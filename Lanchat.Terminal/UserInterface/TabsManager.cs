using System.Linq;
using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.UserInterface
{
    public class TabsManager
    {
        private readonly TabPanel tabPanel;
        public ChatView MainChatView { get; }
        public TabsManager(TabPanel tabPanel)
        {
            this.tabPanel = tabPanel;
            MainChatView = new ChatView(true);
            
            tabPanel.AddSystemTab(new Tab("Lanchat", new HomeView()));
            tabPanel.AddSystemTab(new Tab("Detected users", new DetectedUsersView()));
            tabPanel.AddSystemTab(new Tab("File transfer", new FileTransfersView()));
            var mainChatTab = new Tab("main", MainChatView);
            tabPanel.AddChatTab(mainChatTab);
        }

        public ChatView AddPrivateChatView(INode node)
        {
            var chatView = new ChatView(false, node);
            var chatTab = new Tab(node.User.Nickname, chatView) {Id = node.Id};
            tabPanel.AddChatTab(chatTab);
            return chatView;
        }

        public void ClosePrivateChatView(INode node)
        {
            var chatTab = tabPanel.Tabs.FirstOrDefault(x => x.Id == node.Id);
            tabPanel.RemoveChatTab(chatTab);
        }

        public DebugView AddDebugView()
        {
            var debugView = new DebugView();
            var debugTab = new Tab("Debug", debugView);
            tabPanel.AddSystemTab(debugTab);
            return debugView;
        }
    }
}