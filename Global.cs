using System;
using System.Diagnostics;

namespace Teos
{
    public static class Global
    {
        public const string WindowName = "INFERNO";
        public const string ServerName = "Shaiya Inferno";
        public const string DiscordAppId = "956883709259317278"; // Inferno
        public const string DiscordAppName = "Shaiya Inferno";
        
        public static string ExecutableLocation => Process.GetCurrentProcess().MainModule?.FileName ??
                                                   throw new InvalidOperationException("Executable location is null");
        public static string SiteUrl => "https://api.shaiya-inferno.com/";
    }
}
