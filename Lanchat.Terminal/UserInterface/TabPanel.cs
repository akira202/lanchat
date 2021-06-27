using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface
{
    public class TabPanel : SimpleControl, IInputListener
    {
        private readonly List<IInputListener> inputListeners;
        private readonly DockPanel wrapper;
        private readonly VerticalStackPanel systemTabsPanel;
        private readonly VerticalStackPanel chatTabsPanel;

        public Tab CurrentTab { get; private set; }
        public  List<Tab> Tabs { get; } = new();

        public TabPanel(List<IInputListener> inputListeners)
        {
            this.inputListeners = inputListeners;
            systemTabsPanel = new VerticalStackPanel();
            chatTabsPanel = new VerticalStackPanel();

            wrapper = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Right,
                DockedControl = new Boundary
                {
                    MinWidth = 25,
                    MaxWidth = 25,
                    Content = new DockPanel
                    {
                        DockedControl = new Border
                        {
                            BorderStyle = BorderStyle.Single,
                            Content = systemTabsPanel
                        },
                        FillingControl = new Border
                        {
                            BorderStyle = BorderStyle.Single,
                            Content = chatTabsPanel
                        }
                    }
                }
            };
            Content = wrapper;
        }

        public void AddSystemTab(Tab tab)
        {
            Tabs.Add(tab);
            systemTabsPanel.Add(tab.Header);
            if (Tabs.Count == 1)
                SelectTab(0);
        }

        public void AddChatTab(Tab tab)
        {
            Tabs.Add(tab);
            chatTabsPanel.Add(tab.Header);
            if (Tabs.Count == 1)
                SelectTab(0);
        }

        public void RemoveChatTab(Tab tab)
        {
            if (CurrentTab == tab)
            {
               SelectTab(0); 
            }
            Tabs.Remove(tab);
            chatTabsPanel.Children = chatTabsPanel.Children.Where(x => x != tab.Header).ToList();
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Tab) return;
            SelectTab((Tabs.IndexOf(CurrentTab) + 1) % Tabs.Count);
            inputEvent.Handled = true;
        }

        private void SelectTab(int tab)
        {
            if (CurrentTab != null)
            {
                inputListeners.Remove(CurrentTab.VerticalScrollPanel);
            }

            CurrentTab?.MarkAsInactive();
            CurrentTab = Tabs[tab];
            
            inputListeners.Add(CurrentTab.VerticalScrollPanel);
            CurrentTab.MarkAsActive();
            wrapper.FillingControl = new Border
            {
                BorderStyle = BorderStyle.Single,
                Content = CurrentTab.Content
            };
        }
    }
}