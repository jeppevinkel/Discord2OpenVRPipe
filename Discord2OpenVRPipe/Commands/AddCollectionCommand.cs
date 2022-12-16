using System.Collections.ObjectModel;
using System.Diagnostics;
using Discord2OpenVRPipe.Models;

namespace Discord2OpenVRPipe.Commands;

public class AddCollectionCommand<T> : CommandBase
{
    private readonly ObservableCollection<T> _to;

    public AddCollectionCommand(ObservableCollection<T> to)
    {
        _to = to;
    }
    
    public override void Execute(object? parameter)
    {
        if (parameter is not T item) return;
        _to.Add(item);
    }
}