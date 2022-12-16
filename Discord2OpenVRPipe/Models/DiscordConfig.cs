using System.Collections.ObjectModel;

namespace Discord2OpenVRPipe.Models;

public class DiscordConfig
{
    public string? BotToken { get; set; }
    public string? DiscordServer { get; set; }
    public string? ImageChannel { get; set; }
    public string? CommandChannel { get; set; }
    public ObservableCollection<string> ModeratorRoles { get; }
    public bool RestrictPosting { get; set; }
    public ObservableCollection<string> WhitelistedRoles { get; }
    public ObservableCollection<DiscordCommand> Commands { get; }

    public DiscordConfig()
    {
        ModeratorRoles = new();
        WhitelistedRoles = new();
        Commands = new();
    }
}