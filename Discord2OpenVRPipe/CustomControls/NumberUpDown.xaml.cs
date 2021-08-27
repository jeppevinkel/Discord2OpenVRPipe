using System;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Discord2OpenVRPipe.CustomControls
{
    public partial class NumberUpDown : UserControl
    {
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(NumberUpDown), new PropertyMetadata(0.0));
        public double Minimum
        {
            get { return (double)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }
        
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(NumberUpDown), new PropertyMetadata(100.0));
        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }
        
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register(nameof(Increment), typeof(double), typeof(NumberUpDown), new PropertyMetadata(1.0));
        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }
        
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumberUpDown), new PropertyMetadata(0.0));
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                Debug.WriteLine(value);
                PART_NumericTextBox.Text = value.ToString(ValueFormat);
            }
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
                double newvalue;
                // see if the text will parse to a float
                bool error = !double.TryParse(e.Text, out newvalue);
                string text = textbox.Text;
                if (!error)
                {
                    // we have a valid float, so insert the new text at the 
                    //caret's position 
                    text = text.Insert(textbox.CaretIndex, e.Text);
                    // check the string again to make sure it still parses
                    error = !double.TryParse(text, out newvalue);
                    if (!error)
                    {
                        // we're good, so make sure the value is in the 
                        // specified min/max range
                        error = (newvalue < this.Minimum || newvalue > this.Maximum);
                    }
                }
                if (error)
                {
                    // play the error sound
                    SystemSounds.Hand.Play();
                    // reset the caret index to where it was when we entered 
                    // this method
                    textbox.CaretIndex = caretIndex;
                }
                else
                {
                    // set the textbox text (this will set the caret index to 0)
                    this.PART_NumericTextBox.Text = text;
                    // put the caret at the END of the inserted text
                    textbox.CaretIndex = caretIndex+e.Text.Length;
                    // set the Value to the new value 
                    this.Value = newvalue;
                }
            }
            catch (FormatException)
            {
            }
            e.Handled = true;
        }

        private void numericBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
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
            Debug.WriteLine($"i[{Increment}] max[{Maximum}] val[{Value}] {Math.Min(this.Maximum, this.Value + this.Increment)}");
            Value = Math.Min(this.Maximum, this.Value + this.Increment);
        }
        
        private void DecreaseValue()
        {
            Debug.WriteLine($"i[{Increment}] min[{Minimum}] val[{Value}] {Math.Max(this.Minimum, this.Value - this.Increment)}");
            Value = Math.Max(this.Minimum, this.Value - this.Increment);
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RepeatButton btn = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
            if (btn != null)
            {
                btn.Click += increaseBtn_Click;
            }

            btn = GetTemplateChild("PART_DecreaseButton") as RepeatButton;
            if (btn != null)
            {
                btn.Click += decreaseBtn_Click;
            }

            TextBox tb = GetTemplateChild("PART_NumericTextBox") as TextBox;
            if (tb != null)
            {
                PART_NumericTextBox = tb;
                PART_NumericTextBox.Text = Value.ToString(ValueFormat);
                PART_NumericTextBox.PreviewTextInput += numericBox_PreviewTextInput;
                PART_NumericTextBox.MouseWheel += numericBox_MouseWheel;
            }

            btn = null;
            tb = null;
        }
    }
}