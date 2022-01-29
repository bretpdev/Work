using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class AccountNumberTextBox : NumericTextBox
    {
        const int AccountNumberLength = 10;
        public override int MaxLength
        {
            get { return AccountNumberLength; }
            set { base.MaxLength = AccountNumberLength; }
        }

        public string Ssn
        {
            get
            {
                return ValidateInput(this.Text) ? this.Text : null;
            }
            set
            {
                if (ValidateInput(value)) this.Text = value;
            }
        }

		//TODO: hit data warehouse and verify that account exists
        public static new bool ValidateInput(string input)
        {
            return (input ?? "").Length == AccountNumberLength && NumericTextBox.ValidateInput(input);
        }
    }
}
