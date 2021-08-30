using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Discord2OpenVRPipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private AppController _controller;
        private Properties.Settings _settings = Properties.Settings.Default;

        public MainWindow()
        {
            InitializeComponent();

            Title = Properties.Resources.AppName;

            LoadSettings();
            
            Label_Version.Content = Properties.Resources.Version;

            _controller = new AppController(status =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (status)
                    {
                        Label_OpenVRStatus.Background = Brushes.OliveDrab;
                        Label_OpenVRStatus.Content = "Connected";
                    }
                    else
                    {
                        Label_OpenVRStatus.Background = Brushes.Tomato;
                        Label_OpenVRStatus.Content = "Disconnected";
                        
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
                });
            if (_settings.LaunchMinimized)
            {
                Hide();
                WindowState = WindowState.Minimized;
                ShowInTaskbar = !_settings.MinimizeToTray;
            }
        }
        
        private void LoadSettings()
        {
            if (_settings.NotificationStyle is null)
            {
                _settings.NotificationStyle = new NotificationStyleConfig();
            }
            
            checkBox_MinimizeOnLaunch.IsChecked = _settings.LaunchMinimized;
            checkBox_MinimizeToTray.IsChecked = _settings.MinimizeToTray;
            checkBox_ExitWithSteamVR.IsChecked = _settings.ExitWithSteam;
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

        private void CheckBox_ExitWithSteamVR_Checked(object sender, RoutedEventArgs e)
        {
            _settings.ExitWithSteam = CheckboxValue(e);
            _settings.Save();
        }

        private void ButtonBotReconnectClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonChannelEditClick(object sender, RoutedEventArgs e)
        {
            var dialog = new DiscordChannelSettings(_controller);
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                Properties.Settings.Default.DiscordServerId = dialog.SelectedGuild.Id;
                Properties.Settings.Default.DiscordChannelId = dialog.SelectedChannel.Id;
                textBoxChannel.Text = dialog.SelectedChannel.Name;
            }
        }

        private void ButtonStyleEditClick(object sender, RoutedEventArgs e)
        {
            var notif = new NotificationStyleSettings(_settings.NotificationStyle);
            notif.ShowDialog();
            var res = notif.DialogResult;
        }
    }
}