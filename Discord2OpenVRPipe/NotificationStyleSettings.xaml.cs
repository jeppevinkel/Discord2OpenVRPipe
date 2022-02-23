using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace Discord2OpenVRPipe
{
    public partial class NotificationStyleSettings : Window
    {
        private NotificationTween _selectedTweenIn = NotificationTween.Linear;
        private AppController _appController;

        private Color watermarkColor = Colors.Black;
        private SolidColorBrush watermarkColorBrush;

        private NotificationStyleConfig _newConf;

        public NotificationTween SelectedTweenIn
        {
            get
            {
                return _selectedTweenIn;
            }
            set
            {
                _selectedTweenIn = value;
            }
        }

        public NotificationTween SelectedTweenOut { get; set; } = NotificationTween.Linear;

        public NotificationAnchorType SelectedAnchorType { get; set; } = NotificationAnchorType.Head;

        private int _selectedTweenInIndex = 0;
        private int _selectedTweenOutIndex = 0;
        private int _selectedAnchorTypeIndex = 0;
        
        public int SelectedTweenInIndex
        {
            get
            {
                return _selectedTweenInIndex;
            }
            set
            {
                _selectedTweenInIndex = value;
            }
        }
        
        public int SelectedTweenOutIndex
        {
            get
            {
                return _selectedTweenOutIndex;
            }
            set
            {
                _selectedTweenOutIndex = value;
            }
        }
        
        public int SelectedAnchorTypeIndex
        {
            get
            {
                return _selectedAnchorTypeIndex;
            }
            set
            {
                _selectedAnchorTypeIndex = value;
            }
        }


        private List<ComboTween> _tweenList;
        private List<ComboAnchorType> _anchorTypeList;

        public List<ComboTween> TweenList
        {
            get
            {
                return _tweenList;
            }
            set
            {
                _tweenList = value;
            }
        }
        
        public List<ComboAnchorType> AnchorTypeList
        {
            get
            {
                return _anchorTypeList;
            }
            set
            {
                _anchorTypeList = value;
            }
        }

        public NotificationStyleSettings(NotificationStyleConfig curConf, AppController appController)
        {
            InitializeComponent();
            TweenList = ((NotificationTween[])Enum.GetValues(typeof(NotificationTween))).Select(t => new ComboTween(t, NotificationExtensions.NotificationTweenTooltip(t))).ToList();
            AnchorTypeList = ((NotificationAnchorType[])Enum.GetValues(typeof(NotificationAnchorType))).Select(t => new ComboAnchorType(t, NotificationExtensions.NotificationAnchorTypeTooltip(t))).ToList();
            
            _appController = appController;
            
            _newConf = curConf.Clone();

            // Initialize values

            CheckBoxAttachToAnchor.IsChecked = _newConf.Properties.AttachToAnchor;
            CheckBoxAttachToAnchor.Checked += (sender, args) => _newConf.Properties.AttachToAnchor = true;
            CheckBoxAttachToAnchor.Unchecked += (sender, args) => _newConf.Properties.AttachToAnchor = false;
            CheckBoxAlignHorizontal.IsChecked = _newConf.Properties.AlignToHorizon;
            CheckBoxAlignHorizontal.Checked += (sender, args) => _newConf.Properties.AlignToHorizon = true;
            CheckBoxAlignHorizontal.Unchecked += (sender, args) => _newConf.Properties.AlignToHorizon = false;
            CheckBoxAttachToHorizon.IsChecked = _newConf.Properties.AttachToHorizon;
            CheckBoxAttachToHorizon.Checked += (sender, args) => _newConf.Properties.AttachToHorizon = true;
            CheckBoxAttachToHorizon.Unchecked += (sender, args) => _newConf.Properties.AttachToHorizon = false;

            PropertiesChannel.Value = _newConf.Properties.OverlayChannel;
            PropertiesChannel.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.OverlayChannel = args.NewValue;
            };
            PropertiesDistance.Value = _newConf.Properties.DistanceZ;
            PropertiesDistance.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.DistanceZ = args.NewValue;
            };
            PropertiesDuration.Value = _newConf.Properties.DurationMs;
            PropertiesDuration.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.DurationMs = args.NewValue;
            };
            PropertiesHz.Value = _newConf.Properties.AnimationHz;
            PropertiesHz.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.AnimationHz = args.NewValue;
            };
            PropertiesPitch.Value = _newConf.Properties.PitchDeg;
            PropertiesPitch.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.PitchDeg = args.NewValue;
            };
            PropertiesWidth.Value = _newConf.Properties.WidthM;
            PropertiesWidth.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.WidthM = args.NewValue;
            };
            PropertiesYaw.Value = _newConf.Properties.YawDeg;
            PropertiesYaw.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.YawDeg = args.NewValue;
            };

            TransitionInDistance.Value = _newConf.Properties.Transitions[0].Distance;
            TransitionInDistance.ValueChanged += (sender, args) => _newConf.Properties.Transitions[0].Distance = args.NewValue;
            TransitionInDuration.Value = _newConf.Properties.Transitions[0].Duration;
            TransitionInDuration.ValueChanged += (sender, args) => _newConf.Properties.Transitions[0].Duration = args.NewValue;
            TransitionInHorizontal.Value = _newConf.Properties.Transitions[0].Horizontal;
            TransitionInHorizontal.ValueChanged += (sender, args) => _newConf.Properties.Transitions[0].Horizontal = args.NewValue;
            TransitionInOpacity.Value = _newConf.Properties.Transitions[0].Opacity;
            TransitionInOpacity.ValueChanged += (sender, args) => _newConf.Properties.Transitions[0].Opacity = args.NewValue;
            TransitionInScale.Value = _newConf.Properties.Transitions[0].Scale;
            TransitionInScale.ValueChanged += (sender, args) => _newConf.Properties.Transitions[0].Scale = args.NewValue;
            TransitionInSpin.Value = _newConf.Properties.Transitions[0].Spin;
            TransitionInSpin.ValueChanged += (sender, args) => _newConf.Properties.Transitions[0].Spin = args.NewValue;
            TransitionInVertical.Value = _newConf.Properties.Transitions[0].Vertical;
            TransitionInVertical.ValueChanged += (sender, args) => _newConf.Properties.Transitions[0].Vertical = args.NewValue;

            TransitionOutDistance.Value = _newConf.Properties.Transitions[1].Distance;
            TransitionOutDistance.ValueChanged += (sender, args) => _newConf.Properties.Transitions[1].Distance = args.NewValue;
            TransitionOutDuration.Value = _newConf.Properties.Transitions[1].Duration;
            TransitionOutDuration.ValueChanged += (sender, args) => _newConf.Properties.Transitions[1].Duration = args.NewValue;
            TransitionOutHorizontal.Value = _newConf.Properties.Transitions[1].Horizontal;
            TransitionOutHorizontal.ValueChanged += (sender, args) => _newConf.Properties.Transitions[1].Horizontal = args.NewValue;
            TransitionOutOpacity.Value = _newConf.Properties.Transitions[1].Opacity;
            TransitionOutOpacity.ValueChanged += (sender, args) => _newConf.Properties.Transitions[1].Opacity = args.NewValue;
            TransitionOutScale.Value = _newConf.Properties.Transitions[1].Scale;
            TransitionOutScale.ValueChanged += (sender, args) => _newConf.Properties.Transitions[1].Scale = args.NewValue;
            TransitionOutSpin.Value = _newConf.Properties.Transitions[1].Spin;
            TransitionOutSpin.ValueChanged += (sender, args) => _newConf.Properties.Transitions[1].Spin = args.NewValue;
            TransitionOutVertical.Value = _newConf.Properties.Transitions[1].Vertical;
            TransitionOutVertical.ValueChanged += (sender, args) => _newConf.Properties.Transitions[1].Vertical = args.NewValue;
            
            TweenInSelect.ItemsSource = TweenList;
            TweenInSelect.SelectedValue = _newConf.Properties.Transitions[0].Tween;
            TweenInSelect.SelectedIndex = (int)_newConf.Properties.Transitions[0].Tween;
            
            TweenOutSelect.ItemsSource = TweenList;
            TweenOutSelect.SelectedValue = _newConf.Properties.Transitions[1].Tween;
            TweenOutSelect.SelectedIndex = (int)_newConf.Properties.Transitions[1].Tween;

            AnchorTypeSelect.ItemsSource = AnchorTypeList;
            AnchorTypeSelect.SelectedValue = _newConf.Properties.AnchorType;
            AnchorTypeSelect.SelectedIndex = (int)_newConf.Properties.AnchorType;

            watermarkColor = Properties.Settings.Default.WatermarkColor;
            watermarkEnabledCheckBox.IsChecked = Properties.Settings.Default.WatermarkImages;

            watermarkColorBrush = new SolidColorBrush(watermarkColor);
            watermarkColorRec.Fill = watermarkColorBrush;

            redSelect.Value = watermarkColor.R;
            greenSelect.Value = watermarkColor.G;
            blueSelect.Value = watermarkColor.B;

            redSelect.ValueChanged += (sender, args) =>
            {
                watermarkColor.R = (byte)args.NewValue;
                watermarkColorBrush.Color = watermarkColor;
            };

            greenSelect.ValueChanged += (sender, args) =>
            {
                watermarkColor.G = (byte) args.NewValue;
                watermarkColorBrush.Color = watermarkColor;
            };

            blueSelect.ValueChanged += (sender, args) =>
            {
                watermarkColor.B = (byte) args.NewValue;
                watermarkColorBrush.Color = watermarkColor;
            };
        }

        private void TweenInSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (ComboBox)sender;

            SelectedTweenIn = (NotificationTween)box.SelectedValue;
            SelectedTweenInIndex = box.SelectedIndex;

            _newConf.Properties.Transitions[0].Tween = SelectedTweenIn;
        }

        private void TweenOutSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (ComboBox)sender;

            SelectedTweenOut = (NotificationTween)box.SelectedValue;
            SelectedTweenOutIndex = box.SelectedIndex;

            _newConf.Properties.Transitions[1].Tween = SelectedTweenOut;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.NotificationStyle = _newConf;
            Properties.Settings.Default.WatermarkColor = watermarkColor;
            Properties.Settings.Default.WatermarkImages = watermarkEnabledCheckBox.IsChecked ?? false;
            DialogResult = true;
        }

        private void TestButtonClick(object sender, RoutedEventArgs e)
        {
            _appController.TestPipe(_newConf);
        }

        private void AnchorTypeSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (ComboBox)sender;

            SelectedAnchorType = (NotificationAnchorType)box.SelectedValue;
            SelectedAnchorTypeIndex = box.SelectedIndex;

            _newConf.Properties.AnchorType = SelectedAnchorType;
        }
    }
    
    public class ComboTween
    {
        public NotificationTween TweenValue { get; set; }
        public string Name { get; set; }
        public string Tooltip { get; set; }

        public ComboTween(NotificationTween tween, string tooltip)
        {
            TweenValue = tween;
            Name = tween.ToString();
            Tooltip = tooltip;
        }
    }
    
    public class ComboAnchorType
    {
        public NotificationAnchorType AnchorTypeValue { get; set; }
        public string Name { get; set; }
        public string Tooltip { get; set; }

        public ComboAnchorType(NotificationAnchorType anchorType, string tooltip)
        {
            AnchorTypeValue = anchorType;
            Name = anchorType.ToString();
            Tooltip = tooltip;
        }
    }
}