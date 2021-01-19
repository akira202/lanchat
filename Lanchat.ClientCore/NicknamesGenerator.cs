using System;

namespace Lanchat.ClientCore
{
    public static class NicknamesGenerator
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

        public static string GimmeNickname()
        {
            var random = new Random();
            return Nicknames[random.Next(0, Nicknames.Length)];
        }
    }
}