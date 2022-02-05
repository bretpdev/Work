using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.WinForms
{
    public class NumericDecimalTextBox : AlphaNumericTextBox
    {
        protected override bool ValidateKey(char character)
        {
            AllowedSpecialCharacters = ".";
            return ValidateInput(character.ToString());
        }

        public static new bool ValidateInput(string input)
        {
            return AlphaNumericTextBox.ValidateNumericInput(input);
        }

        public override ValidationResults Validate()
        {
            AllowedSpecialCharacters = ".";
            string val = this.Text;
            foreach (char item in AllowedSpecialCharacters)
                val = val.Replace(item.ToString(), "");
            bool valid = AlphaNumericTextBox.ValidateNumericInput(val);
            if (valid)
                return new ValidationResults(true);
            else
                return new ValidationResults(false, "Please enter only numbers");
        }
    }
}