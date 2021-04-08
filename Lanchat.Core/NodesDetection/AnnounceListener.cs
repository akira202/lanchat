using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodesDetection
{
    internal class AnnounceListener
    {
        private readonly IConfig config;
        private readonly ObservableCollection<Announce> detectedNodes;
        private readonly JsonReader jsonReader;
        private readonly UdpClient udpClient;
        private readonly string uniqueId;

        public AnnounceListener(
            IConfig config,
            UdpClient udpClient,
            string uniqueId,
            ObservableCollection<Announce> detectedNodes)
        {
            this.udpClient = udpClient;
            this.config = config;
            this.uniqueId = uniqueId;
            this.detectedNodes = detectedNodes;
            jsonReader = new JsonReader();
        }

        internal void Start()
        {
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, config.BroadcastPort));
            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    try
                    {
                        HandleBroadcast(recvBuffer, from);
                    }
                    catch (JsonException)
                    {
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }
            });
        }

        private void HandleBroadcast(byte[] recvBuffer, IPEndPoint from)
        {
            var broadcast = jsonReader.Deserialize<Announce>(Encoding.UTF8.GetString(recvBuffer));
            if (!CheckPreconditions(broadcast)) return;
            broadcast.IpAddress = from.Address;
            BroadcastReceived(broadcast);
        }

        private bool CheckPreconditions(Announce broadcast)
        {
            if (!Validator.TryValidateObject(broadcast, new ValidationContext(broadcast), new List<ValidationResult>()))
            {
                return false;
            }

            return broadcast.Guid != uniqueId;
        }

        private void BroadcastReceived(Announce e)
        {
            var alreadyDetected = detectedNodes.FirstOrDefault(x => Equals(x.IpAddress, e.IpAddress));
            if (alreadyDetected == null)
                AddNewNode(e);
            else
                UpdateNode(e, alreadyDetected);
        }

        private void UpdateNode(Announce newAnnounce, Announce alreadyDetected)
        {
            alreadyDetected.Active = true;
            alreadyDetected.Nickname = newAnnounce.Nickname;
        }

        private void AddNewNode(Announce newAnnounce)
        {
            detectedNodes.Add(newAnnounce);
            newAnnounce.Active = true;
            SetupTimer(newAnnounce);
        }

        private void SetupTimer(Announce newAnnounce)
        {
            var timer = new Timer
            {
                Interval = 2500,
                Enabled = true
            };

            timer.Elapsed += (_, _) =>
            {
                if (newAnnounce.Active)
                {
                    newAnnounce.Active = false;
                    return;
                }

                timer.Dispose();
                detectedNodes.Remove(newAnnounce);
            };
        }
    }
}