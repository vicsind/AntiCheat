using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Teos
{
    public static class Helper
    {
        public static int GetCharId()
        {
            try
            {
                var process = GetGameProcess();
                if (process == null)
                    return 0;

                int value = 0;
                ReadProcessMemory(process.Handle, CharIdAddress, ref value, sizeof(int), 0);
                return value;
            }
            catch
            {
                return 0;
            }
        }

        public static UserInfo GetUserInfo()
        {
            try
            {
                var process = GetGameProcess();
                if (process == null)
                    return new UserInfo();

                byte[] nameBytes = new byte[20];
                ReadProcessMemory(process.Handle, CharNameAddress, nameBytes, nameBytes.Length, out _);
                string charName = Encoding.Default.GetString(nameBytes).Trim('\0');

                int kills = 0;
                ReadProcessMemory(process.Handle, CharKillsAddress, ref kills, sizeof(int), 0);
                return new UserInfo { CharName = charName, Kills = kills };
            }
            catch
            {
                return new UserInfo();
            }
        }
        
        public static bool IsGameLaunched() => GetGameProcess() != null;

        public static Process GetGameProcess() => Process.GetProcessesByName("game")
            .FirstOrDefault(x => x.MainWindowTitle == Global.WindowName);
        
        private const int CharIdAddress = 0x009519BC;
        private const int CharNameAddress = 0x0095D954;
        private const int CharKillsAddress = 0x022FBAC8;

        [DllImport("kernel32", SetLastError = true)]
        public static extern int ReadProcessMemory(IntPtr hProcess, int lpBase, ref int lpBuffer, int nSize, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
    }

    public class UserInfo
    {
        public string CharName { get; set; }
        public int Kills { get; set; }
    }
}
