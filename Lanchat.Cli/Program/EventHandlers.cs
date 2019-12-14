﻿using Lanchat.Cli.Ui;
using Lanchat.Common.NetworkLib;
using System.Diagnostics;

namespace Lanchat.Cli.ProgramLib
{
    public class EventHandlers
    {
        public EventHandlers(Program program)
        {
            this.program = program;
        }

        // Main program reference
        private readonly Program program;

        // Handle input
        public void OnRecievedInput(object o, InputEventArgs e)
        {
            var input = e.Input;

            // Check is input command
            if (input.StartsWith("/"))
            {
                program.Commands.Execute(input.Substring(1));
            }

            // Or message
            else
            {
                Prompt.Out(input, null, Config.Nickname);
                program.Network.Out.SendAll(input);
            }
        }

        // Handle message
        public void OnRecievedMessage(object o, ReceivedMessageEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Out(e.Content, null, e.Nickname);
            }
        }

        // Handle connect
        public void OnNodeConnected(object o, NodeConnectionStatusEvent e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice(e.Nickname + " connected");
            }

            if (Config.Muted.Exists(x => x.Equals(e.NodeIP)))
            {
                var user = program.Network.NodeList.Find(x => x.Ip.Equals(e.NodeIP));
                user.Mute = true;
                Trace.WriteLine($"{e.NodeIP} is muted");
            }
        }

        // Handle disconnect
        public void OnNodeDisconnected(object o, NodeConnectionStatusEvent e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice(e.Nickname + " disconnected");
            }
        }

        // Handle changed nickname
        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            if (!program.DebugMode)
            {
                Prompt.Notice($"{e.OldNickname} changed nickname to {e.NewNickname}");
            }
        }
    }
}