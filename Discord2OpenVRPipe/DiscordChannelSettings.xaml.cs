using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Discord.WebSocket;

namespace Discord2OpenVRPipe
{
    public partial class DiscordChannelSettings : Window
    {
        public SocketGuild SelectedGuild;
        public SocketGuildChannel SelectedChannel;
        public DiscordChannelSettings(AppController controller)
        {
            InitializeComponent();
            List<SocketGuild> guilds = controller.GetGuilds().ToList();
            discordServers.ItemsSource = guilds;
            int guildIndex = guilds.IndexOf(controller.GetGuild(Properties.Settings.Default.DiscordServerId));
            if (guildIndex >= 0)
            {
                discordServers.SelectedIndex = guildIndex;
                SelectedGuild = discordServers.SelectedItem as SocketGuild;
                if (SelectedGuild is not null)
                {
                    discordChannels.ItemsSource = SelectedGuild.TextChannels;
                    
                    int channelIndex = SelectedGuild.TextChannels.ToList().IndexOf(controller.GetChannel(Properties.Settings.Default.DiscordChannelId) as SocketTextChannel);
                    if (channelIndex >= 0)
                    {
                        discordChannels.SelectedIndex = channelIndex;
                        SelectedChannel = discordChannels.SelectedItem as SocketTextChannel;
                    }
                }
                
            }
            discordServers.SelectionChanged += (sender, args) =>
            {
                SelectedGuild = discordServers.SelectedItem as SocketGuild;
                if (SelectedGuild is not null)
                {
                    discordChannels.ItemsSource = SelectedGuild.TextChannels;
                }
            };
            discordChannels.SelectionChanged += (sender, args) =>
            {
                SelectedChannel = discordChannels.SelectedItem as SocketGuildChannel;
            };
        }
        
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedChannel is null || SelectedGuild is null)
            {
                DialogResult = false;
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}