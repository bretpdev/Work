using System;
using System.Windows.Forms;
using Uheaa.Common;

namespace VERFORBUH
{ 
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUtId.Text.Length != 7 || !txtUtId.Text.ToUpper().Contains("UT"))
                Dialog.Error.Ok("The UT ID is not correct. Please fix it and try again");
            else if (txtPassword.Text.Length != 8)
                Dialog.Error.Ok($"The password must be 10 characters long but is only {txtPassword.Text.Length}. Please fix and try again.");
            else
                this.DialogResult = DialogResult.OK;
        }
    }
}