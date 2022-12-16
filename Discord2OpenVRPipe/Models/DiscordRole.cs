using System;

namespace Discord2OpenVRPipe.Models;

[Serializable]
public struct DiscordRole
{
    public DiscordRole(string name, long id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Id = id;
    }

    public string Name { get; set; }
    public long Id { get; set; }
}