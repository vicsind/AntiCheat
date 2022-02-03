using DiscordRPC;
using DiscordRPC.Logging;
using System.Threading.Tasks;

namespace Teos.Discord
{
    public class DiscordSystem
    {
        private readonly DiscordRpcClient _client = new DiscordRpcClient(Global.DiscordAppId);
        public Timestamps StartTimestamp;
        private int _ticks;
        public async void Start()
        {
            try
            {
                _client.Logger = new ConsoleLogger { Level = LogLevel.Warning };
                // Connect to the RPC
                _client.Initialize();

                while (true)
                {
                    await Task.Delay(5000);
                    await Tick();
                    if (++_ticks % 12 == 0)
                    {
                        await DiscordHelper.Check();
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
        private async Task Tick()
        {
            try
            {
                if (Settings.Default.DiscordApi && Helper.IsGameLaunched())
                {
                    string details = "Toon not selected";
                    string state = "In game";

                    int charId = Helper.GetCharId();
                    if (charId != 0)
                    {
                        var userInfo = Helper.GetUserInfo();
                        details = $"{userInfo.CharName} | {userInfo.Kills:N0} kills";
                        state = "In game";
                    }

                    StartTimestamp = StartTimestamp ?? Timestamps.Now;
                    _client.SetPresence(new RichPresence
                    {
                        Details = details,
                        State = state,
                        Assets = new Assets
                        {
                            LargeImageKey = "shaiya-1",
                            LargeImageText = Global.DiscordAppName,
                            //SmallImageKey = "logo"
                        },
                        Timestamps = StartTimestamp
                    });
                }
                else
                {
                    StartTimestamp = null;
                    _client.ClearPresence();
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
