using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Discord2OpenVRPipe
{
    public partial class NotificationStyleSettings : Window
    {
        private NotificationTween _selectedTweenIn = NotificationTween.Linear;
        private NotificationTween _selectedTweenOut = NotificationTween.Linear;

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

        public NotificationStyleSettings()
        {
            InitializeComponent();
            TweenList = ((NotificationTween[])Enum.GetValues(typeof(NotificationTween))).Select(t => new ComboTween(t, NotificationExtensions.NotificationTweenTooltip(t))).ToList();
            
            TweenInSelect.ItemsSource = TweenList;
            TweenInSelect.SelectedValue = SelectedTweenIn;
            TweenInSelect.SelectedIndex = SelectedTweenInIndex;
            
            TweenOutSelect.ItemsSource = TweenList;
            TweenOutSelect.SelectedValue = SelectedTweenOut;
            TweenOutSelect.SelectedIndex = SelectedTweenOutIndex;
        }

        private void TweenInSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (ComboBox)sender;

            SelectedTweenIn = (NotificationTween)box.SelectedValue;
            SelectedTweenInIndex = box.SelectedIndex;
        }

        private void TweenOutSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (ComboBox)sender;

            SelectedTweenOut = (NotificationTween)box.SelectedValue;
            SelectedTweenOutIndex = box.SelectedIndex;
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