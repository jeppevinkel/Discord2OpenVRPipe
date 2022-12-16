using System.Collections.Generic;

namespace Discord2OpenVRPipe.Models;

public abstract class DiscordCommand
{
    public string Name { get; }
    public string[] Aliases { get; }
    public string Description { get; }

    public void Execute(string rawArgument)
    {
        var arguments = rawArgument.Trim().Split(' ');
        Execute(arguments);
        
    }

    public virtual void Execute(IEnumerable<string> arguments)
    {
        
    }
}