using System;
using System.Diagnostics;
using Discord2OpenVRPipe.Models;
using Discord2OpenVRPipe.Services;
using Discord2OpenVRPipe.Stores;
using Discord2OpenVRPipe.ViewModels;

namespace Discord2OpenVRPipe.Commands;

public class NavigateCommand : CommandBase
{
    private readonly NavigationService _navigationService;

    public NavigateCommand(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public override void Execute(object? parameter)
    {
        _navigationService.Navigate();
    }
}