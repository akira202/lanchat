﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Lanchat.Terminal
{
    public class Config
    {
        private static int _BroadcastPort;
        private static int _HeartbeatSendTimeout;
        private static int _HeartbeatReceiveTimeout;
        private static int _ConnectionTimeout;
        private static int _HostPort;
        private static string _Nickname;

        [JsonConstructor]
        public Config(string nickname, int broadcast, int host, List<string> muted, int heartbeatSend, int heartbeatReceive, int connection)
        {
            Nickname = nickname;
            BroadcastPort = broadcast;
            HostPort = host;
            HeartbeatSendTimeout = heartbeatSend;
            HeartbeatReceiveTimeout = heartbeatReceive;
            ConnectionTimeout = connection;
            Muted = muted ?? new List<string>();
        }

        [DefaultValue(4001)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int BroadcastPort
        {
            get => _BroadcastPort;
            set
            {
                _BroadcastPort = value;
                Save();
            }
        }

        [DefaultValue(3000)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int HeartbeatSendTimeout
        {
            get { return _HeartbeatSendTimeout; }
            set
            {
                _HeartbeatSendTimeout = value;
                Save();
            }
        }

        [DefaultValue(5000)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int HeartbeatReceiveTimeout
        {
            get { return _HeartbeatReceiveTimeout; }
            set
            {
                _HeartbeatReceiveTimeout = value;
                Save();
            }
        }

        [DefaultValue(5000)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int ConnectionTimeout
        {
            get { return _ConnectionTimeout; }
            set
            {
                _ConnectionTimeout = value;
                Save();
            }
        }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int HostPort
        {
            get => _HostPort;
            set
            {
                _HostPort = value;
                Save();
            }
        }

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public List<string> Muted { get; private set; }

        [DefaultValue("user")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Nickname
        {
            get => _Nickname;
            set
            {
                _Nickname = value;
                Save();
            }
        }

        public static string Path { get; private set; }

        public static Config Load()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Path = Environment.GetEnvironmentVariable("HOME") + "/.Lancaht2/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Path = Environment.GetEnvironmentVariable("HOME") + "/Library/Preferences/.Lancaht2/";
                }

                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path + "config.json"));
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is DirectoryNotFoundException || e is JsonSerializationException || e is JsonReaderException)
                {
                    Trace.WriteLine($"[APP] Config load error");
                    return JsonConvert.DeserializeObject<Config>("{}");
                }
                throw;
            }
        }

        public void AddMute(IPAddress ip)
        {
            if (ip != null)
            {
                Muted.Add(ip.ToString());
                Save();
            }
        }

        public void RemoveMute(IPAddress ip)
        {
            if (ip != null)
            {
                Muted.Remove(ip.ToString());
                Save();
            }
        }

        private void Save()
        {
            try
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                File.WriteAllText(Path + "config.json", JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception e)
            {
                if (e is DirectoryNotFoundException || e is UnauthorizedAccessException)
                {
                    Trace.WriteLine(e.Message);
                    return;
                }
                throw;
            }
        }
    }
}