using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface
{
    public class LogPanel : SimpleControl
    {
        private readonly object lockUi = new();
        private readonly VerticalStackPanel stackPanel;

        public LogPanel()
        {
            stackPanel = new VerticalStackPanel();
            Content = stackPanel;
        }

        public void Add(string text)
        {
            lock (lockUi)
            {
                foreach (var line in Prepare(text))
                {
                    stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                            {
                                new TextBlock {Text = $"{DateTime.Now:HH:mm} "},
                                new TextBlock {Text = "-", Color = ConsoleColor.Blue},
                                new TextBlock {Text = "!"},
                                new TextBlock {Text = "- ", Color = ConsoleColor.Blue},
                                new TextBlock {Text = line}
                            }
                        }
                    });
                    Ui.ScrollPanel.Top = int.MaxValue;
                }
            }
        }

        public void AddMessage(string text, string nickname = null)
        {
            lock (lockUi)
            {
                foreach (var line in Prepare(text))
                {
                    stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                            {
                                new TextBlock {Text = $"{DateTime.Now:HH:mm} "},
                                new TextBlock {Text = "<", Color = ConsoleColor.DarkGray},
                                new TextBlock {Text = $"{nickname}"},
                                new TextBlock {Text = "> ", Color = ConsoleColor.DarkGray},
                                new TextBlock {Text = line}
                            }
                        }
                    });
                    Ui.ScrollPanel.Top = int.MaxValue;
                }
            }
        }

        public void AddPrivateMessage(string text, string nickname = null)
        {
            lock (lockUi)
            {
                foreach (var line in Prepare(text))
                {
                    stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                            {
                                new TextBlock {Text = $"{DateTime.Now:HH:mm} "},
                                new TextBlock {Text = "<", Color = ConsoleColor.DarkGray},
                                new TextBlock {Text = $"{nickname}", Color = ConsoleColor.Magenta},
                                new TextBlock {Text = "> ", Color = ConsoleColor.DarkGray},
                                new TextBlock {Text = line}
                            }
                        }
                    });
                    Ui.ScrollPanel.Top = int.MaxValue;
                }
            }
        }

        public void AddCustomTextBlock(IEnumerable<TextBlock> line)
        {
            var children = new[]
            {
                new TextBlock {Text = $"{DateTime.Now:HH:mm} "},
                new TextBlock {Text = "-", Color = ConsoleColor.Blue},
                new TextBlock {Text = "!"},
                new TextBlock {Text = "- ", Color = ConsoleColor.Blue}
            };


            lock (lockUi)
            {
                stackPanel.Add(new WrapPanel
                {
                    Content = new HorizontalStackPanel
                    {
                        Children = children.Concat(line)
                    }
                });
                Ui.ScrollPanel.Top = int.MaxValue;
            }
        }

        private static IEnumerable<string> Prepare(string text)
        {
            if (text == null) return new[] {""};

            return text.Split(
                new[] {"\r\n", "\r", "\n"},
                StringSplitOptions.None
            );
        }
    }
}