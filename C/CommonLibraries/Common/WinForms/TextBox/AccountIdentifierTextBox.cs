using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class AccountIdentifierTextBox : NumericTextBox
    {
        const int IdentifierMaxLength = 10;
        const int IdentifierMinLength = 9;

        public AccountIdentifierTextBox()
        {
            this.MaxLength = IdentifierMaxLength;
        }

        public string Ssn
        {
            get { return SsnTextBox.ValidateInput(this.Text) ? this.Text : null; }
            set { if (SsnTextBox.ValidateInput(value)) this.Text = value; }
        }

        public string AccountNumber
        {
            get { return AccountNumberTextBox.ValidateInput(this.Text) ? this.Text : null; }
            set { if (AccountNumberTextBox.ValidateInput(value)) this.Text = value; }
        }

        public static new bool ValidateInput(string input)
        {
            int length = (input ?? "").Length;
            return length.BetweenInc(IdentifierMinLength, IdentifierMaxLength) && NumericTextBox.ValidateInput(input);
        }
    }
}
