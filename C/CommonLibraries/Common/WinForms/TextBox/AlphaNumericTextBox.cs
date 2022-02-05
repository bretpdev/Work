using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class AlphaNumericTextBox : ValidatableTextBox
    {
        public string AllowedSpecialCharacters { get; set; }

        public AlphaNumericTextBox()
        {
            this.KeyPress += KeyPressHandler;
            AllowedSpecialCharacters = string.Empty;
        }

        protected virtual bool ValidateKey(char character)
        {
            return true;
        }

        private void KeyPressHandler(object sender, KeyPressEventArgs e)
        {
            bool isCommand = IsCommand(e);
            bool isValid = ValidateInput(e.KeyChar.ToString()) && ValidateKey(e.KeyChar);
            bool isSpecialCharacter = IsAllowedSpecialCharacter(e.KeyChar.ToString());
           
            if (!isCommand && !isValid && !isSpecialCharacter)
                e.Handled = true;
        }

        private bool IsAllowedSpecialCharacter(string input)
        {
            if (AllowedSpecialCharacters == null)
                return false;

            return AllowedSpecialCharacters.Contains(input);
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

        public static bool ValidateInput(string input)
        {
            return ValidateNumericInput(input) || ValidateAlphaInput(input);
        }

        public static bool ValidateNumericInput(string input)
        {
            long tryParse = 0;
            return long.TryParse(input, out tryParse);
        }

        public static bool ValidateAlphaInput(string input)
        {
            foreach (char c in input)
                if (!ValidAlphaChar(c))
                    return false;
            return true;
        }

        private static bool ValidAlphaChar(char c)
        {
            var isCapital = ((int)c).BetweenInc(65, 90); //A-Z
            var isLower = ((int)c).BetweenInc(97, 122); //a-z
            return isCapital || isLower;
        }
    }
}
