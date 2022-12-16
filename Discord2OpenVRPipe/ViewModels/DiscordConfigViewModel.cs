using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Discord2OpenVRPipe.Commands;
using Discord2OpenVRPipe.Models;

namespace Discord2OpenVRPipe.ViewModels;

public class DiscordConfigViewModel : ViewModelBase
{
    private readonly DiscordConfig _discordConfig;
    private readonly Settings _settings = Settings.Default;
    
    private string? _botToken;
    public string? BotToken
    {
        get => _botToken;
        set
        {
            _botToken = value;
            OnPropertyChanged();
        }
    }

    private bool _isConnected = false;
    public bool IsConnected
    {
        get => _isConnected;
        set
        {
            _isConnected = value;
            OnPropertyChanged();
        }
    }

    private bool _whitelistEnabled = false;
    public bool WhitelistEnabled
    {
        get => _whitelistEnabled;
        set
        {
            _whitelistEnabled = value;
            OnPropertyChanged();
        }
    }

    private DiscordRole _whitelistEnabledSelected;
    public DiscordRole WhitelistEnabledSelected
    {
        get => _whitelistEnabledSelected;
        set
        {
            _whitelistEnabledSelected = value;
            OnPropertyChanged();
        }
    }

    private DiscordRole _whitelistAvailableSelected;
    public DiscordRole WhitelistAvailableSelected
    {
        get => _whitelistAvailableSelected;
        set
        {
            _whitelistAvailableSelected = value;
            OnPropertyChanged();
        }
    }

    public double CooldownMinutes
    {
        get => _settings.CooldownMinutes;
        set
        {
            _settings.CooldownMinutes = value;
            OnPropertyChanged();
        }
    }

    public bool CooldownEnabled
    {
        get => _settings.CooldownEnabled;
        set
        {
            _settings.CooldownEnabled = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<DiscordRole> WhitelistedRoles { get; } = new();

    public ObservableCollection<DiscordRole> WhitelistRolesAvailable { get; } = new();

    public ObservableCollection<DiscordRole> DiscordServers { get; } = new();

    public ObservableCollection<DiscordRole> DiscordChannels { get; } = new();

    public ObservableCollection<DiscordRole> DiscordRoles { get; } = new();

    public ICommand ConnectCommand { get; }
    public ICommand DisconnectCommand { get; }
    public ICommand AddToWhitelistCommand { get; }
    public ICommand RemoveFromWhitelistCommand { get; }

    public DiscordConfigViewModel(DiscordConfig discordConfig)
    {
        _discordConfig = discordConfig;
        
        ConnectCommand = new DiscordConnectCommand(this);
        DisconnectCommand = new DiscordDisconnectCommand(this);
        AddToWhitelistCommand = new SwapCollectionCommand<DiscordRole>(WhitelistRolesAvailable, WhitelistedRoles);
        RemoveFromWhitelistCommand = new SwapCollectionCommand<DiscordRole>(WhitelistedRoles, WhitelistRolesAvailable);

        WhitelistedRoles.Add(new DiscordRole("Moderators", 0));
        WhitelistedRoles.Add(new DiscordRole("Cool kids", 1));

        WhitelistRolesAvailable.Add(new DiscordRole("Normies", 2));
        WhitelistRolesAvailable.Add(new DiscordRole("Baddies", 3));
    }
}