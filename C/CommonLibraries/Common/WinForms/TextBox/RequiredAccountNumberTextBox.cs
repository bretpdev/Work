using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class RequiredAccountNumberTextBox : NumericTextBox
    {
        const int AccountNumberLength = 10;
        public override int MaxLength
        {
            get { return AccountNumberLength; }
            set { base.MaxLength = AccountNumberLength; }
        }

        protected override bool ValidateKey(char character)
        {
            return ValidateInput(character.ToString());
        }

		//TODO: hit data warehouse and verify that account exists
        public override ValidationResults Validate()
        {
            bool valid = AlphaNumericTextBox.ValidateNumericInput(this.Text);
            if (valid)
                return new ValidationResults(true);
            else if (this.Text.Length == 0)
                return new ValidationResults(false, "Field required");
            else
                return new ValidationResults(false, "Please enter only numbers");
        }

		//TODO: hit data warehouse and verify that account exists
        public static new bool ValidateInput(string input)
        {
            return AlphaNumericTextBox.ValidateNumericInput(input);
        }
    }
}
