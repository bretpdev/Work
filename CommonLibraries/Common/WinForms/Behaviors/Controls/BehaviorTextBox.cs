using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class BehaviorTextBox : TextBox, IHasBehaviors, IHasValue, IHasBackColor
    {
        public BehaviorTextBox()
        {
            InstalledBehaviors = new BehaviorInstallation();
        }
        private BehaviorInstallation installation;
        [Editor(typeof(InstalledBehaviorsEditor), typeof(UITypeEditor))]
        public BehaviorInstallation InstalledBehaviors
        {
            get { return installation; }
            set
            {
                installation = value;
                installation.Install();
            }
        }

        #region IHasValue Members
        public object Value
        {
            get { return Text; }
            set
            {
                if (ValueSet != null) //value has been reset in code
                    ValueSet(this, new ValueSetEventArgs(value ?? "")); //null defaults to empty string for text boxes
                Text = value.IsNull(o => o.ToString(), null);
            }
        }
        public event OnValueChanged ValueChanged;
        public event OnValueSet ValueSet;
        protected override void OnTextChanged(EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, new ValueChangedEventArgs(this.Value));
            base.OnTextChanged(e);
        }
        #endregion
    }
}
