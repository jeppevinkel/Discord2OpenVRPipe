using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Discord2OpenVRPipe.Models;
using Discord2OpenVRPipe.Stores;
using Discord2OpenVRPipe.ViewModels;

namespace Discord2OpenVRPipe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly DiscordConfig _discord;
        private readonly NavigationStore _navigationStore;

        public App()
        {
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }
            
            _discord = new DiscordConfig();
            _navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new GeneralSettingsViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore, _discord)
            };
            MainWindow.Show();
            
            base.OnStartup(e);
        }
    }
}
