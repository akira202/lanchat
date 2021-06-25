using System;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Space;

namespace Lanchat.Terminal.UserInterface
{
    public class Tab
    {
        private Background headerBackground;
        public IControl Header { get; private set; }
        public IControl Content { get; set; }

        public Tab(string name, IControl content)
        {
            UpdateHeader(name);
            Content = content;
            MarkAsInactive();
        }
        
        public Tab(string name)
        {
            UpdateHeader(name);
            MarkAsInactive();
        }

        public void MarkAsActive() => headerBackground.Color = ConsoleColor.Blue;
        public void MarkAsInactive() => headerBackground.Color = ConsoleColor.DarkBlue;
        
        public void UpdateHeader(string name)
        {
            headerBackground = new Background
            {
                Content = new Margin
                {
                    Offset = new Offset(1, 0, 1, 0),
                    Content = new TextBlock {Text = name}
                }
            };

            Header = headerBackground;
        }
    }
}