using System;
using System.Diagnostics;
using System.Drawing;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using Discord2OpenVRPipe.Properties;
using Brushes = System.Windows.Media.Brushes;

namespace Discord2OpenVRPipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private AppController _controller;
        private Properties.Settings _settings = Properties.Settings.Default;
        private System.Windows.Forms.NotifyIcon _notifyIcon = null;
        private WindowState _storedWindowState = WindowState.Normal;

        public double CooldownMinutes
        {
            get => Properties.Settings.Default.CooldownMinutes;
            set
            {
                if (Dispatcher.CheckAccess())
                {
                    Settings.Default.CooldownMinutes = value;
                    Cooldown.SetValue(value);
                }
                else
                {
                    var act = new Action<double>(cooldown =>
                    {
                        Settings.Default.CooldownMinutes = cooldown;
                        Cooldown.SetValue(cooldown);
                    });
                    Dispatcher.Invoke(act, args: value);
                }
            }
        }

        public bool CooldownEnabled
        {
            get => Settings.Default.CooldownEnabled;
            set
            {
                if (Dispatcher.CheckAccess())
                {
                    Settings.Default.CooldownEnabled = value;
                    CooldownEnabledCheckBox.IsChecked = value;
                }
                else
                {
                    var act = new Action<bool>(cooldownEnabled =>
                    {
                        Settings.Default.CooldownEnabled = cooldownEnabled;
                        CooldownEnabledCheckBox.IsChecked = cooldownEnabled;
                    });
                    Dispatcher.Invoke(act, args: value);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Title = Properties.Resources.AppName;

            LoadSettings();
            
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            _notifyIcon.BalloonTipTitle = Properties.Resources.AppName;
            _notifyIcon.Text = Properties.Resources.AppName;
            _notifyIcon.Icon = new Icon(@"icon.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Click += new EventHandler(NotifyIconClick);
            _notifyIcon.BalloonTipClicked += new EventHandler(NotifyIconClick);
            
            Label_Version.Content = Properties.Resources.Version;

            _controller = new AppController(this,  status =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (status)
                    {
                        Label_OpenVRStatus.Background = Brushes.OliveDrab;
                        Label_OpenVRStatus.Text = "Connected";
                    }
                    else
                    {
                        Label_OpenVRStatus.Background = Brushes.Tomato;
                        Label_OpenVRStatus.Text = "Disconnected";

                        if (_settings.ExitWithSteam)
                        {
                            _controller.Shutdown();
                            _notifyIcon?.Dispose();
                            System.Windows.Application.Current.Shutdown();
                        }
                        
                    }
                });
            },
                status =>
                {
                    if (Dispatcher.CheckAccess())
                    {
                        if (status)
                        {
                            Label_DiscordStatus.Background = Brushes.OliveDrab;
                            Label_DiscordStatus.Text = "Connected";
                        }
                        else
                        {
                            Label_DiscordStatus.Background = Brushes.Tomato;
                            Label_DiscordStatus.Text = "Disconnected";

                        }
                    }
                    else
                    {
                        var act = new Action<bool>(_status =>
                        {
                            if (_status)
                            {
                                Label_DiscordStatus.Background = Brushes.OliveDrab;
                                Label_DiscordStatus.Text = "Connected";
                            }
                            else
                            {
                                Label_DiscordStatus.Background = Brushes.Tomato;
                                Label_DiscordStatus.Text = "Disconnected";

                            }
                        });
                        Dispatcher.Invoke(act, args: status);
                    }
                }, channel =>
                {
                    if (Dispatcher.CheckAccess())
                    {
                        textBoxChannel.Text = channel;
                    }
                    else
                    {
                        var act = new Action<string>(_channel =>
                        {
                            textBoxChannel.Text = _channel;
                        });
                        Dispatcher.Invoke(act, args: channel);
                    }
                }, status =>
                {
                    if (Dispatcher.CheckAccess())
                    {
                        if (status)
                        {
                            Label_PipeStatus.Background = Brushes.OliveDrab;
                            Label_PipeStatus.Text = "Connected";
                        }
                        else
                        {
                            Label_PipeStatus.Background = Brushes.Tomato;
                            Label_PipeStatus.Text = "Disconnected";

                        }
                    }
                    else
                    {
                        var act = new Action<bool>(_status =>
                        {
                            if (_status)
                            {
                                Label_PipeStatus.Background = Brushes.OliveDrab;
                                Label_PipeStatus.Text = "Connected";
                            }
                            else
                            {
                                Label_PipeStatus.Background = Brushes.Tomato;
                                Label_PipeStatus.Text = "Disconnected";

                            }
                        });
                        Dispatcher.Invoke(act, args: status);
                    }
                });

            Cooldown.IsEnabled = Settings.Default.CooldownEnabled;
            Cooldown.Value = Settings.Default.CooldownMinutes;
            CooldownEnabledCheckBox.IsChecked = Settings.Default.CooldownEnabled;
            Cooldown.ValueChanged += (sender, args) =>
            {
                Settings.Default.CooldownMinutes = args.NewValue;
            };

            textBoxCommandPrefix.Text = Settings.Default.CommandPrefix;
            textBoxCommandPrefix.TextChanged += (sender, args) =>
            {
                Settings.Default.CommandPrefix = textBoxCommandPrefix.Text;
            };
            
            if (_settings.LaunchMinimized)
            {
                if (_settings.MinimizeToTray)
                {
                    // _notifyIcon.ShowBalloonTip(5000, "Tip", "I am minimized to the system tray!", ToolTipIcon.Info);
                    Hide();
                    _notifyIcon.ShowBalloonTip(2000);
                }
                WindowState = WindowState.Minimized;
            }
        }

        public void SetCooldown(double value)
        {
            if (Dispatcher.CheckAccess())
            {
                Cooldown.SetValue(value);
                // Cooldown.Value = value;
                Settings.Default.CooldownMinutes = value;
            }
            else
            {
                var act = new Action<double>(_value =>
                {
                    Cooldown.SetValue(_value);
                    // Cooldown.Value = _value;
                    Settings.Default.CooldownMinutes = _value;
                });
                Dispatcher.Invoke(act, value);
            }
        }
        
        void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                if (_settings.MinimizeToTray)
                {
                    Hide();
                }
                if (_notifyIcon != null)
                {
                    _notifyIcon.ShowBalloonTip(2000);
                    // ShowInTaskbar = false;
                }
            }
            else
            {
                _storedWindowState = WindowState;
                // ShowInTaskbar = true;
            }
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }
        
        private void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }
        
        private void ShowTrayIcon(bool show)
        {
            if (_notifyIcon != null)
                _notifyIcon.Visible = show;
        }
        
        private void NotifyIconClick(object sender, EventArgs e)
        {
            Show();
            Activate();
            WindowState = _storedWindowState;
        }

        public void SetMinimizeToTray(bool state)
        {
            ShowInTaskbar = !state;
        }
        
        private void LoadSettings()
        {
            if (_settings.NotificationStyle is null)
            {
                _settings.NotificationStyle = new NotificationStyleConfig();
            }
            
            checkBox_MinimizeOnLaunch.IsChecked = _settings.LaunchMinimized;
            checkBox_MinimizeToTray.IsChecked = _settings.MinimizeToTray;
            checkBox_ExitWithSteam.IsChecked = _settings.ExitWithSteam;
            textBoxPort.Text = _settings.PipePort.ToString();
            textBoxToken.Text = _settings.BotToken;
        }

        private void ButtonPortEditClick(object sender, RoutedEventArgs e)
        {
            int port = InputDialog.PromptInt("OpenVR2Pipe port", "Edit Pipe Port", _settings.PipePort);
            if (port != int.MinValue)
            {
                _settings.PipePort = port;
                _settings.Save();
                textBoxPort.Text = _settings.PipePort.ToString();
            }
        }

        private void ButtonTokenEditClick(object sender, RoutedEventArgs e)
        {
            string token = InputDialog.PromptString("Discord bot token", "Edit Bot Token", _settings.BotToken);
            if(token != null)
            {
                _settings.BotToken = token;
                _settings.Save();
                textBoxToken.Text = _settings.BotToken;
            }
        }
        
        private void ClickedURL(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            Process.Start(link.NavigateUri.ToString());
        }
        
        private bool CheckboxValue(RoutedEventArgs e)
        {
            var name = e.RoutedEvent.Name;
            return name == "Checked";
        }
        
        private void CheckBox_MinimizeOnLaunch_Checked(object sender, RoutedEventArgs e)
        {
            _settings.LaunchMinimized = CheckboxValue(e);
            _settings.Save();
        }

        private void CheckBox_MinimizeToTray_Checked(object sender, RoutedEventArgs e)
        {
            _settings.MinimizeToTray = CheckboxValue(e);
            _settings.Save();
        }

        private void CheckBox_ExitWithSteam_Checked(object sender, RoutedEventArgs e)
        {
            _settings.ExitWithSteam = CheckboxValue(e);
            _settings.Save();
        }

        private void ButtonBotReconnectClick(object sender, RoutedEventArgs e)
        {
            _controller.ReconnectDiscord();
        }

        private void ButtonChannelEditClick(object sender, RoutedEventArgs e)
        {
            var dialog = new DiscordChannelSettings(_controller);
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                Settings.Default.DiscordServerId = dialog.SelectedGuild?.Id ?? 0;
                Settings.Default.DiscordChannelId = dialog.SelectedChannel?.Id ?? 0;
                Settings.Default.DiscordCommandChannelId = dialog.SelectedCommandChannel?.Id ?? 0;
                Settings.Default.DiscordModeratorRoleId = dialog.SelectedModeratorRole?.Id ?? 0;
                textBoxChannel.Text = dialog.SelectedChannel?.Name ?? "None Selected";
            }
        }

        private void ButtonStyleEditClick(object sender, RoutedEventArgs e)
        {
            var notif = new NotificationStyleSettings(_settings.NotificationStyle, _controller);
            notif.ShowDialog();
            var res = notif.DialogResult;
        }

        private void ButtonStyleTestClick(object sender, RoutedEventArgs e)
        {
            _controller.TestPipe();
        }

        private void CheckBox_CooldownEnabled(object sender, RoutedEventArgs e)
        {
            bool enabled = CheckboxValue(e);
            Properties.Settings.Default.CooldownEnabled = enabled;
            Properties.Settings.Default.Save();
            Cooldown.IsEnabled = enabled;
        }
    }
}