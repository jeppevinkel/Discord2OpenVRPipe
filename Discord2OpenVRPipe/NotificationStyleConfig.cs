using System;
using Newtonsoft.Json;

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
        [JsonProperty(PropertyName = "headset")]
        public bool Headset { get; set; }
        [JsonProperty(PropertyName = "horizontal")]
        public bool Horizontal { get; set; }
        [JsonProperty(PropertyName = "level")]
        public bool Level { get; set; }
        [JsonProperty(PropertyName = "channel")]
        public int Channel { get; set; }
        [JsonProperty(PropertyName = "hz")]
        public int Hz { get; set; }
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }
        [JsonProperty(PropertyName = "width")]
        public double Width { get; set; }
        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }
        [JsonProperty(PropertyName = "pitch")]
        public double Pitch { get; set; }
        [JsonProperty(PropertyName = "yaw")]
        public double Yaw { get; set; }
    }

    [Serializable]
    public class NotificationStyleConfigTransition
    {
        [JsonProperty(PropertyName = "scale")]
        public double Scale { get; set; }
        [JsonProperty(PropertyName = "opacity")]
        public double Opacity { get; set; }
        [JsonProperty(PropertyName = "vertical")]
        public double Vertical { get; set; }
        [JsonProperty(PropertyName = "horizontal")]
        public double Horizontal { get; set; }
        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }
        [JsonProperty(PropertyName = "spin")]
        public double Spin { get; set; }
        [JsonProperty(PropertyName = "tween")]
        public NotificationTween Tween { get; set; }
        [JsonProperty(PropertyName = "duration")]
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

        public static PipeNotification GetNotification(this NotificationStyleConfig style, string imageData)
        {
            return new PipeNotification
            {
                Image = imageData,
                Properties = style.Properties,
                Transition = style.TransitionIn,
                Transition2 = style.TransitionOut
            };
        }
    }

    public class PipeNotification
    {
        [JsonProperty(PropertyName = "custom")]
        public bool Custom = true;
        [JsonProperty(PropertyName = "image")]
        public string Image;
        [JsonProperty(PropertyName = "properties")]
        public NotificationStyleConfigProperties Properties;
        [JsonProperty(PropertyName = "transition")]
        public NotificationStyleConfigTransition Transition;
        [JsonProperty(PropertyName = "transition2")]
        public NotificationStyleConfigTransition Transition2;
    }
}