using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Screen = Uheaa.Common.Scripts.Screen;

namespace MD
{
    public partial class PasswordExpiredForm : BaseForm
    {
        ReflectionInterface RI { get; set; }
        public PasswordExpiredForm()
        {
            InitializeComponent();
        }

        public void Initialize(ReflectionInterface ri)
        {
            RI = ri;
        }

        private void PasswordExpiredForm_Load(object sender, EventArgs e)
        {
            ValidatePasswords();
        }

        bool initialPasswordEntered = false;
        bool confirmPasswordEntered = false;
        IEnumerable<char> upperChars = Enumerable.Range((int)'A', 26).Select(o => (char)o).Union(new char[] { '@', '#', '$' });
        IEnumerable<char> lowerChars = Enumerable.Range((int)'a', 26).Select(o => (char)o);
        private bool? ValidatePasswords()
        {
            //string password = PasswordText.Text; //using PasswordText.Text directly
            bool length = PasswordText.Text.Length == 8;
            bool upper = PasswordText.Text.Intersect(upperChars).Any();
            bool lower = PasswordText.Text.Intersect(lowerChars).Any();
            bool number = PasswordText.Text.Length >= 2 && Char.IsDigit(PasswordText.Text[1]);
            bool same = ConfirmText.Text == PasswordText.Text;
            bool empty = PasswordText.Text.IsNullOrEmpty();
            bool overallSuccess = true;
            Action<bool?, CheckBox> check = (b, c) =>
            {
                bool? val = b;
                if (b == false)
                {
                    val = !initialPasswordEntered ? (bool?)null : false;
                    overallSuccess = false;
                }
                c.CheckState = val.Quat(CheckState.Checked, CheckState.Unchecked, CheckState.Indeterminate);
                c.ForeColor = val.Quat(Color.Green, Color.Red, Color.DarkGoldenrod);
            };
            check(length, LengthCheck);
            check(upper, UppercaseCheck);
            check(lower, LowerCaseCheck);
            check(number, NumberCheck);
            if (confirmPasswordEntered)
                ConfirmLabel.ForeColor = NewPasswordLabel.ForeColor = same ? Color.Black : Color.Red;

            bool? ret = overallSuccess ? true : (bool?)null;
            if (!same || empty)
            {
                ret = false;
                OkButton.Enabled = false;
            }
            else
                OkButton.Enabled = true;
            return ret;
        }

        private void ShowPasswordCheck_CheckedChanged(object sender, EventArgs e)
        {
            PasswordText.PasswordChar = ConfirmText.PasswordChar = ShowPasswordCheck.Checked ? '\0' : '*';
        }

        private void PasswordText_TextChanged(object sender, EventArgs e)
        {
            ValidatePasswords();
        }

        private void ConfirmText_Enter(object sender, EventArgs e)
        {
            initialPasswordEntered = true;
            ValidatePasswords();
        }

        private void ConfirmText_Leave(object sender, EventArgs e)
        {
            confirmPasswordEntered = true;
            ValidatePasswords();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            bool? result = ValidatePasswords();
            if (result == false)
                OkButton.Enabled = false;
            else if (result == null || result == true)
            {
                RI.PutText(20, 65, PasswordText.Text, ReflectionInterface.Key.Enter);
                PasswordText.Text = "";
                ConfirmText.Text = "";
                DialogResult = DialogResult.OK;
            }
        }
    }
}
