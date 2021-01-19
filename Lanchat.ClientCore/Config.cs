using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using Lanchat.Core;

namespace Lanchat.ClientCore
{
    public class Config
    {
        private static int _port = 3645;
        private static int _broadcastPort = 3646;
        private static string _nickname = NicknamesGenerator.GimmeNickname();
        private static List<string> _blockedAddresses = new();
        private static bool _automaticConnecting = true;

        public List<string> BlockedAddresses
        {
            get => _blockedAddresses;
            set
            {
                _blockedAddresses = value;
                CoreConfig.BlockedAddresses = _blockedAddresses.Select(IPAddress.Parse).ToList();
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                CoreConfig.ServerPort = value;
                Save();
            }
        }

        public int BroadcastPort
        {
            get => _broadcastPort;
            set
            {
                _broadcastPort = value;
                CoreConfig.BroadcastPort = value;
                Save();
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                CoreConfig.Nickname = value;
                Save();
            }
        }

        public bool AutomaticConnecting
        {
            get => _automaticConnecting;
            set
            {
                _automaticConnecting = value;
                CoreConfig.AutomaticConnecting = value;
                Save();
            }
        }

        public static string ConfigPath { get; private set; }
        public static string DataPath { get; private set; }

        public void AddBlocked(IPAddress ipAddress)
        {
            var ipString = ipAddress.MapToIPv6().ToString();
            if (BlockedAddresses.Contains(ipString)) return;
            BlockedAddresses.Add(ipString);
            CoreConfig.BlockedAddresses.Add(ipAddress);
            Save();
        }

        public void RemoveBlocked(IPAddress ipAddress)
        {
            BlockedAddresses.Remove(ipAddress.MapToIPv6().ToString());
            CoreConfig.BlockedAddresses.Remove(ipAddress);
            Save();
        }

        public static Config Load()
        {
            Config config;
            try
            {
                var xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                var xdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

                if (xdgDataHome != null && xdgConfigHome != null)
                {
                    DataPath = xdgDataHome;
                    ConfigPath = $"{xdgConfigHome}/config.json";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2";
                    ConfigPath = $"{DataPath}/config.json";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    DataPath = Environment.GetEnvironmentVariable("HOME") + "/.Lancaht2";
                    ConfigPath = $"{DataPath}/config.json";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    DataPath = Environment.GetEnvironmentVariable("HOME") + "/Library/Preferences/.Lancaht2";
                    ConfigPath = $"{DataPath}/config.json";
                }

                config = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigPath));
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException) && !(e is DirectoryNotFoundException) && !(e is JsonException)) throw;
                config = new Config();
                config.Save();
            }

            return config;
        }

        private void Save()
        {
            try
            {
                var configFileDirectory = Path.GetDirectoryName(ConfigPath);
                if (!Directory.Exists(configFileDirectory)) Directory.CreateDirectory(configFileDirectory!);
                File.WriteAllText(ConfigPath,
                    JsonSerializer.Serialize(this, new JsonSerializerOptions {WriteIndented = true}));
            }
            catch (Exception e)
            {
                if (!(e is DirectoryNotFoundException) && !(e is UnauthorizedAccessException)) throw;
            }
        }
    }
}