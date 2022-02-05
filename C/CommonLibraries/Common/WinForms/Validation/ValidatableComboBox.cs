using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class ValidatableComboBox : ComboBox, IValidatable
    {
        #region IValidatable Members

        public virtual ValidationResults Validate()
        {
            return ValidationResults.Successful;
        }

        public void MarkInvalid()
        {
            this.BackColor = Color.LightPink;
        }

        public void MarkValid()
        {
            this.BackColor = Color.LightGreen;
        }

        public void ResetValidation()
        {
            this.BackColor = Color.White;
        }

        #endregion
    }
}
