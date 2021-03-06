using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Discord;
using Discord.WebSocket;
using Discord2OpenVRPipe.Properties;
using Newtonsoft.Json;
using nQuant;
using Valve.VR;
using Websocket.Client;
using Color = System.Drawing.Color;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Discord2OpenVRPipe
{
    public class AppController
    {
        private bool _openVRConnected = false;
        private bool _discordConnected = false;
        private bool _notificationPipeConnected = false;
        private EasyOpenVRSingleton _vr = EasyOpenVRSingleton.Instance;
        private readonly Action<bool> _openvrStatusAction = delegate(bool b) { Debug.WriteLine($"[OpenVR] {(b == true ? "Connected" : "Disconnected")}"); };
        private readonly Action<bool> _discordStatusAction = delegate(bool b) { Debug.WriteLine($"[Discord] {(b == true ? "Connected" : "Disconnected")}"); };
        private readonly Action<string> _discordReadyAction = delegate(string ch) { Debug.WriteLine($"[Discord] Channel: {ch}"); };
        private readonly Action<bool> _notificationPipeAction = delegate(bool b) { Debug.WriteLine($"[Pipe] {(b == true ? "Connected" : "Disconnected")}"); };
        private bool _shouldShutDown = false;
        private DiscordSocketClient _discordClient;
        private WebsocketClient _notificationPipe;
        private MainWindow _mainWindow;

        public AppController(MainWindow mainWindow, Action<bool> openvrStatusAction, Action<bool> discordStatusAction, Action<string> discordReadyAction, Action<bool> notificationPipeAction)
        {
            _mainWindow = mainWindow;
            _openvrStatusAction += openvrStatusAction;
            _discordStatusAction += discordStatusAction;
            _discordReadyAction += discordReadyAction;
            _notificationPipeAction += notificationPipeAction;
            var thread = new Thread(Worker);
            if (!thread.IsAlive) thread.Start();
            var discordThread = new Task(DiscordMain);
            discordThread.Start();
        }

        public IReadOnlyCollection<SocketGuild> GetGuilds()
        {
            return _discordClient.Guilds;
        }

        public SocketGuild GetGuild(ulong id)
        {
            return _discordClient.GetGuild(id);
        }

        public SocketChannel GetChannel(ulong id)
        {
            return _discordClient.GetChannel(id);
        }

        private readonly string _testImage =
            "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";

        public void TestPipe(NotificationStyleConfig notificationStyleConfig = null)
        {
            PushPipe(_testImage, notificationStyleConfig);
        }
        
        public void PushPipe(string imgData, NotificationStyleConfig notificationStyleConfig = null)
        {
            var notificationStyle = notificationStyleConfig ?? Properties.Settings.Default.NotificationStyle;
            
            var json = JsonConvert.SerializeObject(notificationStyle.GetNotification(imgData));
            
            Debug.WriteLine(json);
            
            _notificationPipe.Send(json);
        }
        
        public async void PushPipeAsync(Task<string> imgDataPromise, NotificationStyleConfig notificationStyleConfig = null)
        {
            var imgData = await imgDataPromise;
            PushPipe(imgData, notificationStyleConfig);
        }

        DateTimeOffset cooldownExpiry = DateTimeOffset.UtcNow;
        
        private async void DiscordMain()
        {
            Thread.CurrentThread.IsBackground = true;
            
            _discordClient = new DiscordSocketClient();
            _discordClient.Log += message =>
            {
                Debug.WriteLine(message.ToString());
                return Task.CompletedTask;
            };

            _discordClient.LoggedIn += () =>
            {
                _discordStatusAction.Invoke(true);
                _discordConnected = true;
                return Task.CompletedTask;
            };
            _discordClient.LoggedOut += () =>
            {
                _discordStatusAction.Invoke(false);
                _discordConnected = false;
                return Task.CompletedTask;
            };
            _discordClient.Ready += () =>
            {
                SocketChannel channel = _discordClient.GetChannel(Properties.Settings.Default.DiscordChannelId);
                if (channel is SocketTextChannel txtChannel)
                {
                    _discordReadyAction.Invoke(txtChannel.Name);
                }
                else
                {
                    _discordReadyAction.Invoke("          ");
                }
                
                return Task.CompletedTask;
            };

            _discordClient.MessageReceived += message =>
            {
                if (message.Author.IsBot ||
                    !(message.Channel is SocketTextChannel channel) || channel.Guild.Id != Properties.Settings.Default.DiscordServerId)
                {
                    return Task.CompletedTask;
                }
                
                if (message.Channel.Id == Properties.Settings.Default.DiscordCommandChannelId && message.Content.StartsWith(Properties.Settings.Default.CommandPrefix) && message.Author is SocketGuildUser member)
                {
                    if (member.Roles.All(r => r.Id != Settings.Default.DiscordModeratorRoleId))
                    {
                        channel.SendMessageAsync("Fuck off you moderator wannabe!");
                        return Task.CompletedTask;
                    }
                    
                    var command =
                        DiscordCommand.Parse(
                            message.Content.Remove(0, Properties.Settings.Default.CommandPrefix.Length));

                    switch (command.Command)
                    {
                        case "ping":
                        {
                            message.Channel.SendMessageAsync($"Pong! {string.Join(" ", command.Args)}");
                        } break;
                        case "cooldown":
                        {
                            var act = new Action<DiscordCommand>(command =>
                            {
                                if (command.Args.Length != 1)
                                {
                                    message.Channel.SendMessageAsync(
                                        "You have to supply either off or the length of the cooldown in minutes.");
                                    return;
                                }

                                if (command.Args[0] == "off")
                                {
                                    _mainWindow.CooldownEnabled = false;
                                    message.Channel.SendMessageAsync(
                                        $"The cooldown has been disabled.");
                                    return;
                                } else if (command.Args[0] == "on")
                                {
                                    _mainWindow.CooldownEnabled = true;
                                    message.Channel.SendMessageAsync(
                                        $"The cooldown has been enabled.");
                                    return;
                                }
                                else if (double.TryParse(command.Args[0], out var newCooldown))
                                {
                                    var max = _mainWindow.Cooldown.Maximum;
                                    var min = _mainWindow.Cooldown.Minimum;

                                    if (newCooldown > max || newCooldown < min)
                                    {
                                        message.Channel.SendMessageAsync(
                                            $"The cooldown has to be more than {min} minutes and less than {max} minutes.");
                                        return;
                                    }

                                    _mainWindow.CooldownEnabled = true;
                                    _mainWindow.SetCooldown(newCooldown);

                                    message.Channel.SendMessageAsync(
                                        $"The cooldown has been set to {command.Args[0]} minutes.");
                                    return;
                                }
                                else
                                {
                                    message.Channel.SendMessageAsync(
                                        "The supplied value is invalid. It must be either `off` or a number.");
                                    return;
                                }
                            });
                            _mainWindow.Dispatcher.Invoke(act, command);
                        } break;
                    }
                    
                    return Task.CompletedTask;
                }

                if (message.Channel.Id != Properties.Settings.Default.DiscordChannelId)
                {
                    return Task.CompletedTask;
                }

                if (!message.Attachments.Any(a => a.Filename.EndsWith(".png") || a.Filename.EndsWith(".jpg"))) return Task.CompletedTask;

                if (Properties.Settings.Default.CooldownEnabled && cooldownExpiry > DateTimeOffset.UtcNow)
                {
                    var timeLeft = (cooldownExpiry - DateTimeOffset.UtcNow);
                    message.Channel.SendMessageAsync($"Fuck you this is too much! Try again in {timeLeft.Minutes} minutes and {timeLeft.Seconds} seconds you idiot!");
                    return Task.CompletedTask;
                }
                
                foreach (Attachment attachment in message.Attachments.Where(a => a.Filename.EndsWith(".png") || a.Filename.EndsWith(".jpg") || a.Filename.EndsWith(".jpeg")))
                {
                    var img = GetImageAsBitmapUrl(attachment.Url).Result;

                    var scale = Math.Min(1280f / img.Width, (720f / img.Height));
                    var newWidth = (int) (img.Width * scale);
                    var newHeight = (int) (img.Height * scale);

                    var resized = ResizeBitmap(img, newWidth, newHeight);

                    if (Properties.Settings.Default.WatermarkImages)
                        WatermarkBitmap(resized, message.Author.Username);
                    
                    using var ms = new MemoryStream();
                    var quantizer = new WuQuantizer();
                    using (var quantized = quantizer.QuantizeImage(resized, 10, 70))
                    {
                        quantized.Save(ms, ImageFormat.Png);
                    }

                    
                    // resized.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    var sigBase64 = Convert.ToBase64String(ms.GetBuffer());
                    Debug.WriteLine(sigBase64);

                    message.Channel.SendMessageAsync($"I have piped {attachment.Filename} to VR.");
                    PushPipe(sigBase64);
                    
                    cooldownExpiry = DateTimeOffset.UtcNow.AddMinutes(Properties.Settings.Default.CooldownMinutes);
                }
                
                return Task.CompletedTask;
            };

            ReconnectDiscord();
        }

        public Bitmap WatermarkBitmap(Bitmap bmp, string watermark)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Font f = new Font("Verdana", 21);
                var savedCol = Properties.Settings.Default.WatermarkColor;
                g.DrawString(watermark, f, new SolidBrush(Color.FromArgb(savedCol.R, savedCol.G, savedCol.B)), 5, bmp.Height - f.Height - 5);
            }

            return bmp;
        }
        
        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }
 
            return result;
        }

        public async void ReconnectDiscord()
        {
            if (_discordClient is null)
            {
                var discordThread = new Task(DiscordMain);
                discordThread.Start();
                return;
            }
            if (_discordConnected)
            {
                await _discordClient.LogoutAsync();
            }
            
            try
            {
                await _discordClient.LoginAsync(TokenType.Bot, Properties.Settings.Default.BotToken);
                await _discordClient.StartAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        
        private void Worker()
        {
            var vrInitComplete = false;
            var pipeInitComplete = false;
            DateTimeOffset lastStatusUpdate = DateTimeOffset.UtcNow;

            Thread.CurrentThread.IsBackground = true;
            while (true)
            {
                if (_openVRConnected)
                {
                    if (!vrInitComplete)
                    {
                        vrInitComplete = true;
                        _vr.AddApplicationManifest("./app.vrmanifest", "jeppevinkel.discord2openvrpipe", true);
                        Debug.WriteLine(Path.GetFullPath("./app.vrmanifest"));
                        _openvrStatusAction.Invoke(true);
                        RegisterEvents();
                    }
                    else
                    {
                        _vr.UpdateEvents(false);
                    }
                    Thread.Sleep(250);
                }
                else
                {
                    Debug.WriteLine("Initializing OpenVR...");
                    _openVRConnected = _vr.Init();
                    Thread.Sleep(2000);
                }

                if (_notificationPipeConnected)
                {
                    
                }
                else
                {
                    if (!pipeInitComplete)
                    {
                        var rawUrl = $"ws://localhost:{Properties.Settings.Default.PipePort.ToString()}";
                        var url = new Uri(rawUrl);
                        _notificationPipe =
                            new WebsocketClient(url);

                        _notificationPipe.IsReconnectionEnabled = true;
                        _notificationPipe.ReconnectTimeout = TimeSpan.FromSeconds(5);

                        _notificationPipe.ReconnectionHappened.Subscribe(info =>
                        {
                            _notificationPipeAction.Invoke(true);
                            _notificationPipeConnected = true;
                        });

                        _notificationPipe.DisconnectionHappened.Subscribe(info =>
                        {
                            _notificationPipeAction.Invoke(false);
                            _notificationPipeConnected = false;
                            Debug.WriteLine($"[Pipe] {info.Type}, {info.CloseStatusDescription}");
                        });

                        _notificationPipe.MessageReceived.Subscribe(msg =>
                        {
                            Debug.WriteLine($"[Pipe] {msg}");
                        });

                        _notificationPipe.Start();
                        pipeInitComplete = true;
                    }
                    else
                    {
                        
                    }
                }

                if (_shouldShutDown)
                {
                    _shouldShutDown = false;
                    vrInitComplete = false;
                    pipeInitComplete = false;
                    _vr.AcknowledgeShutdown();
                    Thread.Sleep(500);
                    _vr.Shutdown();
                    _openvrStatusAction.Invoke(false);
                }

                if (_discordConnected && Settings.Default.CooldownEnabled && (DateTimeOffset.UtcNow.Subtract(lastStatusUpdate) > TimeSpan.FromSeconds(10)) && (cooldownExpiry > DateTimeOffset.UtcNow))
                {
                    var str = $"Cooldown: {(cooldownExpiry - DateTimeOffset.UtcNow).ToString("mm\\:ss")}";
                    Debug.WriteLine(str);
                    _discordClient.SetActivityAsync(new StreamingGame(
                        str,
                        "https://twitch.tv/c0ldvengeance"));
                    
                    lastStatusUpdate = DateTimeOffset.UtcNow;
                } else if (_discordConnected && Settings.Default.CooldownEnabled && (DateTimeOffset.UtcNow.Subtract(lastStatusUpdate) > TimeSpan.FromSeconds(10)) && _discordClient.Activity is not null)
                {
                    _discordClient.SetActivityAsync(null);
                    
                    lastStatusUpdate = DateTimeOffset.UtcNow;
                }
            }
        }

        private void RegisterEvents()
        {
            _vr.RegisterEvent(EVREventType.VREvent_Quit, (data) =>
            {
                _openVRConnected = false;
                _shouldShutDown = true;
                });
        }
        
        public async static Task<string> GetImageAsBase64Url(string url)
        {
            using (var client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(url);
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<Bitmap> GetImageAsBitmapUrl(string url)
        {
            System.Net.WebRequest request =
                System.Net.WebRequest.Create(
                    url);
            System.Net.WebResponse response = await request.GetResponseAsync();
            System.IO.Stream responseStream =
                response.GetResponseStream();
            if (responseStream != null)
            {
                Bitmap bitmap = new Bitmap(responseStream);
                return bitmap;
            }
            else
            {
                return null;
            }
        }
        
        public void Shutdown()
        {
            _shouldShutDown = true;
        }
    }

    public class DiscordCommand
    {
        public string Command { get; set; }
        public string[] Args { get; set; }

        public DiscordCommand(string command, IEnumerable<string> args)
        {
            Command = command;
            Args = args.ToArray();
        }

        public static DiscordCommand Parse(string command)
        {
            var strings = command.Split(' ');
            return new DiscordCommand(strings.First(), strings.Skip(1));
        }
    }
}