using System.Windows.Controls;
using System.Windows.Input;
using Discord2OpenVRPipe.ViewModels;

namespace Discord2OpenVRPipe.Views;

public partial class DiscordConfigView : UserControl
{
    public DiscordConfigView()
    {
        InitializeComponent();
    }

    private void OnMouseDoubleClickWhitelistAvailable(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is DiscordConfigViewModel vm && vm.AddToWhitelistCommand.CanExecute(null))
        {
            vm.AddToWhitelistCommand.Execute(vm.WhitelistAvailableSelected);
        }
    }

    private void OnMouseDoubleClickWhitelistEnabled(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is DiscordConfigViewModel vm && vm.RemoveFromWhitelistCommand.CanExecute(null))
        {
            vm.RemoveFromWhitelistCommand.Execute(vm.WhitelistEnabledSelected);
        }
    }
}