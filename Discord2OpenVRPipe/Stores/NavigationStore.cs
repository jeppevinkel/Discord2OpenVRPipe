using System;
using Discord2OpenVRPipe.ViewModels;

namespace Discord2OpenVRPipe.Stores;

public class NavigationStore
{
    private ViewModelBase? _currentViewModel;
    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }

    public event Action? CurrentViewModelChanged;
}