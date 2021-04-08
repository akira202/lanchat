using System;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Lanchat.Terminal
{
    public static class UpdateChecker
    {
        public static string CheckUpdates()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "lanchat-terminal");

            try
            {
                var response = client.GetAsync("https://api.github.com/repos/tofudd/lanchat/releases/latest").Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var lastGithubVersion =
                    new Version(Regex.Match(result, "(?<=\"tag_name\":\")(.*)(?=\",\"target)").Value);
                return Assembly.GetEntryAssembly()?.GetName().Version?.CompareTo(lastGithubVersion) == -1
                    ? lastGithubVersion.ToString()
                    : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}