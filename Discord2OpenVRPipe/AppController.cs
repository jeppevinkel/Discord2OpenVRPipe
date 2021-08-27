using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Valve.VR;
using static Discord2OpenVRPipe.EasyOpenVRSingleton;

namespace Discord2OpenVRPipe
{
    public class AppController
    {
        private bool _openVRConnected = false;
        private bool _discordConnected = false;
        private EasyOpenVRSingleton _vr = EasyOpenVRSingleton.Instance;
        private Action<bool> _openvrStatusAction;
        private Action<bool> _discordStatusAction;
        private Action<string> _discordReadyAction;
        private bool _shouldShutDown = false;
        private DiscordSocketClient _discordClient;

        public AppController(Action<bool> openvrStatusAction, Action<bool> discordStatusAction, Action<string> discordReadyAction)
        {
            _openvrStatusAction = openvrStatusAction;
            _discordStatusAction = discordStatusAction;
            _discordReadyAction = discordReadyAction;
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
                if (message.Author.IsBot || message.Channel.Id != Properties.Settings.Default.DiscordChannelId ||
                    !(message.Channel is SocketTextChannel channel) || channel.Guild.Id != Properties.Settings.Default.DiscordServerId || !message.Attachments.Any(a => a.Filename.EndsWith(".png") || a.Filename.EndsWith(".jpg"))) return Task.CompletedTask;

                foreach (Attachment attachment in message.Attachments.Where(a => a.Filename.EndsWith(".png") || a.Filename.EndsWith(".jpg")))
                {
                    message.Channel.SendMessageAsync($"I have piped {attachment.Filename} to VR.");
                }
                
                return Task.CompletedTask;
            };

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
            var initComplete = false;

            Thread.CurrentThread.IsBackground = true;
            while (true)
            {
                if (_openVRConnected)
                {
                    if (!initComplete)
                    {
                        initComplete = true;
                        _vr.AddApplicationManifest("./app.vrmanifest", "jeppevinkel.discord2openvrpipe", true);
                        _openvrStatusAction.Invoke(true);
                    }
                    else
                    {
                        _vr.UpdateEvents(false);
                    }
                    Thread.Sleep(250);
                }
                else
                {
                    if (!_openVRConnected)
                    {
                        Debug.WriteLine("Initializing OpenVR...");
                        _openVRConnected = _vr.Init();
                    }
                    Thread.Sleep(2000);
                }

                if (_shouldShutDown)
                {
                    _shouldShutDown = false;
                    initComplete = false;
                    _vr.AcknowledgeShutdown();
                    Thread.Sleep(500);
                    _vr.Shutdown();
                    _openvrStatusAction.Invoke(false);
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
        
        public void Shutdown()
        {
            _openvrStatusAction = (status) => { };
            _shouldShutDown = true;
        }
    }
}