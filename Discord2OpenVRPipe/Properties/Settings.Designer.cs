using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;

namespace Discord2OpenVRPipe.Properties
{
    public class Settings : ApplicationSettingsBase
    {
        private static Settings _defaultInstance = (Settings) Synchronized(new Settings());
        public static Settings Default
        {
            get => _defaultInstance;
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Save();
            base.OnPropertyChanged(sender, e);
        }

        [UserScopedSetting]
        [DefaultSettingValueAttribute(
            @"")]
        public string BotToken
        {
            get { return (string) this[nameof(BotToken)]; }
            set { this[nameof(BotToken)] = value; }
        }
        
        [UserScopedSetting]
        [DefaultSettingValueAttribute(
            @"8077")]
        public int PipePort
        {
            get { return (int) this[nameof(PipePort)]; }
            set { this[nameof(PipePort)] = value; }
        }
        
        [UserScopedSetting]
        [DefaultSettingValueAttribute(
            @"")]
        public ulong DiscordServerId
        {
            get { return (ulong) this[nameof(DiscordServerId)]; }
            set { this[nameof(DiscordServerId)] = value; }
        }
        
        [UserScopedSetting]
        [DefaultSettingValueAttribute(
            @"")]
        public ulong DiscordChannelId
        {
            get { return (ulong) this[nameof(DiscordChannelId)]; }
            set { this[nameof(DiscordChannelId)] = value; }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool LaunchMinimized {
            get {
                return ((bool)(this[nameof(LaunchMinimized)]));
            }
            set {
                this[nameof(LaunchMinimized)] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExitWithSteam {
            get {
                return ((bool)(this[nameof(ExitWithSteam)]));
            }
            set {
                this[nameof(ExitWithSteam)] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool MinimizeToTray {
            get {
                return ((bool)(this[nameof(MinimizeToTray)]));
            }
            set {
                this[nameof(MinimizeToTray)] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public NotificationStyleConfig NotificationStyle {
            get {
                return ((NotificationStyleConfig)(this[nameof(NotificationStyle)]));
            }
            set {
                this[nameof(NotificationStyle)] = value;
            }
        }
    }
}