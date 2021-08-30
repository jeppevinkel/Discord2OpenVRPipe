using System;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Discord2OpenVRPipe.CustomControls
{
    
    [TemplatePart(Name = "PART_NumericTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public partial class NumberUpDown : UserControl
    {
        private string valueFormat;
        private double initialValue;
        
        
        public static readonly DependencyProperty SilentErrorProperty =
            DependencyProperty.Register("SilentErrorSeparator", typeof(bool), typeof(NumberUpDown), new PropertyMetadata(false));
        public bool SilentError
        {
            get { return (bool)this.GetValue(SilentErrorProperty); }
            set 
            { 
                this.SetValue(SilentErrorProperty, value); 
            }
        }
        
        public static readonly DependencyProperty AllowManualEditProperty =
            DependencyProperty.Register("AllowManualEditSeparator", typeof(bool), typeof(NumberUpDown), new PropertyMetadata(true));
        public bool AllowManualEdit
        {
            get { return (bool)this.GetValue(AllowManualEditProperty); }
            set 
            { 
                this.SetValue(AllowManualEditProperty, value); 
            }
        }
        
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(NumberUpDown), new PropertyMetadata(double.MinValue));
        public double Minimum
        {
            get { return Math.Max(double.MinValue, (double)this.GetValue(MinimumProperty)); }
            set { this.SetValue(MinimumProperty, value); }
        }
        
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(NumberUpDown), new PropertyMetadata(double.MaxValue));
        public double Maximum
        {
            get { return Math.Min(double.MaxValue, (double)this.GetValue(MaximumProperty)); }
            set { this.SetValue(MaximumProperty, value); }
        }
        
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(nameof(Increment), typeof(double), typeof(NumberUpDown), new PropertyMetadata(1.0));
        public double Increment
        {
            get { return Math.Min(double.MaxValue, Math.Abs((double)this.GetValue(IncrementProperty))); }
            set { this.SetValue(IncrementProperty, value); }
        }
        
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumberUpDown), new PropertyMetadata(0.0, OnValueChanged));
        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NumberUpDown numericBoxControl = (NumberUpDown)sender;
            numericBoxControl.OnValueChanged((double)args.OldValue, (double)args.NewValue);
        }
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueFormatProperty =
            DependencyProperty.Register(nameof(ValueFormat), typeof(string), typeof(NumberUpDown), new PropertyMetadata("0.000"));
        public string ValueFormat
        {
            get { return (string)this.GetValue(ValueFormatProperty); }
            set { this.SetValue(ValueFormatProperty, value); }
        }
        
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(ValueChanged), RoutingStrategy.Direct, typeof(RoutedPropertyChangedEventHandler<double>), typeof(NumberUpDown));
        public event RoutedPropertyChangedEventHandler<double> ValueChanged
        {
            add    { this.AddHandler   (ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }
        private void OnValueChanged(double oldValue, double newValue)
        {
            RoutedPropertyChangedEventArgs<double> args = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue);
            args.RoutedEvent = NumberUpDown.ValueChangedEvent;
            this.RaiseEvent(args);
        }
        
        public NumberUpDown()
        {
            InitializeComponent();
            PART_NumericTextBox.Text = Value.ToString(ValueFormat);
        }

        private void increaseBtn_Click(object sender, RoutedEventArgs e)
        {
            IncreaseValue();
        }

        private void decreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            DecreaseValue();
        }

        private void numericBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            int caretIndex = textbox.CaretIndex;
            try
            {
                bool isError = false;
                string text = this.PART_NumericTextBox.Text.Insert(caretIndex, e.Text);
                double value;
                isError = (!double.TryParse(text, out value));
                isError |= (value < this.Minimum || value > this.Maximum);
                if (isError)
                {
                    if (!this.SilentError)
                    {
                        SystemSounds.Hand.Play();
                    }
                    e.Handled = true;
                }
            }
            catch (FormatException)
            {
            }
        }
        
        private void PART_NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = this.PART_NumericTextBox.Text;
            // if the text is empty, use the initial Value
            if (string.IsNullOrEmpty(text))
            {
                text = this.initialValue.ToString(this.valueFormat);
                this.PART_NumericTextBox.Text = text;
            }
            // personal discovery - int.Parse won't parse negative numbers unless you use the 
            // numberstyles indicated here. There are a bunch of interesting number styles 
            // available, and you can even parse the numeric values in a mixed-character string. 
            // Interesting stuff.
            this.Value = (text == "-") ? 0 : double.Parse(text);
        }

        private void numericBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Debug.WriteLine(e.Delta);
            if (e.Delta > 0)
            {
                IncreaseValue();
            }
            else
            {
                DecreaseValue();
            }
        }
        
        private void IncreaseValue()
        {
            this.Value = Math.Min(this.Maximum, this.Value + this.Increment);
            this.PART_NumericTextBox.Text = this.Value.ToString(this.valueFormat);
        }
        
        private void DecreaseValue()
        {
            this.Value = Math.Max(this.Minimum, this.Value - this.Increment);
            this.PART_NumericTextBox.Text = this.Value.ToString(this.valueFormat);
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_IncreaseButton.Click += increaseBtn_Click;
            this.PART_DecreaseButton.Click += decreaseBtn_Click;

            if (this.AllowManualEdit)
            {
                this.PART_NumericTextBox.PreviewTextInput += numericBox_PreviewTextInput;
                this.PART_NumericTextBox.TextChanged += this.PART_NumericTextBox_TextChanged;
            }

            this.PART_NumericTextBox.IsReadOnly = !this.AllowManualEdit;
            this.PART_NumericTextBox.MouseWheel += numericBox_MouseWheel;


            this.valueFormat = "0.000"; 
            this.initialValue = this.Value;
            this.PART_NumericTextBox.Text = this.Value.ToString(this.valueFormat);
        }
    }
}