using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace Teos.Discord
{
    public static class DiscordHelper
    {
        public static async Task Check()
        {
            try
            {
                var gameProcess = Helper.GetGameProcess();
                if (gameProcess != null)
                {
                    string filename = gameProcess.MainModule?.FileName;
                    var processes = Process.GetProcesses();
                    var pl = processes
                        .Where(IsValid)
                        .Select(x => new PInfo
                        {
                            Name = x.ProcessName,
                            Window = x.MainWindowTitle,
                            Path = x.MainModule?.FileName
                        })
                        .ToArray();

                    var request = CreateRequest(CheckUrl);
                    using (var stream = request.GetRequestStream())
                    using (var streamWriter = new StreamWriter(stream))
                    {
                        var errorData = new Data
                        {
                            CharID = Helper.GetCharId(),
                            List = pl,
                            PathToExecutable = Global.ExecutableLocation,
                            MD5 = CalculateMd5(filename),
                            PName = filename,
                            IsOS64 = Environment.Is64BitOperatingSystem,
                            IsProcess64 = Environment.Is64BitProcess
                        };
                        var json = JsonConvert.SerializeObject(errorData);
                        await streamWriter.WriteAsync(json);
                    }

                    using (var rp = request.GetResponse())
                    using (var rps = rp.GetResponseStream())
                    {
                        var rd = new StreamReader(rps);
                        string rfs = await rd.ReadToEndAsync();
                        if (bool.TryParse(rfs, out bool isDet) && isDet)
                            Application.Current.Shutdown();
                    }
                }
                else
                {
                    // Close current application because client is not running.
                    Application.Current.Shutdown();
                }
            }
            catch
            {
                // ignored
            }
        }

        private static HttpWebRequest CreateRequest(string s)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(s);
            request.Method = "POST";
            request.ContentType = "application/json";
            return request;
        }

        private static bool IsValid(Process process)
        {
            try
            {
                return !string.IsNullOrWhiteSpace(process.MainWindowTitle) && process.MainModule != null;
            }
            catch
            {
                return false;
            }
        }

        private static string CalculateMd5(string filename)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        var hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        
        public sealed class PInfo
        {
            public string Name { get; set; }
            public string Window { get; set; }
            public string Path { get; set; }
        }

        public sealed class Data
        {
            public int CharID { get; set; }
            public PInfo[] List { get; set; }
            public string MD5 { get; set; }
            public string PName { get; set; }
            public string PathToExecutable { get; set; }
            public bool IsOS64 { get; set; }
            public bool IsProcess64 { get; set; }
        }

        private static string CheckUrl => Global.SiteUrl + "check";
    }
}
