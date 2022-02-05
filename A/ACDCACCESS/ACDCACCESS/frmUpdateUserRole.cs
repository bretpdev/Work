using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ACDCAccess
{
    partial class frmUpdateUserRole : Form
    {
        Role _role;
        DataAccess _da;
        List<SqlUser> _users;

        public frmUpdateUserRole()
        {
            InitializeComponent();
        }

        public frmUpdateUserRole(DataAccess da, List<SqlUser> users, Role role, List<Role> roles)
        {
            InitializeComponent();

            _role = role;
            _da = da;
            _users = users;

            lblRoleChange.Text = lblRoleChange.Text.Replace("@@@@@", role.RoleName).Replace("@@@@", _users.Count.ToString());
            roles.RemoveAll(p => p.RoleID == role.RoleID);
            cboRoles.DataSource = roles;
            cboRoles.DisplayMember = "RoleName";
            cboRoles.ValueMember = "RoleID";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ChangeRole(_role = ((Role)cboRoles.SelectedItem));
            DialogResult = DialogResult.OK;
        }

        private void ChangeRole(Role role)
        {
            foreach (SqlUser user in _users)
            {
                _da.ChangeUserRole(role, user);
            }
        }
    }
}
