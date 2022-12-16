namespace Discord2OpenVRPipe.Models;

public class Pipe
{
    public string Port { get; set; }
    public PipeNotification Notification { get; set; }

    public Pipe(string port)
    {
        Port = port;
        Notification = new PipeNotification();
    }
}