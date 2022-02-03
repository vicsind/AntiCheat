using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using Teos.Discord;
using Teos.Logging;

namespace Teos
{
    public partial class App : Application
    {
        private const string Guid = "0dde9e46-7b9c-46e3-8478-a6cacddde2e8";
        private static Notificator _notificator;
        private readonly DiscordSystem _discordSystem = new DiscordSystem();

        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            Mutex mutex = new Mutex(true, Guid);
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();

                mutex.ReleaseMutex();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (o, args) => Logger.LogError((Exception)args.ExceptionObject);
            _notificator = new Notificator();
            _discordSystem.Start();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _notificator.Dispose();
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);
            
            if (string.Equals(assemblyName.Name, "Newtonsoft.Json", StringComparison.CurrentCultureIgnoreCase))
            {
                return Assembly.Load(Teos.Properties.Resources.Newtonsoft_Json);
            }

            if (string.Equals(assemblyName.Name, "DiscordRPC", StringComparison.CurrentCultureIgnoreCase))
            {
                return Assembly.Load(Teos.Properties.Resources.DiscordRPC);
            }

            return null;
        }
    }
}
