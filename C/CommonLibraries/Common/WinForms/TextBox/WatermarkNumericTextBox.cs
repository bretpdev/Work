using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class WatermarkNumericTextBox : WatermarkTextBox
    {
        protected bool ValidateKey(char character)
        {
            return ValidateInput(character.ToString());
        }

        public override ValidationResults Validate()
        {
            bool valid = AlphaNumericTextBox.ValidateNumericInput(this.Text);
            if (valid)
                return new ValidationResults(true);
            else
                return new ValidationResults(false, "Please enter only numbers");
        }

        public static bool ValidateInput(string input)
        {
            return AlphaNumericTextBox.ValidateNumericInput(input);
        }
    }
}
