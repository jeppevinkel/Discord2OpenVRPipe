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
        private NotificationTween _selectedTweenOut = NotificationTween.Linear;
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

        public NotificationTween SelectedTweenOut
        {
            get
            {
                return _selectedTweenOut;
            }
            set
            {
                _selectedTweenOut = value;
            }
        }

        private int _selectedTweenInIndex = 0;
        private int _selectedTweenOutIndex = 0;
        
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


        private List<ComboTween> _tweenList;

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

        public NotificationStyleSettings(NotificationStyleConfig curConf, AppController appController)
        {
            InitializeComponent();
            TweenList = ((NotificationTween[])Enum.GetValues(typeof(NotificationTween))).Select(t => new ComboTween(t, NotificationExtensions.NotificationTweenTooltip(t))).ToList();
            _appController = appController;
            
            _newConf = curConf.Clone();

            // Initialize values
            
            CheckBoxFixedToHeadset.IsChecked = _newConf.Properties.Headset;
            CheckBoxFixedToHeadset.Checked += (sender, args) => _newConf.Properties.Headset = true;
            CheckBoxFixedToHeadset.Unchecked += (sender, args) => _newConf.Properties.Headset = false;
            CheckBoxAlignHorizontal.IsChecked = _newConf.Properties.Horizontal;
            CheckBoxAlignHorizontal.Checked += (sender, args) => _newConf.Properties.Horizontal = true;
            CheckBoxAlignHorizontal.Unchecked += (sender, args) => _newConf.Properties.Horizontal = false;
            CheckBoxLevel.IsChecked = _newConf.Properties.Level;
            CheckBoxLevel.Checked += (sender, args) => _newConf.Properties.Level = true;
            CheckBoxLevel.Unchecked += (sender, args) => _newConf.Properties.Level = false;

            PropertiesChannel.Value = _newConf.Properties.Channel;
            PropertiesChannel.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.Channel = args.NewValue;
            };
            PropertiesDistance.Value = _newConf.Properties.Distance;
            PropertiesDistance.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.Distance = args.NewValue;
            };
            PropertiesDuration.Value = _newConf.Properties.Duration;
            PropertiesDuration.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.Duration = args.NewValue;
            };
            PropertiesHz.Value = _newConf.Properties.Hz;
            PropertiesHz.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.Hz = args.NewValue;
            };
            PropertiesPitch.Value = _newConf.Properties.Pitch;
            PropertiesPitch.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.Pitch = args.NewValue;
            };
            PropertiesWidth.Value = _newConf.Properties.Width;
            PropertiesWidth.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.Width = args.NewValue;
            };
            PropertiesYaw.Value = _newConf.Properties.Yaw;
            PropertiesYaw.ValueChanged += (sender, args) =>
            {
                _newConf.Properties.Yaw = args.NewValue;
            };

            TransitionInDistance.Value = _newConf.TransitionIn.Distance;
            TransitionInDistance.ValueChanged += (sender, args) => _newConf.TransitionIn.Distance = args.NewValue;
            TransitionInDuration.Value = _newConf.TransitionIn.Duration;
            TransitionInDuration.ValueChanged += (sender, args) => _newConf.TransitionIn.Duration = args.NewValue;
            TransitionInHorizontal.Value = _newConf.TransitionIn.Horizontal;
            TransitionInHorizontal.ValueChanged += (sender, args) => _newConf.TransitionIn.Horizontal = args.NewValue;
            TransitionInOpacity.Value = _newConf.TransitionIn.Opacity;
            TransitionInOpacity.ValueChanged += (sender, args) => _newConf.TransitionIn.Opacity = args.NewValue;
            TransitionInScale.Value = _newConf.TransitionIn.Scale;
            TransitionInScale.ValueChanged += (sender, args) => _newConf.TransitionIn.Scale = args.NewValue;
            TransitionInSpin.Value = _newConf.TransitionIn.Spin;
            TransitionInSpin.ValueChanged += (sender, args) => _newConf.TransitionIn.Spin = args.NewValue;
            TransitionInVertical.Value = _newConf.TransitionIn.Vertical;
            TransitionInVertical.ValueChanged += (sender, args) => _newConf.TransitionIn.Vertical = args.NewValue;

            TransitionOutDistance.Value = _newConf.TransitionOut.Distance;
            TransitionOutDistance.ValueChanged += (sender, args) => _newConf.TransitionOut.Distance = args.NewValue;
            TransitionOutDuration.Value = _newConf.TransitionOut.Duration;
            TransitionOutDuration.ValueChanged += (sender, args) => _newConf.TransitionOut.Duration = args.NewValue;
            TransitionOutHorizontal.Value = _newConf.TransitionOut.Horizontal;
            TransitionOutHorizontal.ValueChanged += (sender, args) => _newConf.TransitionOut.Horizontal = args.NewValue;
            TransitionOutOpacity.Value = _newConf.TransitionOut.Opacity;
            TransitionOutOpacity.ValueChanged += (sender, args) => _newConf.TransitionOut.Opacity = args.NewValue;
            TransitionOutScale.Value = _newConf.TransitionOut.Scale;
            TransitionOutScale.ValueChanged += (sender, args) => _newConf.TransitionOut.Scale = args.NewValue;
            TransitionOutSpin.Value = _newConf.TransitionOut.Spin;
            TransitionOutSpin.ValueChanged += (sender, args) => _newConf.TransitionOut.Spin = args.NewValue;
            TransitionOutVertical.Value = _newConf.TransitionOut.Vertical;
            TransitionOutVertical.ValueChanged += (sender, args) => _newConf.TransitionOut.Vertical = args.NewValue;
            
            TweenInSelect.ItemsSource = TweenList;
            TweenInSelect.SelectedValue = _newConf.TransitionIn.Tween;
            TweenInSelect.SelectedIndex = (int)_newConf.TransitionIn.Tween;
            
            TweenOutSelect.ItemsSource = TweenList;
            TweenOutSelect.SelectedValue = _newConf.TransitionOut.Tween;
            TweenOutSelect.SelectedIndex = (int)_newConf.TransitionOut.Tween;

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

            _newConf.TransitionIn.Tween = SelectedTweenIn;
        }

        private void TweenOutSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (ComboBox)sender;

            SelectedTweenOut = (NotificationTween)box.SelectedValue;
            SelectedTweenOutIndex = box.SelectedIndex;

            _newConf.TransitionOut.Tween = SelectedTweenOut;
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
}