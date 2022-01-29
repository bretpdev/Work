using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class SsnTextBox : NumericTextBox
    {
        const int SsnLength = 9;
        public override int MaxLength
        {
            get { return SsnLength; }
            set { base.MaxLength = SsnLength; }
        }

        public string Ssn
        {
            get { return ValidateInput(this.Text) ? this.Text : null; }
            set { if (ValidateInput(value)) this.Text = value; }
        }

		//TODO: hit data warehouse and verify that account exists
        public override ValidationResults Validate()
        {
            bool isValid = ValidateInput(this.Text);
            if (isValid)
                return ValidationResults.Successful;
            else
                return new ValidationResults(false, "SSN must be 9 digits in length.  Do not include dashes.");
        }

		//TODO: hit data warehouse and verify that account exists
        public static new bool ValidateInput(string input)
        {
            return (input ?? "").Length == SsnLength && NumericTextBox.ValidateInput(input);
        }
    }
}
