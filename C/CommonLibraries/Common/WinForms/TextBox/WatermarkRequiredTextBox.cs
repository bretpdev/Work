using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class WatermarkRequiredTextBox : WatermarkTextBox
    {
        public override ValidationResults Validate()
        {
            if (this.Text.Length == 0)
                return new ValidationResults(false, "Field Required");
            return base.Validate();
        }
    }
}
