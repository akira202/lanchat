using System;
using System.Reflection;
using System.Threading;
using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using Lanchat.ClientCore;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface
{
    public static class Ui
    {
        private static TextBox _input;
        internal static BottomBar BottomBar { get; private set; }
        internal static LogPanel Log { get; private set; }
        internal static TextBlock PromptIndicator { get; private set; }
        internal static VerticalScrollPanel ScrollPanel { get; private set; }
        internal static TextBlock StatusBar { get; private set; }
        internal static FileTransferMonitor FileTransferMonitor { get; private set; }

        public static void Start()
        {
            Log = new LogPanel();
            FileTransferMonitor = new FileTransferMonitor();
            BottomBar = new BottomBar();

            _input = new TextBox();

            PromptIndicator = new TextBlock
            {
                Text = $"[{Program.Config.Nickname}] "
            };

            ScrollPanel = new VerticalScrollPanel
            {
                Content = Log,
                ScrollBarBackground = new Character(),
                ScrollBarForeground = new Character()
            };


            var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
            version = version?.Remove(version.Length - 2);

            StatusBar = new TextBlock
            {
                Text = $"Lanchat {version}"
            };

            var dockPanel = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Bottom,
                FillingControl = new DockPanel
                {
                    Placement = DockPanel.DockedControlPlacement.Top,

                    // Top bar
                    DockedControl = new Boundary
                    {
                        MaxHeight = 1,
                        Content = new Background
                        {
                            Color = ConsoleColor.DarkBlue,
                            Content = StatusBar
                        }
                    },

                    // Log
                    FillingControl = ScrollPanel
                },

                DockedControl = new DockPanel
                {
                    Placement = DockPanel.DockedControlPlacement.Bottom,

                    // Prompt
                    DockedControl = new Boundary
                    {
                        MinHeight = 1,
                        MaxHeight = 1,
                        Content = new HorizontalStackPanel
                        {
                            Children = new IControl[]
                            {
                                new Style
                                {
                                    Content = PromptIndicator
                                },
                                _input
                            }
                        }
                    },

                    // Bottom bar
                    FillingControl = BottomBar
                }
            };

            // Start console UI 
            ConsoleManager.Console = new SimplifiedConsole();
            ConsoleManager.Setup();
            ConsoleManager.Resize(new Size(100, 30));
            ConsoleManager.Content = dockPanel;
            Console.Title = Resources._WindowTitle;
            Log.Add(Resources._Logo);

            if (Program.Config.Fresh) Log.Add(string.Format(Resources._FirstRunMessage, ConfigManager.ConfigPath));

            // Main UI loop
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    BottomBar.Clock.Text = DateTime.Now.ToString("HH:mm");
                    ConsoleManager.ReadInput(new IInputListener[]
                    {
                        new InputController(_input),
                        ScrollPanel,
                        _input
                    });
                    ConsoleManager.AdjustBufferSize();
                }

                // ReSharper disable once FunctionNeverReturns
            }).Start();
        }

        public static void SetupNetworkEvents()
        {
            BottomBar.SetupEvents();
        }
    }
}