using Discord2OpenVRPipe.ViewModels;

namespace Discord2OpenVRPipe.Commands;

public class DiscordConnectCommand : CommandBase
{
    private readonly DiscordConfigViewModel _discordConfigViewModel;

    public DiscordConnectCommand(DiscordConfigViewModel discordConfigViewModel)
    {
        _discordConfigViewModel = discordConfigViewModel;
        _discordConfigViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(_discordConfigViewModel.IsConnected))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter)
    {
        return !_discordConfigViewModel.IsConnected;
    }

    public override void Execute(object? parameter)
    {
        _discordConfigViewModel.IsConnected = true;
    }
}