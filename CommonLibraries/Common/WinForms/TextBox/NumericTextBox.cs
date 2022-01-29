using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class NumericTextBox : AlphaNumericTextBox
    {
        protected override bool ValidateKey(char character)
        {
            return ValidateInput(character.ToString());
        }

        public override ValidationResults Validate()
        {
            string val = this.Text;
            foreach (char item in AllowedSpecialCharacters)
                val = val.Replace(item.ToString(),"");
            bool valid = AlphaNumericTextBox.ValidateNumericInput(val);
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
