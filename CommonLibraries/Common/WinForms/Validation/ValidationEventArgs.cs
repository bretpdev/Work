using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class ValidationEventArgs
    {
        public bool FormIsValid { get; internal set; }
        public IEnumerable<IValidatable> ValidControls { get; internal set; }
        public IEnumerable<IValidatable> InvalidControls { get; internal set; }
        public ValidationEventArgs(bool formIsValid, IEnumerable<IValidatable> validControls, IEnumerable<IValidatable> invalidControls)
        {
            FormIsValid = formIsValid;
            ValidControls = validControls;
            InvalidControls = invalidControls;
        }
    }
}
