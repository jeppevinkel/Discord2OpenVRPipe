using System;
using System.Windows;

namespace Discord2OpenVRPipe
{
    public partial class InputDialog : Window
    {
        public enum InputType
        {
            Text,
            Integer,
            Float
        }

        public string rawValue;
        public string textValue;
        public int intValue;
        public float floatValue;
        private InputType _inputType;
        public InputDialog(string labelText, object defaultValue, InputType inputType = InputType.Text)
        {
            this.rawValue = defaultValue.ToString();
            this._inputType = inputType;
            InitializeComponent();
            inputLabel.Text = labelText;
            textBoxValue.Text = defaultValue.ToString();
            textBoxValue.Focus();
            textBoxValue.SelectAll();
        }

        public static string PromptString(string labelText, string title, string defaultValue)
        {
            var inst = new InputDialog(labelText, defaultValue, InputType.Text) {Title = title};
            inst.ShowDialog();
            return inst.DialogResult == true ? inst.textValue : null;
        }

        public static float PromptFloat(string labelText, string title, float defaultValue)
        {
            var inst = new InputDialog(labelText, defaultValue, InputType.Float) {Title = title};
            inst.ShowDialog();
            return inst.DialogResult == true ? inst.floatValue : float.NaN;
        }

        public static int PromptInt(string labelText, string title, int defaultValue)
        {
            var inst = new InputDialog(labelText, defaultValue, InputType.Integer) {Title = title};
            inst.ShowDialog();
            return inst.DialogResult == true ? inst.intValue : int.MinValue;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            switch (_inputType)
            {
                case InputType.Text:
                    rawValue = textBoxValue.Text;
                    textValue = rawValue;
                    DialogResult = true;
                    break;
                case InputType.Integer:
                    rawValue = textBoxValue.Text;
                    DialogResult = Int32.TryParse(rawValue, out intValue);
                    break;
                case InputType.Float:
                    rawValue = textBoxValue.Text;
                    DialogResult = float.TryParse(rawValue, out floatValue);
                    break;
                default:
                    DialogResult = false;
                    break;
            }
        }
    }
}