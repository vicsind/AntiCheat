using System;
using System.Diagnostics;

namespace Teos
{
    public static class Global
    {
        public const string WindowName = "Shaiya";
        public const string ServerName = "Sanctuary Shaiya";
        public const string DiscordAppId = "920500122360627240"; // Sanctuary
        public const string DiscordAppName = "Sanctuary Shaiya";
        
        public static string ExecutableLocation => Process.GetCurrentProcess().MainModule?.FileName ??
                                                   throw new InvalidOperationException("Executable location is null");
        public static string SiteUrl => "https://api.shaiya-sanctuary.com/";
    }
}
