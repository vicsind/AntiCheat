using System;
using System.Diagnostics;

namespace Teos
{
    public static class Global
    {
        public const string WindowName = "WONDER";
        public const string ServerName = "Shaiya Wonder";
        public const string DiscordAppId = "969731263529353216"; // Wonder
        public const string DiscordAppName = "Shaiya Wonder";
        
        public static string ExecutableLocation => Process.GetCurrentProcess().MainModule?.FileName ??
                                                   throw new InvalidOperationException("Executable location is null");
        public static string SiteUrl => "https://api.shaiyawonder.com/";
    }
}
