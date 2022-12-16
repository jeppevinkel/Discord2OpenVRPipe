using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Discord2OpenVRPipe.Commands;
using Discord2OpenVRPipe.Models;
using Discord2OpenVRPipe.Repositories;

namespace Discord2OpenVRPipe.ViewModels;

public class DiscordConfigViewModel : ViewModelBase
{
    private readonly DiscordConfig _discordConfig;
    private readonly Settings _settings = Settings.Default;
    
    public string? BotToken
    {
        get => _settings.BotToken;
        set
        {
            _settings.BotToken = value;
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

    public bool WhitelistEnabled
    {
        get => _settings.WhitelistEnabled;
        set
        {
            _settings.WhitelistEnabled = value;
            OnPropertyChanged();
        }
    }

    private DiscordRole? _whitelistEnabledSelected;
    public DiscordRole? WhitelistEnabledSelected
    {
        get => _whitelistEnabledSelected;
        set
        {
            _whitelistEnabledSelected = value;
            OnPropertyChanged();
        }
    }

    private DiscordRole? _whitelistAvailableSelected;
    public DiscordRole? WhitelistAvailableSelected
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

    public List<DiscordRole> WhitelistRolesAvailable => DiscordRoles.Except(WhitelistedRoles).ToList();

    // public ObservableCollection<DiscordRole> DiscordModeratorRoles => _settings.DiscordModeratorRoles;

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
        AddToWhitelistCommand = new AddCollectionCommand<DiscordRole>(WhitelistedRoles);
        RemoveFromWhitelistCommand = new RemoveCollectionCommand<DiscordRole>(WhitelistedRoles);


        WhitelistedRoles.CollectionChanged += (sender, args) =>
        {
            _settings.WhitelistedRoles = WhitelistedRoles.ToArray();
            OnPropertyChanged(nameof(WhitelistRolesAvailable));
        };
        DiscordRoles.CollectionChanged += (sender, args) =>
        {
            OnPropertyChanged(nameof(WhitelistRolesAvailable));
        };
        
        foreach (var discordRole in _settings.WhitelistedRoles)
        {
            WhitelistedRoles.Add(discordRole);
        }
        
        DiscordRoles.Add(new DiscordRole("Moderators", 0));
        DiscordRoles.Add(new DiscordRole("Cool kids", 1));

        DiscordRoles.Add(new DiscordRole("Normies", 2));
        DiscordRoles.Add(new DiscordRole("Baddies", 3));
        DiscordRoles.Add(new DiscordRole("Lollers", 54));
    }
}