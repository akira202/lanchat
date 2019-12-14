﻿using Lanchat.Common.HostLib.Types;
using System;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    // Event handlers
    public class HostEventsHandlers
    {
        // Constructor
        public HostEventsHandlers(Network network)
        {
            this.network = network;
        }

        private readonly Network network;

        // Received broadcast
        public void OnReceivedBroadcast(object o, RecievedBroadcastEventArgs e)
        {
            if (IsCanAdd(e.Sender, e.SenderIP))
            {
                // Create new node
                try
                {
                    network.CreateNode(e.Sender.Id, e.Sender.Port, e.SenderIP);
                }
                catch (Exception ex)
                {
                    Trace.Write("Connecting error: " + ex.Message);
                }
            }
        }

        // Node connected
        public void OnNodeConnected(object o, NodeConnectionStatusEvent e)
        {
            // If broadcast isn't already received host will create node when the handshake is received
            Trace.WriteLine("New connection from: " + e.NodeIP.ToString());
        }

        // Node disconnected
        public void OnNodeDisconnected(object o, NodeConnectionStatusEvent e)
        {
            // Find node in list
            var node = network.NodeList.Find(x => x.Ip.Equals(e.NodeIP));
            // If node exist delete it
            if (node != null)
            {
                var nickname = node.ClearNickname;
                // Log disconnect
                Trace.WriteLine(node.Nickname + " disconnected");
                // Emit event
                network.Events.OnNodeDisconnected(node.Ip, node.Nickname);
                // Remove node from list
                network.NodeList.Remove(node);
                // Delete the number if nicknames are not duplicated now
                CheckNickcnameDuplicates(nickname);
            }
            // If node doesn't exist log exception
            else
            {
                Trace.WriteLine("Node does not exist");
            }
        }

        // Recieved handshake
        public void OnReceivedHandshake(object o, RecievedHandshakeEventArgs e)
        {
            Trace.WriteLine("Received handshake");
            Trace.Indent();
            Trace.WriteLine(e.NodeHandshake.Nickname);
            Trace.WriteLine(e.SenderIP);
            Trace.Unindent();

            // If node already crated just accept handhshake
            if (network.NodeList.Exists(x => x.Ip.Equals(e.SenderIP)))
            {
                var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
                user.AcceptHandshake(e.NodeHandshake);
                network.Events.OnNodeConnected(e.SenderIP, e.NodeHandshake.Nickname);
                Trace.WriteLine("Node found and handshake accepted");
            }

            // If list doesn't contain node with this ip create node and accept handshake
            else
            {
                // Create new node
                network.CreateNode(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP);
                Trace.WriteLine("New node created after recieved handshake");

                // Accept handshake
                var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
                user.AcceptHandshake(e.NodeHandshake);
            }

            // Add number to peers with same nicknames
            CheckNickcnameDuplicates(e.NodeHandshake.Nickname);

            // Emit event
            network.Events.OnNodeConnected(e.SenderIP, network.NodeList.Find(x => x.Ip.Equals(e.SenderIP)).Nickname);
        }

        // Receieved symetric key
        public void OnReceivedKey(object o, RecievedKeyEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            user.CreateRemoteAes(network.Rsa.Decode(e.AesKey), network.Rsa.Decode(e.AesIV));
        }

        // Recieved message
        public void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            if (!user.Mute)
            {
                var content = user.RemoteAes.Decode(e.Content);
                Trace.WriteLine(user.Nickname + ": " + content);
                network.Events.OnReceivedMessage(content, user.Nickname);
            }
            else
            {
                Trace.WriteLine($"Message from muted user ({e.SenderIP}) blocked");
            }
        }

        // Changed nickname
        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            var oldNickname = user.Nickname;
            user.Nickname = e.NewNickname;
            // Check is nickname duplicated
            CheckNickcnameDuplicates(e.NewNickname);
            network.Events.OnChangedNickname(oldNickname, e.NewNickname, e.SenderIP);
            // Emit event
            Trace.WriteLine($"{oldNickname} nickname changed to {e.NewNickname}");
        }

        // Check is paperplane come from self or user alredy exist in list
        private bool IsCanAdd(Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != network.Id && !network.NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !network.NodeList.Exists(x => x.Ip.Equals(senderIp));
        }

        // Check nickname duplicates
        private void CheckNickcnameDuplicates(string nickname)
        {
            var users = network.NodeList.FindAll(x => x.ClearNickname == nickname);
            if (users.Count > 1)
            {
                var index = 1;
                foreach (var item in users)
                {
                    item.NicknameNum = index;
                    index++;
                }
            }
            else if (users.Count > 0)
            {
                users[0].NicknameNum = 0;
            }
        }
    }
}