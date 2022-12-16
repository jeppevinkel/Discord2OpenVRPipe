using System.Collections.ObjectModel;
using System.Diagnostics;
using Discord2OpenVRPipe.Models;

namespace Discord2OpenVRPipe.Commands;

public class RemoveCollectionCommand<T> : CommandBase
{
    private readonly ObservableCollection<T> _from;

    public RemoveCollectionCommand(ObservableCollection<T> from)
    {
        _from = from;
    }
    
    public override void Execute(object? parameter)
    {
        if (parameter is not T item) return;
        _from.Remove(item);
    }
}