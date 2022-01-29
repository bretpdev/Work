using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public interface IValidatable
    {
        /// <summary>
        /// Retrieve information on whether the control is valid.  Does not change the appearance or behavior of the control.
        /// </summary>
        ValidationResults Validate();
        /// <summary>
        /// Update the control's visual appearance to reflect that it is invalid.
        /// </summary>
        void MarkInvalid();
        /// <summary>
        /// Update the control's visual appearance to reflect that it is valid.
        /// </summary>
        void MarkValid();
        /// <summary>
        /// Update the control's visual appearance to reflect it's original appearance.
        /// </summary>
        void ResetValidation();
    }
}
