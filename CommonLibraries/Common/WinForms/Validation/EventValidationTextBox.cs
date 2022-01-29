using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class EventValidationTextBox : ValidatableTextBox
    {
        public delegate ValidationResults ValidationHandler(EventValidationTextBox sender);
        public event ValidationHandler OnValidate;
        public override ValidationResults Validate()
        {
            if (OnValidate != null)
                return OnValidate(this);
            return base.Validate();
        }
    }
}
