using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class ValidatableMaskedTextBox : MaskedTextBox, IValidatable
    {
        #region IValidatable Members

        public virtual ValidationResults Validate()
        {
            return new ValidationResults(true);
        }

        bool markedInvalid = false;
        public void MarkInvalid()
        {
            this.BackColor = Color.LightPink;
            markedInvalid = true;
        }

        public void MarkValid()
        {
            if (markedInvalid) //only show as valid if it was previously invalid
                this.BackColor = Color.LightGreen;
        }

        public void ResetValidation()
        {
            this.BackColor = Color.White;
            markedInvalid = false;
        }

        #endregion
    }
}
