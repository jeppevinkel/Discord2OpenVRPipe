using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Discord2OpenVRPipe
{
    [Serializable]
    public class NotificationStyleConfig
    {
        public NotificationStyleConfigProperties Properties { get; set; }

        public NotificationStyleConfig()
        {
            Properties = new NotificationStyleConfigProperties();
        }

        public NotificationStyleConfig Clone()
        {
            return new NotificationStyleConfig
            {
                Properties = new NotificationStyleConfigProperties
                {
                    Enabled = this.Properties.Enabled,
                    Transitions = this.Properties.Transitions,
                    AnchorType = this.Properties.AnchorType,
                    AnimationHz = this.Properties.AnimationHz,
                    DistanceX =  this.Properties.DistanceX,
                    DistanceY =  this.Properties.DistanceY,
                    DistanceZ =  this.Properties.DistanceZ,
                    DurationMs =  this.Properties.DurationMs,
                    OverlayChannel =  this.Properties.OverlayChannel,
                    PitchDeg = this.Properties.PitchDeg,
                    RollDeg = this.Properties.RollDeg,
                    WidthM = this.Properties.WidthM,
                    YawDeg = this.Properties.YawDeg,
                    AlignToHorizon = this.Properties.AlignToHorizon,
                    AttachToAnchor = this.Properties.AttachToAnchor,
                    AttachToHorizon = this.Properties.AttachToHorizon,
                }
            };
        }
    }

    [Serializable]
    public class NotificationStyleConfigProperties
    {
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; } = true;
        [JsonProperty(PropertyName = "anchorType")]
        public NotificationAnchorType AnchorType { get; set; } = NotificationAnchorType.Head;
        [JsonProperty(PropertyName = "attachToAnchor")]
        public bool AttachToAnchor { get; set; } = false;
        [JsonProperty(PropertyName = "attachToHorizon")]
        public bool AttachToHorizon { get; set; } = false;
        [JsonProperty(PropertyName = "alignToHorizon")]
        public bool AlignToHorizon { get; set; } = false;
        [JsonProperty(PropertyName = "overlayChannel")]
        public int OverlayChannel { get; set; } = 0;
        [JsonProperty(PropertyName = "animationHz")]
        public int AnimationHz { get; set; } = -1;
        [JsonProperty(PropertyName = "durationMs")]
        public int DurationMs { get; set; } = 5000;
        [JsonProperty(PropertyName = "widthM")]
        public double WidthM { get; set; } = 1;
        [JsonProperty(PropertyName = "zDistanceM")]
        public double DistanceZ { get; set; } = 1;
        [JsonProperty(PropertyName = "yDistanceM")]
        public double DistanceY { get; set; } = 0;
        [JsonProperty(PropertyName = "xDistanceM")]
        public double DistanceX { get; set; } = 0;
        [JsonProperty(PropertyName = "yawDeg")]
        public double YawDeg { get; set; } = 0;
        [JsonProperty(PropertyName = "pitchDeg")]
        public double PitchDeg { get; set; } = 0;
        [JsonProperty(PropertyName = "rollDeg")]
        public double RollDeg { get; set; } = 0;
        [JsonProperty(PropertyName = "transitions")]
        public NotificationStyleConfigTransition[] Transitions { get; set; } = new NotificationStyleConfigTransition[2]
        {
            new NotificationStyleConfigTransition(),
            new NotificationStyleConfigTransition()
        };
        [JsonProperty(PropertyName = "textAreas")]
        public List<NotificationStyleConfigTextArea> TextAreas { get; set; } = new();
    }

    [Serializable]
    public class NotificationStyleConfigTextArea
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "xPositionPx")]
        public int XPositionPx { get; set; } = 0;
        [JsonProperty(PropertyName = "yPositionPx")]
        public int YPositionPx { get; set; } = 0;
        [JsonProperty(PropertyName = "widthPx")]
        public int WidthPx { get; set; } = 0;
        [JsonProperty(PropertyName = "heightPx")]
        public int HeightPx { get; set; } = 0;
        [JsonProperty(PropertyName = "fontSizePt")]
        public int FontSizePt { get; set; } = 10;
        [JsonProperty(PropertyName = "fontFamily")]
        public string FontFamily { get; set; } = "Arial";
        [JsonProperty(PropertyName = "fontColor")]
        public string FontColor { get; set; } = "#004080";
        [JsonProperty(PropertyName = "horizontalAlignment")]
        public NotificationHorizontalAlignment HorizontalAlignment { get; set; } = NotificationHorizontalAlignment.Near;
        [JsonProperty(PropertyName = "verticalAlignment")]
        public NotificationVerticalAlignment VerticalAlignment { get; set; } = NotificationVerticalAlignment.Near;
    }

    [Serializable]
    public class NotificationStyleConfigTransition
    {
        [JsonProperty(PropertyName = "scalePer")]
        public double Scale { get; set; } = 1;

        [JsonProperty(PropertyName = "opacityPer")]
        public double Opacity { get; set; } = 0;

        [JsonProperty(PropertyName = "yDistanceM")]
        public double Vertical { get; set; } = 0;

        [JsonProperty(PropertyName = "zDistanceM")]
        public double Horizontal { get; set; } = 0;

        [JsonProperty(PropertyName = "xDistanceM")]
        public double Distance { get; set; } = 0;

        [JsonProperty(PropertyName = "rollDeg")]
        public double Spin { get; set; } = 0;

        [JsonProperty(PropertyName = "tweenType")]
        public NotificationTween Tween { get; set; } = NotificationTween.Linear;

        [JsonProperty(PropertyName = "durationMs")]
        public int Duration { get; set; } = 250;
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

    [Serializable]
    public enum NotificationAnchorType
    {
        World,
        Head,
        LeftHand,
        RightHand,
    }
    
    [Serializable]
    public enum NotificationHorizontalAlignment
    {
        Near,
        Center,
        Far,
    }
    
    [Serializable]
    public enum NotificationVerticalAlignment
    {
        Near,
        Center,
        Far,
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

        public static string NotificationAnchorTypeTooltip(this NotificationAnchorType anchorType)
        {
            return anchorType switch
            {
                NotificationAnchorType.World => "The notification is anchored to the world origin",
                NotificationAnchorType.Head => "The notification is anchored to the user's head",
                NotificationAnchorType.LeftHand => "The notification is anchored to the user's left hand",
                NotificationAnchorType.RightHand => "The notification is anchored to the user's right hand",
                _ => throw new ArgumentOutOfRangeException(nameof(anchorType), anchorType, null)
            };
        }

        public static PipeNotification GetNotification(this NotificationStyleConfig style, string imageData)
        {
            return new PipeNotification
            {
                ImageData = imageData,
                Properties = style.Properties,
            };
        }

        public static PipeNotification WriteText(this PipeNotification notification, string text, Rect textArea,
            string fontFamily = "Arial", int fontSizePt = 10, string fontColor = "#ffffff",
            NotificationHorizontalAlignment horizontalAlignment = NotificationHorizontalAlignment.Near,
            NotificationVerticalAlignment verticalAlignment = NotificationVerticalAlignment.Near) 
        {
            notification.Properties.TextAreas.Add(new NotificationStyleConfigTextArea()
            {
                Text = text,
                WidthPx = (int)textArea.Width,
                HeightPx = (int)textArea.Height,
                FontFamily = fontFamily,
                FontSizePt = fontSizePt,
                FontColor = fontColor,
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = verticalAlignment,
                XPositionPx = (int)textArea.X,
                YPositionPx = (int)textArea.Y,
            });
            return notification;
        }

        public static PipeNotification WriteText(this PipeNotification notification, string text, Rect textArea,
            string fontFamily = "Arial", int fontSizePt = 10, Color fontColor = new Color(),
            NotificationHorizontalAlignment horizontalAlignment = NotificationHorizontalAlignment.Near,
            NotificationVerticalAlignment verticalAlignment = NotificationVerticalAlignment.Near)
        {
            return notification.WriteText(text, textArea, fontFamily, fontSizePt, fontColor.ToString(),
                horizontalAlignment, verticalAlignment);
        }
    }

    public class PipeNotification
    {
        [JsonProperty(PropertyName = "imageData")]
        public string ImageData;
        [JsonProperty(PropertyName = "customProperties")]
        public NotificationStyleConfigProperties Properties;
    }
}