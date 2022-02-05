using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;

namespace KEYIDENCHN
{
    public class VerificationInfo
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public bool RemoveMiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public bool RemoveSuffix { get; set; }
        public string DOB { get; set; }

        public bool AdminRequired { get { return (FirstName + DOB).Trim().Length > 0; } }

        public static VerificationInfo FromForm(KeyIdentifierUpdateForm form)
        {
            VerificationInfo vi = new VerificationInfo()
            {
                FirstName = form.FirstNameBox.Text,
                MiddleName = form.MiddleNameBox.Text,
                RemoveMiddleName = form.MiddleNameCheck.Checked,
                LastName = form.LastNameBox.Text,
                Suffix = form.SuffixBox.Text,
                RemoveSuffix = form.SuffixCheck.Checked,
                DOB = form.DobBox.Text
            };
            return vi;
        }

        public void PopulateForm(KeyIdentifierUpdateForm form)
        {
            form.FirstNameBox.Text = FirstName;
            form.MiddleNameBox.Text = MiddleName;
            form.MiddleNameCheck.Checked = RemoveMiddleName;
            form.LastNameBox.Text = LastName;
            form.SuffixBox.Text = Suffix;
            form.SuffixCheck.Checked = RemoveSuffix;
            form.DobBox.Text = DOB;
        }


        public VerificationResults Verify(KeyIdentifierUpdateForm verifiedForm)
        {
            var vi = FromForm(verifiedForm);
            VerificationResults vr = new VerificationResults();
            vr.ErrorAdd(FieldNames.DOB, DOB, vi.DOB);
            vr.ErrorAdd(FieldNames.FirstName, FirstName, vi.FirstName);
            vr.ErrorAdd(FieldNames.LastName, LastName, vi.LastName);
            vr.ErrorAdd(FieldNames.MiddleName, RemoveMiddleName, vi.RemoveMiddleName, MiddleName, vi.MiddleName);
            vr.ErrorAdd(FieldNames.Suffix, RemoveSuffix, vi.RemoveSuffix, Suffix, vi.Suffix);
            return vr;
        }
    }

    public class VerificationResults
    {
        public bool ValidEntry { get { return Errors.Count == 0; } }
        public List<ErrorField> Errors { get; private set; }
        public void ErrorAdd(string field, string oldValue, string newValue)
        {
            if (oldValue != newValue)
                Errors.Add(new ErrorField(field, "The {0} fields do not match.  You entered '{1}', then entered '{2}'.", oldValue, newValue));
        }
        public void ErrorAdd(string field, bool oldRemove, bool newRemove, string oldValue, string newValue)
        {
            if (oldRemove && !newRemove)
                Errors.Add(new ErrorField(field, "The {0} fields do not match.  You first chose to remove it, but did not choose to remove it during verification."));
            else if (!oldRemove && newRemove)
                Errors.Add(new ErrorField(field, "The {0} fields do not match.  You chose to remove it during verification, but did not choose to remove it during the first step."));
            else
                ErrorAdd(field, oldValue, newValue);
        }
        public VerificationResults()
        {
            Errors = new List<ErrorField>();
        }
    }

    public class ErrorField
    {
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
        public ErrorField(string field, string message, params object[] formatArgs)
        {
            FieldName = field;
            var args = new List<object>(formatArgs);
            args.Insert(0, field);
            ErrorMessage = string.Format(message, args.ToArray());
        }
    }
}
