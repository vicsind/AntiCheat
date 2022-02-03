using System;
using System.Windows;
using System.Windows.Forms;
using Teos.Properties;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MenuItem = System.Windows.Controls.MenuItem;

namespace Teos
{
    public class Notificator : IDisposable
    {

        public Notificator()
        {
            MenuItem discordMenuItem = new MenuItem { Header = "Show game in Discord" };
            discordMenuItem.IsCheckable = true;
            discordMenuItem.IsChecked = Settings.Default.DiscordApi;
            discordMenuItem.Checked += DiscordMenuItemClick;
            discordMenuItem.Unchecked += DiscordMenuItemClick;

            MenuItem exitMenuItem = new MenuItem { Header = "Exit" };
            exitMenuItem.Click += Menu_Close;

            _contextMenu.Items.Add(discordMenuItem);
            _contextMenu.Items.Add(exitMenuItem);

            // Notify icon
            NotifyIcon.MouseClick += NotifyIconOnMouseClick;
        }

        private void NotifyIconOnMouseClick(object sender, MouseEventArgs e)
        {
            _contextMenu.IsOpen = e.Button == MouseButtons.Right;
        }

        private static void DiscordMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            Settings.Default.DiscordApi = menuItem.IsChecked;
            Settings.Default.Save();
        }

        /*
         * NOTIFY ICON
         */
        private static void Menu_Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        

        public void Dispose()
        {
            NotifyIcon?.Dispose();
        }

        /// <summary>
        /// Иконка в трее.
        /// </summary>
        public readonly NotifyIcon NotifyIcon = new NotifyIcon { Icon = Resources.app, Visible = true };

        /// <summary>
        /// 
        /// </summary>
        private readonly ContextMenu _contextMenu = new ContextMenu();
    }
}
