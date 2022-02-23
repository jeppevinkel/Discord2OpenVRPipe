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
        public SocketGuildChannel SelectedCommandChannel;
        public SocketRole SelectedModeratorRole;
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
                    discordCommandChannels.ItemsSource = SelectedGuild.TextChannels;
                    discordRoles.ItemsSource = SelectedGuild.Roles;
                    
                    int channelIndex = SelectedGuild.TextChannels.ToList().IndexOf(controller.GetChannel(Properties.Settings.Default.DiscordChannelId) as SocketTextChannel);
                    if (channelIndex >= 0)
                    {
                        discordChannels.SelectedIndex = channelIndex;
                        SelectedChannel = discordChannels.SelectedItem as SocketTextChannel;
                    }
                    
                    int commandChannelIndex = SelectedGuild.TextChannels.ToList().IndexOf(controller.GetChannel(Properties.Settings.Default.DiscordCommandChannelId) as SocketTextChannel);
                    if (commandChannelIndex >= 0)
                    {
                        discordCommandChannels.SelectedIndex = commandChannelIndex;
                        SelectedCommandChannel = discordCommandChannels.SelectedItem as SocketTextChannel;
                    }
                    
                    int moderatorRoleIndex = SelectedGuild.Roles.ToList().IndexOf(SelectedGuild.GetRole(Properties.Settings.Default.DiscordModeratorRoleId));
                    if (moderatorRoleIndex >= 0)
                    {
                        discordRoles.SelectedIndex = moderatorRoleIndex;
                        SelectedModeratorRole = discordRoles.SelectedItem as SocketRole;
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
            discordCommandChannels.SelectionChanged += (sender, args) =>
            {
                SelectedCommandChannel = discordCommandChannels.SelectedItem as SocketGuildChannel;
            };
            discordRoles.SelectionChanged += (sender, args) =>
            {
                SelectedModeratorRole = discordRoles.SelectedItem as SocketRole;
            };
        }
        
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedChannel is null || SelectedGuild is null || SelectedCommandChannel is null || SelectedModeratorRole is null)
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