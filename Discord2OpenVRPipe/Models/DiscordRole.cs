namespace Discord2OpenVRPipe.Models;

public class DiscordRole
{
    public string Name { get; }
    public long Id { get; }

    public DiscordRole(string name, long id)
    {
        Name = name;
        Id = id;
    }
}