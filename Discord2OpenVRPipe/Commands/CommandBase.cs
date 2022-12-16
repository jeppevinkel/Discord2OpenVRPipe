using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Discord2OpenVRPipe.Commands;

public abstract class CommandBase : ICommand
{
    public virtual bool CanExecute(object? parameter)
    {
        Debug.WriteLine(parameter);
        return true;
    }

    public abstract void Execute(object? parameter);

    protected void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? CanExecuteChanged;
}