using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpecialEmailCampaignFed
{
    public partial class Login : Form
    {
        private LoginData lData;
        public Login(LoginData userInfo)
        {
            InitializeComponent();
            lData = userInfo;
            loginDataBindingSource.DataSource = lData;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            lData.UserId = txtUserId.Text;
            lData.Password = txtPassword.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
