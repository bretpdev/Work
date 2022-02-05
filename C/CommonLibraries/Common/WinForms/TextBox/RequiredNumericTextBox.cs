using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class RequiredNumericTextBox : NumericTextBox
    {
        protected override bool ValidateKey(char character)
        {
            return ValidateInput(character.ToString());
        }

        public override ValidationResults Validate()
        {
            bool valid = AlphaNumericTextBox.ValidateNumericInput(this.Text);
            if (!this.Enabled)
                return new ValidationResults(true);
            if (valid)
                return new ValidationResults(true);
            else if (this.Text.Length == 0)
                return new ValidationResults(false, "Field required");
            else
                return new ValidationResults(false, "Please enter only numbers");
        }

        public static new bool ValidateInput(string input)
        {
            return AlphaNumericTextBox.ValidateNumericInput(input);
        }
    }
}
