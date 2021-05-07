using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Lanchat.Core.Config;
using Lanchat.Core.Json;
using Lanchat.Core.Models;
using Lanchat.Core.Udp;

namespace Lanchat.Core.NodesDetection
{
    internal class AnnounceListener
    {
        private readonly IConfig config;
        private readonly ObservableCollection<Announce> detectedNodes;
        private readonly JsonUtils jsonUtils;
        private readonly IUdpClient udpClient;
        private readonly string uniqueId;

        public AnnounceListener(
            IConfig config,
            IUdpClient udpClient,
            string uniqueId,
            ObservableCollection<Announce> detectedNodes)
        {
            this.udpClient = udpClient;
            this.config = config;
            this.uniqueId = uniqueId;
            this.detectedNodes = detectedNodes;
            jsonUtils = new JsonUtils();
        }

        internal void Start()
        {
            udpClient.Bind(new IPEndPoint(IPAddress.Any, config.BroadcastPort));
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
                    { }
                    catch (InvalidOperationException)
                    { }
                }
            });
        }

        private void HandleBroadcast(string json, IPEndPoint from)
        {
            var broadcast = jsonUtils.Deserialize<Announce>(json);
            if (!CheckPreconditions(broadcast))
            {
                return;
            }

            broadcast.IpAddress = from.Address;
            BroadcastReceived(broadcast);
        }

        private bool CheckPreconditions(Announce broadcast)
        {
            if (Validator.TryValidateObject(broadcast,
                new ValidationContext(broadcast),
                new List<ValidationResult>()))
            {
                return broadcast.Guid != uniqueId;
            }

            return false;
        }

        private void BroadcastReceived(Announce e)
        {
            var alreadyDetected = detectedNodes.FirstOrDefault(x => Equals(x.IpAddress, e.IpAddress));
            if (alreadyDetected == null)
            {
                AddNewNode(e);
            }
            else
            {
                UpdateNode(e, alreadyDetected);
            }
        }

        private static void UpdateNode(Announce newAnnounce, Announce alreadyDetected)
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