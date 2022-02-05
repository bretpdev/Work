using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace OLDEMOS
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
            this.DialogResult = DialogResult.OK;
        }
    }
}