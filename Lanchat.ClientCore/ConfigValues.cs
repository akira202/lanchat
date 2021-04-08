// ReSharper disable StringLiteralTypo

using System;
using System.Runtime.InteropServices;
using Syroot.Windows.IO;

namespace Lanchat.ClientCore
{
    internal static class ConfigValues
    {
        private static readonly string[] Nicknames =
        {
            "Reimu",
            "Marisa",
            "Alice",
            "Patchouli",
            "Cirno",
            "Sakuya",
            "Flandre",
            "Youmu",
            "Ran",
            "Yukari",
            "Reisen",
            "Kaguya",
            "Mokou",
            "Eirin",
            "Sanae",
            "Suwako",
            "Utsuho",
            "Koishi",
            "Byakuren",
            "Seiga"
        };

        internal static string GetDownloadsDirectory()
        {
            var path = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

            if (path != null) return path;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                path = new KnownFolder(KnownFolderType.Downloads).Path;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                path = Environment.GetEnvironmentVariable("HOME") + "/Downloads";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                path = Environment.GetEnvironmentVariable("HOME") + "/Downloads";

            return path;
        }

        internal static string GetNickname()
        {
            var random = new Random();
            return Nicknames[random.Next(0, Nicknames.Length)];
        }
    }
}