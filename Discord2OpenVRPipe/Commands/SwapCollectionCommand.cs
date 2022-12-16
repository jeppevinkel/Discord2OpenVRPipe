using System.Collections.ObjectModel;
using System.Diagnostics;
using Discord2OpenVRPipe.Models;

namespace Discord2OpenVRPipe.Commands;

public class SwapCollectionCommand<T> : CommandBase
{
    private readonly ObservableCollection<T> _from;
    private readonly ObservableCollection<T> _to;

    public SwapCollectionCommand(ObservableCollection<T> from, ObservableCollection<T> to)
    {
        _from = from;
        _to = to;
    }
    
    public override void Execute(object? parameter)
    {
        if (parameter is not T item) return;
        _to.Add(item);
        _from.Remove(item);
    }
}