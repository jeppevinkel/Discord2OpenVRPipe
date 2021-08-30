using System;

namespace Discord2OpenVRPipe
{
    [Serializable]
    public class NotificationStyleConfig
    {
        public NotificationStyleConfigProperties Properties { get; set; }
        public NotificationStyleConfigTransition TransitionIn { get; set; }
        public NotificationStyleConfigTransition TransitionOut { get; set; }

        public NotificationStyleConfig()
        {
            Properties = new NotificationStyleConfigProperties
            {
                Headset = false,
                Horizontal = true,
                Channel = 0,
                Hz = -1,
                Duration = 1000,
                Width = 1,
                Distance = 1,
                Pitch = 0,
                Yaw = 0
            };
            TransitionIn = new NotificationStyleConfigTransition
            {
                Scale = 1,
                Opacity = 0,
                Vertical = 0,
                Horizontal = 0,
                Distance = 0,
                Spin = 0,
                Tween = NotificationTween.Linear,
                Duration = 100
            };
            TransitionOut = new NotificationStyleConfigTransition
            {
                Scale = 1,
                Opacity = 0,
                Vertical = 0,
                Horizontal = 0,
                Distance = 0,
                Spin = 0,
                Tween = NotificationTween.Linear,
                Duration = 100
            };
        }

        public NotificationStyleConfig Clone()
        {
            return new NotificationStyleConfig
            {
                Properties = new NotificationStyleConfigProperties
                {
                    Channel = this.Properties.Channel,
                    Distance = this.Properties.Distance,
                    Duration = this.Properties.Duration,
                    Headset = this.Properties.Headset,
                    Horizontal = this.Properties.Horizontal,
                    Hz = this.Properties.Hz,
                    Level = this.Properties.Level,
                    Pitch = this.Properties.Pitch,
                    Width = this.Properties.Width,
                    Yaw = this.Properties.Yaw
                },
                TransitionIn = new NotificationStyleConfigTransition
                {
                    Distance = this.TransitionIn.Distance,
                    Duration = this.TransitionIn.Duration,
                    Horizontal = this.TransitionIn.Horizontal,
                    Opacity = this.TransitionIn.Opacity,
                    Scale = this.TransitionIn.Scale,
                    Spin = this.TransitionIn.Spin,
                    Tween = this.TransitionIn.Tween,
                    Vertical = this.TransitionIn.Vertical
                },
                TransitionOut = new NotificationStyleConfigTransition
                {
                    Distance = this.TransitionOut.Distance,
                    Duration = this.TransitionOut.Duration,
                    Horizontal = this.TransitionOut.Horizontal,
                    Opacity = this.TransitionOut.Opacity,
                    Scale = this.TransitionOut.Scale,
                    Spin = this.TransitionOut.Spin,
                    Tween = this.TransitionOut.Tween,
                    Vertical = this.TransitionOut.Vertical
                }
            };
        }
    }

    [Serializable]
    public class NotificationStyleConfigProperties
    {
        public bool Headset { get; set; }
        public bool Horizontal { get; set; }
        public bool Level { get; set; }
        public int Channel { get; set; }
        public int Hz { get; set; }
        public int Duration { get; set; }
        public double Width { get; set; }
        public double Distance { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }
    }

    [Serializable]
    public class NotificationStyleConfigTransition
    {
        public double Scale { get; set; }
        public double Opacity { get; set; }
        public double Vertical { get; set; }
        public double Horizontal { get; set; }
        public double Distance { get; set; }
        public double Spin { get; set; }
        public NotificationTween Tween { get; set; }
        public int Duration { get; set; }
    }
    
    [Serializable]
    public enum NotificationTween
    {
        Linear,
        Sine,
        Quadratic,
        Cubic,
        Quartic,
        Quintic,
        Circle,
        Back,
        Elastic,
        Bounce
    }

    public static class NotificationExtensions
    {
        public static string NotificationTweenTooltip(this NotificationTween tween)
        {
            return tween switch
            {
                NotificationTween.Linear => "Default unmodified, hard stop",
                NotificationTween.Sine => "Based on the sine curve, soft stop",
                NotificationTween.Quadratic => "^2, soft stop",
                NotificationTween.Cubic => "^3, soft stop",
                NotificationTween.Quartic => "^4, soft stop",
                NotificationTween.Quintic => "^5, soft stop",
                NotificationTween.Circle => "Based on a circle, soft stop",
                NotificationTween.Back => "Overshoots but comes back, soft stop",
                NotificationTween.Elastic => "Jiggling, soft stop",
                NotificationTween.Bounce => "Multiple bounces, hard stop",
                _ => throw new ArgumentOutOfRangeException(nameof(tween), tween, null)
            };
        }
    }
}