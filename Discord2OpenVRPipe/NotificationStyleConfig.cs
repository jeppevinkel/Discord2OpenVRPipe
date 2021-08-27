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
    }

    [Serializable]
    public class NotificationStyleConfigProperties
    {
        public bool Headset { get; set; }
        public bool Horizontal { get; set; }
        public int Channel { get; set; }
        public int Hz { get; set; }
        public int Duration { get; set; }
        public float Width { get; set; }
        public float Distance { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
    }

    [Serializable]
    public class NotificationStyleConfigTransition
    {
        public float Scale { get; set; }
        public float Opacity { get; set; }
        public float Vertical { get; set; }
        public float Horizontal { get; set; }
        public float Distance { get; set; }
        public float Spin { get; set; }
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