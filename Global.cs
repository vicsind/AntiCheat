using System;
using System.Diagnostics;

namespace Teos
{
    public static class Global
    {
        public const string WindowName = "BLESS";
        public const string ServerName = "Shaiya Bless";
        public const string DiscordAppId = "979453618644217876"; // Bless
        public const string DiscordAppName = "Shaiya Bless";
        
        public static string ExecutableLocation => Process.GetCurrentProcess().MainModule?.FileName ??
                                                   throw new InvalidOperationException("Executable location is null");
        public static string SiteUrl => "https://api.shaiyabless.com/";
    }
}
