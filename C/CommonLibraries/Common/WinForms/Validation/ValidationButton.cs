using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace Uheaa.Common.WinForms
{
    public class ValidationButton : Button
    {
        public ValidationButton()
        {
            this.Click += Validate;
        }

        public event ValidationHandler OnValidate;
        public delegate void ValidationHandler(object sender, ValidationEventArgs e);

        public void Validate(object sender, EventArgs e)
        {
            Control parentSearch = this;
            while (!(parentSearch is Form))
                parentSearch = parentSearch.Parent;
            Form parent = parentSearch as Form;

            List<IValidatable> badControls = new List<IValidatable>();
            List<IValidatable> goodControls = new List<IValidatable>();
            foreach (IValidatable v in FindValidationControls(parent))
            {
                var results = v.Validate();
                if (results.Success)
                {
                    goodControls.Add(v);
                    v.MarkValid();
                }
                else
                {
                    badControls.Add(v);
                    v.MarkInvalid();
                }
            }

            if (OnValidate != null)
                OnValidate(this, new ValidationEventArgs(badControls.Count() == 0, goodControls, badControls));
        }

        public IEnumerable<IValidatable> FindValidationControls(Control parent)
        {
            List<IValidatable> results = new List<IValidatable>();
            foreach (Control c in parent.Controls)
                if (c is IValidatable)
                    results.Add(c as IValidatable);
                else
                    results.AddRange(FindValidationControls(c));
            return results;
        }
    }
}
