using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ActiveDirectoryGroups
{
    partial class frmAddUser : Form
    {
        public ActiveDirectoryUser User { get; set; }
        private List<BusinessUnit> Units { get; set; }
        private List<Role> Roles { get; set; }
        public DataAccess DA { get; set; }

        public frmAddUser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates or Updates a new ActiveDirectoryUser
        /// </summary>
        /// <param name="user">ActiveDirectoryUser</param>
        /// <param name="isNewUser">Set to True if new ActiveDirectoryUser, Set to False if updating existing ActiveDirectoryUser</param>
        /// <param name="units">List of BusinessUnit</param>
        /// <param name="roles">List of Role</param>
        public frmAddUser(ActiveDirectoryUser user, List<BusinessUnit> units, List<Role> roles, DataAccess da)
        {
            InitializeComponent();

            Units = units.OrderBy(p => p.Name).ToList();
            Roles = roles.OrderBy(p => p.RoleName).ToList();
            DA = da;

            //Set the drop down data sources
            cboBusinessUnit.DataSource = Units;
            cboBusinessUnit.DisplayMember = "Name";
            cboBusinessUnit.ValueMember = "ID";
            cboRole.DataSource = Roles;
            cboRole.DisplayMember = "RoleName";
            cboRole.ValueMember = "RoleID";

            User = user;

            if (User.SqlUserID == 0)
            {
                User.SqlUserID = 0;
                User.Status = true;
                btnUpdateUser.Visible = false;
            }
            else
            {
                //Disable the required fields since this is updating the user
                txtUserName.Enabled = false;
                txtFirstName.Enabled = false;
                txtMiddleInitial.Enabled = false;
                txtLastName.Enabled = false;
                txtEmail.Enabled = false;
                btnAdd.Text = "Update";
            }

            activeDirectoryUserBindingSource.DataSource = User;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                User.WindowsUserName = txtUserName.Text;
                User.FirstName = txtFirstName.Text;
                User.LastName = txtLastName.Text;
                User.EmailAddress = txtEmail.Text + lblEmailExtension.Text;
                User.MiddleInitial = txtMiddleInitial.Text;
                User.BusinessUnit = Units.Where(p => p.Name == cboBusinessUnit.Text).FirstOrDefault();
                User.Role = Roles.Where(p => p.RoleName == cboRole.Text).FirstOrDefault();
                User.Extension = txtExtension.Text;
                User.Extension2 = txtExtension2.Text;
                User.Status = chkStatus.Checked;
                User.Title = cboTitle.Text;
                User.AesUserID = txtAesUserID.Text;

                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Checks all required fields and warns user when data is missing
        /// </summary>
        private bool ValidateControls()
        {
            List<string> missingData = new List<string>();
            if (txtUserName.Text.Trim().IsNullOrEmpty())
                missingData.Add("Windows User Name");
            if (txtFirstName.Text.Trim().IsNullOrEmpty())
                missingData.Add("First Name");
            if (txtLastName.Text.Trim().IsNullOrEmpty())
                missingData.Add("Last Name");
            if (txtEmail.Text.Trim().IsNullOrEmpty())
                missingData.Add("Email Address");
            if (cboBusinessUnit.Text.Trim().IsNullOrEmpty())
                missingData.Add("Business Unit");
            if (cboRole.Text.Trim().IsNullOrEmpty())
                missingData.Add("Role");
            if (cboTitle.Text.Trim().IsNullOrEmpty())
                missingData.Add("Title");
            if (missingData.Count == 0)
                return true;
            else
            {
                Dialog.Error.Ok($"You are missing required fields. Please supply the following data.\r\n\r\n{string.Join("\r\n", missingData.ToArray())}", "Missing Data");
                return false;
            }
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            txtUserName.Enabled = true;
            txtFirstName.Enabled = true;
            txtMiddleInitial.Enabled = true;
            txtLastName.Enabled = true;
            txtEmail.Enabled = true;
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            txtEmail.Text = txtUserName.Text;
        }

        private void lblEmailExtension_Click(object sender, EventArgs e)
        {
            if (lblEmailExtension.Text.ToUpper().Contains("UTAHSBR"))
                lblEmailExtension.Text = "@uesp.org";
            else
                lblEmailExtension.Text = "@utahsbr.edu";
        }

        private void AesAccounts_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmAesAccounts accts = new frmAesAccounts(User.SqlUserID, DA);
            accts.ShowDialog();
            this.Show();
        }

        private void Tilp_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmTilp tilp = new frmTilp(User.WindowsUserName, DA);
            tilp.ShowDialog();
            this.Show();
        }
    }
}