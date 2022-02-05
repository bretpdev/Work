using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class WatermarkCurrencyTextBox : WatermarkTextBox
    {
        public string AllowedSpecialCharacters { get; set; }

        public WatermarkCurrencyTextBox()
        {
            this.KeyPress += KeyPressHandler;
            AllowedSpecialCharacters = string.Empty;
        }

        public override ValidationResults Validate()
        {
            if (NumericTextBox.ValidateNumericInput(this.Text))
                return new ValidationResults(true);
            else
                return new ValidationResults(false, "Please enter only numbers");
        }

        public void KeyPressHandler(object sender, KeyPressEventArgs e)
        {
            bool isCommand = IsCommand(e);
            bool isValid = ValidateNumericInput(e.KeyChar.ToString());
            bool isSpecialCharacter = IsAllowedSpecialCharacter(e.KeyChar.ToString());

            if (!isCommand && !isValid && !isSpecialCharacter)
            {
                e.Handled = true;
                this.Text = string.Format("$ {0}", this.Text.Replace("$ ", ""));
            }
        }

        public static bool IsCommand(KeyPressEventArgs e)
        {
            //so far backspace is the only key to generate a character.
            //arrows, copy, cut, paste do not.
            bool isBackspace = e.KeyChar == '\b';
            bool isControl = ModifierKeys.HasFlag(Keys.Control);
            bool isCommand = isBackspace || isControl;
            return isCommand;
        }

        public static bool ValidateNumericInput(string input)
        {
            long tryParse = 0;
            return long.TryParse(input, out tryParse);
        }

        private bool IsAllowedSpecialCharacter(string input)
        {
            if (AllowedSpecialCharacters == null)
                return false;

            return AllowedSpecialCharacters.Contains(input);
        }
    }
}
