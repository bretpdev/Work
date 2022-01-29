using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class AlphaTextBox : AlphaNumericTextBox
    {
        protected override bool ValidateKey(char character)
        {
            return ValidateInput(character.ToString());
        }

        public override ValidationResults Validate()
        {
            bool valid = ValidateInput(this.Text);
            if (valid)
                return new ValidationResults(true);
            else
                return new ValidationResults(false, "Please enter only letters");
        }

        public static new bool ValidateInput(string input)
        {
            return AlphaNumericTextBox.ValidateAlphaInput(input);
        }
    }
}
