﻿using lanchat.Commands;
using System;
using System.Drawing;
using System.Reflection;
using Console = Colorful.Console;

namespace lanchat.Terminal
{
    public static class Prompt
    {
        // variables
        public static string promptChar = ">";

        // welcome screen
        public static void Welcome()
        {
            string version = GetVersion();
            Console.Title = "Lanchat 2";
            Console.WriteLine("Lanchat " + version);
            Console.WriteLine("Main port: " + lanchat.Properties.User.Default.mport.ToString());
            Console.WriteLine("Broadcast port: " + lanchat.Properties.User.Default.bport.ToString());
            Console.WriteLine("");
        }

        // read from prompt
        public static void Init()
        {
            while (true)
            {
                Console.Write(promptChar + " ");

                // read input
                string promptInput = Console.ReadLine();
                ClearLine();

                if (!string.IsNullOrEmpty(promptInput))
                {
                    // check is input command
                    if (promptInput.StartsWith("/"))
                    {
                        string command = promptInput.Substring(1);
                        Command.Execute(command);
                    }

                    // or message
                    else
                    {
                        Out(promptInput, null, lanchat.Properties.User.Default.nick);
                    }
                }
            }
        }

        public static void Out(string message, Color? color = null, string nickname = null)
        {
            int currentTopCursor = Console.CursorTop;
            int currentLeftCursor = Console.CursorLeft;
            Console.MoveBufferArea(0, currentTopCursor, Console.WindowWidth, 1, 0, currentTopCursor + 1);
            Console.CursorTop = currentTopCursor;
            Console.CursorLeft = 0;
            if (!string.IsNullOrEmpty(nickname))
            {
                Console.Write(DateTime.Now.ToString("HH:mm:ss") + " ", Color.DimGray);
                Console.Write(nickname + " ", Color.SteelBlue);
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine(message, color ?? Color.White);
            }
            Console.CursorTop = currentTopCursor + 1;
            Console.CursorLeft = currentLeftCursor;
        }

        public static void Notice(string message)
        {
            Out("[#] " + message, Color.DodgerBlue);
        }

        public static void Alert(string message)

        {
            Out("[!] " + message, Color.OrangeRed);
        }

        public static string Query(string query)
        {
            Console.Write(query + " ", Color.LightGreen);
            string answer = Console.ReadLine();
            ClearLine();
            return answer;
        }

        private static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
            Console.SetCursorPosition(0, Console.CursorTop++);
        }

        private static string GetVersion()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if DEBUG
            version += " DEBUG MODE";
#endif
#if ALPHA
        version += " Alpha";
#endif
            return version;
        }
    }
}