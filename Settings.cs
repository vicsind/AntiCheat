using Newtonsoft.Json;
using System;
using System.IO;

namespace Teos
{
    public class Settings
    {
        private static string AppConfigFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Global.ServerName);
        public static string AppConfigFile => Path.Combine(AppConfigFolder, "teos.json");

        public static Settings Default
        {
            get
            {
                if (_default == null)
                    Load();
                return _default;
            }
        }
        private static Settings _default;

        public static void Load()
        {
            try
            {
                string fileContent = File.ReadAllText(AppConfigFile);
                _default = JsonConvert.DeserializeObject<Settings>(fileContent);
            }
            catch
            {
                _default = new Settings();
            }
        }

        public void Save()
        {
            try
            {
                string fileContent = JsonConvert.SerializeObject(this, Formatting.Indented);
                if (!Directory.Exists(AppConfigFolder))
                    Directory.CreateDirectory(AppConfigFolder);
                File.WriteAllText(AppConfigFile, fileContent);
            }
            catch
            {
                // ignored
            }
        }
        
        public bool DiscordApi { get; set; } = true;
    }
}
