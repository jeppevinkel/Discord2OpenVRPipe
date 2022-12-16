using System;
using System.Diagnostics;

namespace Discord2OpenVRPipe.Commands;

public class HyperlinkCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        if (parameter is string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}