using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace MD
{
    public partial class PasswordRevokedForm : BaseForm
    {
        public PasswordRevokedForm()
        {
            InitializeComponent();
        }

        private void ResetLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Proc.Start("AesPasswordReset");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
