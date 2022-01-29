using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class NumericMaskedTextBox : AlphaNumericMaskedTextBox
    {
        protected override bool ValidateKey(char character)
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

        public static new bool ValidateInput(string input)
        {
            return AlphaNumericTextBox.ValidateNumericInput(input);
        }
    }
}
