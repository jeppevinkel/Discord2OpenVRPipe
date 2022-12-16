using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Discord2OpenVRPipe.Commands;
using Discord2OpenVRPipe.Models;
using Discord2OpenVRPipe.Repositories;
using Discord2OpenVRPipe.Services;
using Discord2OpenVRPipe.Stores;

namespace Discord2OpenVRPipe.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;
    private readonly DiscordConfig _discordConfig;
    private readonly HttpClient _httpClient;
    public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;
    
    public Version Version { get; }
    public string VersionString => $"v{Version.ToString(3)}";
    private string _newVersionString = "";
    public string NewVersionString
    {
        get => _newVersionString;
        set
        {
            _newVersionString = value;
            OnPropertyChanged();
        }
    }
    
    private string _newVersionUrl = "";
    public string NewVersionUrl
    {
        get => _newVersionUrl;
        set
        {
            _newVersionUrl = value;
            OnPropertyChanged();
        }
    }
    
    private bool _newVersionAvailable = false;
    public bool NewVersionAvailable
    {
        get => _newVersionAvailable;
        set
        {
            _newVersionAvailable = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand GeneralSettingsCommand { get; }
    public ICommand DiscordConfigCommand { get; }
    public ICommand PipeConfigCommand { get; }
    public ICommand HyperlinkCommand { get; }

    public MainViewModel(NavigationStore navigationStore, DiscordConfig discordConfig)
    {
        Version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new InvalidOperationException("Version should never be null");
        
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Discord2OpenVRPipe Version Checker");
        _httpClient.BaseAddress = new Uri("https://api.github.com");

        _discordConfig = discordConfig;

        GeneralSettingsCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateGeneralSettingsViewModel));
        DiscordConfigCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateDiscordConfigViewModel));
        PipeConfigCommand = new NavigateCommand(new NavigationService(_navigationStore, CreateGeneralSettingsViewModel));
        HyperlinkCommand = new HyperlinkCommand();

        Task.Run(async () =>
        {
            var release = await GetLatestVersion();

            if (release is null) return;

            var versionComponents = release.TagName.Split('-');

            if (versionComponents.Length > 0)
            {
                var versionString = Regex.Replace(versionComponents[0], "[^0-9.]", "");
                var version = new Version(versionString);

                var newRelease = version > Version;

                if (newRelease)
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        NewVersionString = $"New release available! Latest version is {release.TagName}";
                        NewVersionUrl = release.HtmlUrl;
                        NewVersionAvailable = true;
                    });
                }
            }
        });

    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    private GeneralSettingsViewModel CreateGeneralSettingsViewModel()
    {
        return new GeneralSettingsViewModel();
    }

    private DiscordConfigViewModel CreateDiscordConfigViewModel()
    {
        return new DiscordConfigViewModel(_discordConfig);
    }

    public async Task<GithubReleaseRepository?> GetLatestVersion()
    {
        const string url = "/repos/{owner}/{repo}/releases/latest";
        var formattedUrl = url.Replace("{owner}", "jeppevinkel")
            .Replace("{repo}", "discord2openvrpipe");

        await using var stream = await _httpClient.GetStreamAsync(formattedUrl);

        try
        {
            var release = await JsonSerializer.DeserializeAsync<GithubReleaseRepository>(stream);
            return release;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return null;
        }
    }
}