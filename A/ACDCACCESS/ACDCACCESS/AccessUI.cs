using ACDCAccess.Reports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.XPath;
using Uheaa.Common.ProcessLogger;

namespace ACDCAccess
{
    public partial class AccessUI : Form
    {
        public static int _sqlUserId;
        private List<string> _userRoles;
        private DataAccess _da;
        List<ExistingKey> _userAssignedKeys;
        List<ExistingKey> _availableKeys;
        List<Role> _roles;
        List<KeyHistory> _historyKeys;
        List<UserHistory> _userHistory;
        List<KeyHistory> _roleHistoryKeys;
        List<RoleHistory> _roleHistory;
        List<SqlUser> _users;
        public ProcessLogRun LogRun { get; set; }

        public AccessUI()
        {
            InitializeComponent();
        }

        public AccessUI(int sqlUserId, List<string> userRoles, ProcessLogRun logRun)
        {
            InitializeComponent();
            _da = new DataAccess(LogRun);

            _sqlUserId = sqlUserId;
            _userRoles = userRoles;
            _userAssignedKeys = new List<ExistingKey>();
            _availableKeys = new List<ExistingKey>();
            _roleHistory = new List<RoleHistory>();
            SetTabsByAccess();
            LoadDropDownLists();
        }

        /// <summary>
        /// Gives the logged in user access to the tabs their role has been given access to
        /// </summary>
        private void SetTabsByAccess()
        {
            foreach (string role in _userRoles)
            {
                switch (role)
                {
                    case "SystemAnalyst":  //Will be given full access
                        break;
                    default:
                        //((Control)this.tabAddAndRemoveKeysToRole).Enabled = false;
                        //((Control)this.tabAddKeysAndApplications).Enabled = false;
                        //((Control)this.tabModeling).Enabled = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Sets all the comboboxes datasource to null, gathers all the new changes and reloads the comboboxes
        /// </summary>
        private void LoadDropDownLists()
        {
            //Clear out the data
            lbxRoles.DataSource = null;
            cboRole.DataSource = null;
            cboRoleChange.DataSource = null;
            cboRoleModel.DataSource = null;

            //Gather the roles and reset the datasources
            _roles = _da.GetRoles().OrderBy(p => p.RoleName).ToList();
            lbxRoles.DataSource = new List<Role>(_roles);
            lbxRoles.DisplayMember = "RoleName";
            lbxRoles.ValueMember = "RoleID";
            _roles.Insert(0, new Role() { RoleID = 0, RoleName = string.Empty });
            cboRole.DataSource = new List<Role>(_roles);
            cboRole.DisplayMember = "RoleName";
            cboRole.ValueMember = "RoleID";
            cboRolesKeyHistory.DataSource = new List<Role>(_roles);
            cboRolesKeyHistory.DisplayMember = "RoleName";
            cboRolesKeyHistory.ValueMember = "RoleID";

            cboRoleChange.DataSource = new List<Role>(_roles);
            cboRoleChange.DisplayMember = "RoleName";
            cboRoleChange.ValueMember = "RoleID";

            cboRoleModel.DataSource = new List<Role>(_roles);
            cboRoleModel.DisplayMember = "RoleName";
            cboRoleModel.ValueMember = "RoleID";

            //Gather the systems and reset the datasources
            List<string> systems = _da.GetSystems();
            systems.Insert(0, "");
            cboSystem.DataSource = new List<string>(systems);

            cboKeySystem.DataSource = new List<string>(systems);

            _users = _da.GetUsers().OrderBy(name => name.LegalName).ToList();
            _users.Insert(0, new SqlUser() { LegalName = string.Empty, ID = 0 });
        }

        /// <summary>
        /// Clears out the tab that is being selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabAccess.SelectedIndex)
            {
                case 0:
                    lblRoleAdded.Visible = false;
                    txtAddRole.Text = string.Empty;
                    dgvExistingKeys.DataSource = null;
                    LoadDropDownLists();
                    break;
                case 1:
                    cboRole.SelectedIndex = 0;
                    cboSystem.SelectedIndex = -1;
                    dgvAddRole.DataSource = null;
                    dgvRemoveRole.DataSource = null;
                    LoadDropDownLists();
                    break;
                case 2:
                    cboRoleModel.SelectedIndex = -1;
                    cboRoleChange.SelectedIndex = -1;
                    dgvModelAfter.DataSource = null;
                    dgvModelChanged.DataSource = null;
                    LoadDropDownLists();
                    break;
                case 3:
                    chkRoleHistory.Checked = false;
                    dgvRoleHistory.DataSource = null;
                    _roleHistory = _da.GetRoleHistory(chkRoleHistory.Checked).ToList();
                    dgvRoleHistory.DataSource = _roleHistory;
                    dgvRoleHistory.Focus();
                    break;
                case 4:
                    dgvRoleKeyAccess.DataSource = null;
                    cboRolesKeyHistory.SelectedIndex = -1;
                    cboSortRoleKey.SelectedIndex = -1;
                    chkHistory.Checked = false;
                    break;
                case 5:
                    cboSortKey.SelectedIndex = -1;
                    chkKeyHistory.Checked = false;
                    dgvKeyHistory.DataSource = null;
                    _historyKeys = _da.GetKeyHistory(chkHistory.Checked);
                    dgvKeyHistory.DataSource = _historyKeys;
                    dgvKeyHistory.Focus();
                    break;
                case 6:
                    lblFileOpened.Text = string.Empty;
                    dgvUserHistory.DataSource = null;
                    break;
            }
        }

        /// <summary>
        /// Checks the text for characters that are not acceptable
        /// </summary>
        /// <param name="text">The text that is being checked</param>
        /// <returns>False if a character is found, True if the text has no invalid characters</returns>
        private bool CheckForSpecialCharacters(string text)
        {
            List<string> characters = new List<string>() { "!", "@", "#", "$", "%", "^", "&", "*", "_", "=", "+", "{", "}", "[", "]", @"\", "|", "<", ">", ",", ":", ";", "`", "~" };

            bool isCorrect = true;
            string message = "You can not use any of the following characters\r\n\r\n";
            foreach (string str in characters)
            {
                message += str + " ";
                if (text.Contains(str))
                {
                    isCorrect = false;
                }
            }

            if (!isCorrect)
            {
                message += "\r\n\r\nPlease remove the characters and try again";
                MessageBox.Show(message, "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isCorrect;
        }

        #region Add Keys & Roles

        private void txtAddRole_Enter(object sender, EventArgs e)
        {
            lblRoleAdded.Visible = false;
        }

        /// <summary>
        /// Adds a new role to the database, activates previous roles and activates previous roles access if necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRole_Click(object sender, EventArgs e)
        {
            if (!CheckForSpecialCharacters(txtAddRole.Text)) { return; }
            //Validate that the role name is supplied
            if (txtAddRole.Text == string.Empty)
            {
                MessageBox.Show("You must provide a role name", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //Check to see if the role has been used before
            Role inactiveRole = _da.CheckIfRoleNameInactive(txtAddRole.Text);
            if (inactiveRole != null && (inactiveRole.RoleID > 0))
            {
                //If the role has been used before, ask if the user wants to activate the role and restore the roles access
                string message = string.Format("The Role: '{0}' is an inactive role.\r\n\r\nDo you want to activate this role and all of the roles access?\r\n\r\nClick no to create a new role with the default access.", txtAddRole.Text);
                DialogResult result = MessageBox.Show(message, "Role Already Exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                //If the user chooses yes, create the role and add all of the previous access back to the role.
                if (result == DialogResult.Yes)
                {
                    //Add the previous role to the database
                    int newRoleID = int.Parse(_da.AddRole(txtAddRole.Text.Trim()).ToString());
                    if (newRoleID > 0)
                    {
                        DateTime dateDeleted = _da.GetDateRoleWasDeleted(inactiveRole.RoleID);
                        if (dateDeleted != new DateTime(1900, 1, 1))
                        {
                            //Get all of the roles previous keys
                            List<RoleKey> inactiveKeys = _da.GetInactiveKeysForRole(inactiveRole.RoleID, dateDeleted);

                            //Add the keys back to the role, only use a distinct list
                            foreach (RoleKey key in inactiveKeys.Distinct(Comparer.Instance))
                            {
                                _da.AddKeysToRole(key.UserKeyID, newRoleID);
                            }
                            MessageBox.Show(txtAddRole.Text + " Role is now active. The " + txtAddRole.Text + " access has been restored.", "Role Activated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDropDownLists();
                        }
                        else
                        {
                            MessageBox.Show("The '" + txtAddRole.Text + "' Role was added but there was an error adding the access.", "Role Created With Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                //If the user chooses no, create the role but give it no access
                else if (result == DialogResult.No)
                {
                    _da.AddRole(txtAddRole.Text);
                    MessageBox.Show("The new role " + txtAddRole.Text + " has been created.", "Role Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDropDownLists();
                }
            }
            else
            {
                if (_da.AddRole(txtAddRole.Text) > 0)
                {
                    //Add a new role that has never been used before
                    lblRoleAdded.Text = txtAddRole.Text + " Role Added";
                    lblRoleAdded.Visible = true;
                    LoadDropDownLists();
                }
                else
                {
                    MessageBox.Show("The '" + txtAddRole.Text + "' Role already exists", "Role Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            txtAddRole.Text = string.Empty;
        }

        /// <summary>
        /// Inactivates roles and their access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Role role = new Role();

            lblRoleAdded.Visible = false;
            if (lbxRoles.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose a role to delete", "No Role Chosen", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Message the user and ask if they have all the correct roles selected
                string message = "Are you sure you want to delete the following roles?\r\n\r\n";
                foreach (Role item in lbxRoles.SelectedItems)
                {
                    message += item.RoleName + "\r\n";
                }
                if (MessageBox.Show(message, "Delete Role(s)", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //Delete each role selected and inactivate their access
                    foreach (Role item in lbxRoles.SelectedItems)
                    {
                        role.RoleID = item.RoleID;
                        role.RoleName = item.RoleName;

                        _da.DeleteRole(item.RoleID);
                        List<Key> userKeys = _da.GetRoleAssignedKeys(item.RoleID, string.Empty).ToList();
                        _userAssignedKeys = new List<ExistingKey>();
                        if (userKeys.Count > 0)
                        {
                            foreach (Key key in userKeys)
                            {
                                ExistingKey eKey = new ExistingKey();
                                eKey.ID = key.ID;
                                eKey.Name = key.Name;
                                eKey.System = key.Application;
                                eKey.Description = key.Description;
                                _userAssignedKeys.Add(eKey);
                            }
                            foreach (ExistingKey key in _userAssignedKeys)
                            {
                                _da.RemoveKeyFromRole(key.ID, item.RoleID);
                            }
                        }
                        if (role.RoleName != null && role.RoleName != string.Empty) ChangeUsersRole(role);
                    }
                }
                LoadDropDownLists();
            }
        }

        /// <summary>
        /// Opens a form that asks for a new role name for the role being deleted
        /// </summary>
        /// <param name="role"></param>
        private void ChangeUsersRole(Role role)
        {
            List<SqlUser> users = _users.Where(p => p.Role == role.RoleID).ToList();
            if (users.Count > 0)
            {
                frmUpdateUserRole userRole = new frmUpdateUserRole(_da, users, role, _roles);
                userRole.ShowDialog();
            }
        }

        /// <summary>
        /// Opens a new form that allows the user to change the name of a role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRoleUpdate_Click(object sender, EventArgs e)
        {
            frmUpdateRole update = new frmUpdateRole(_da, (Role)lbxRoles.SelectedItem);
            if (update.ShowDialog() == DialogResult.OK)
            {
                LoadDropDownLists();
            }
            else
            {
                MessageBox.Show("The Role name has not been updated", "No Changes Made", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Selects all the keys available for the system chosen by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboKeySystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvExistingKeys.DataSource = null;
            if (cboKeySystem.SelectedIndex != 0 && cboKeySystem.SelectedIndex != -1)
            {
                //Get a list of all the keys for the given system
                List<Key> keys = _da.GetKeys(cboKeySystem.Text);
                List<ExistingKey> keysToDisplay = new List<ExistingKey>();
                foreach (Key item in keys)
                {
                    //Convert the keys to an ExistingKey to be displayed appropriately in the grid view
                    ExistingKey key = new ExistingKey();
                    key.ID = item.ID;
                    key.Name = item.Name;
                    key.System = item.Application;
                    key.Description = item.Description;
                    keysToDisplay.Add(key);
                }
                dgvExistingKeys.DataSource = keysToDisplay;
                dgvExistingKeys.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Adds access keys for the given system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddKeyToSystem_Click(object sender, EventArgs e)
        {
            //Verify that all of the required data is provided
            if (cboKeySystem.Text != "" && txtKey.Text.Trim() != "" && txtDescription.Text.Trim() != "")
            {
                if (CheckForSpecialCharacters(txtKey.Text) && CheckForSpecialCharacters(txtDescription.Text))
                {
                    //Check to see if the key has been used
                    List<Key> keys = _da.GetKeys(cboKeySystem.Text);
                    if (keys.Where(p => p.Name.ToUpper().Trim() == txtKey.Text.ToUpper().Trim()).Count() > 0)
                    {
                        MessageBox.Show("That key already exists. Please choose a new name.", "Key Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtKey.Text = string.Empty;
                        return;
                    }

                    //Create a new key with the data from the form
                    Key key = new Key();
                    key.Application = cboKeySystem.Text;
                    key.Name = txtKey.Text;
                    key.Description = txtDescription.Text;
                    key.Type = "Access";
                    key.AddedBy = _sqlUserId.ToString();
                    if (_da.AddKey(key))
                    {
                        //If the key was added successfully, refresh the grid view so the key is visible
                        txtKey.Text = string.Empty;
                        txtDescription.Text = string.Empty;
                        cboKeySystem_SelectedIndexChanged(sender, e);
                    }
                }
            }
            else
            {
                //Display an error message to the user of the missing data
                if (cboKeySystem.SelectedIndex == 0 || cboKeySystem.SelectedIndex == -1)
                {
                    MessageBox.Show("Please choose a system to add the key", "System Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (txtKey.Text.Trim() == "")
                {
                    MessageBox.Show("Please provide a Key Name to add the key", "Key Name Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (txtDescription.Text.Trim() == "")
                {
                    MessageBox.Show("Please provide a description to add the key", "Description Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Updates the RemovedBy and EndDate fields in the database for the given key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteKey_Click(object sender, EventArgs e)
        {
            //Verify the user has the correct key
            string message = "Are you sure you want to delete the following keys?\r\n\r\n";
            foreach (DataGridViewRow item in dgvExistingKeys.SelectedRows)
            {
                message += item.Cells["Name"].Value.ToString() + "\r\n";
            }
            if (MessageBox.Show(message, "Delete Key(s)", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Delete each key that was chosen in the grid view
                foreach (DataGridViewRow item in dgvExistingKeys.SelectedRows)
                {
                    if (item.Cells["System"].Value != null)
                    {
                        Key key = new Key();
                        key.Application = item.Cells["System"].Value.ToString();
                        key.Name = item.Cells["Name"].Value.ToString();
                        _da.DeleteKey(key);
                        int keyID = _da.GetKeyID(key);
                    }
                }
                cboKeySystem_SelectedIndexChanged(sender, e);
            }
        }

        #endregion

        #region Add & Remove Keys to Role

        /// <summary>
        /// Gets the keys available and keys assigned to the given role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvAddRole.DataSource = null;
            dgvRemoveRole.DataSource = null;

            if (cboRole.SelectedIndex != 0 && cboRole.SelectedIndex != -1)
            {
                //Gets a list of the keys assigned to the role
                List<Key> userKeys = _da.GetRoleAssignedKeys(((Role)cboRole.SelectedItem).RoleID, "").ToList();
                _userAssignedKeys = new List<ExistingKey>();
                foreach (Key item in userKeys)
                {
                    ExistingKey eKey = new ExistingKey();
                    eKey.ID = item.ID;
                    eKey.Name = item.Name;
                    eKey.System = item.Application;
                    eKey.Description = item.Description;
                    _userAssignedKeys.Add(eKey);
                }

                //Gets a list of keys that are available
                List<Key> availKeys = _da.GetKeysByType(cboSystem.Text, DataAccess.Type.Access);
                _availableKeys = new List<ExistingKey>();
                foreach (Key item in availKeys)
                {
                    ExistingKey aKey = new ExistingKey();
                    aKey.ID = item.ID;
                    aKey.Name = item.Name;
                    aKey.System = item.Application;
                    aKey.Description = item.Description;
                    _availableKeys.Add(aKey);
                }

                //Removes the access keys that the role has assigned to not display in the available list
                foreach (ExistingKey key in _userAssignedKeys)
                {
                    _availableKeys.RemoveAll(p => p.Name == key.Name);
                }
                dgvAddRole.DataSource = _availableKeys;
                dgvRemoveRole.DataSource = _userAssignedKeys;
                dgvAddRole.Refresh();
                dgvRemoveRole.Refresh();
                dgvAddRole.Focus();
            }
        }

        /// <summary>
        /// Displays all the keys that are avail for the role for the chosen system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvRemoveRole.DataSource = null;
            dgvAddRole.DataSource = null;

            if (cboSystem.SelectedIndex != 0 && cboSystem.SelectedIndex != -1)
            {
                if (cboRole.Text != "")
                {
                    //Get the keys assigned to the user for the chosen system
                    List<Key> userKeys = _da.GetRoleAssignedKeys(((Role)cboRole.SelectedItem).RoleID, cboSystem.Text).ToList();
                    _userAssignedKeys = new List<ExistingKey>();
                    foreach (Key item in userKeys)
                    {
                        ExistingKey eKey = new ExistingKey();
                        eKey.ID = item.ID;
                        eKey.Name = item.Name;
                        eKey.System = item.Application;
                        eKey.Description = item.Description;
                        _userAssignedKeys.Add(eKey);
                    }

                    //Get the available keys for the chosen system
                    List<Key> availKeys = _da.GetKeysByType(cboSystem.Text, DataAccess.Type.Access);
                    _availableKeys = new List<ExistingKey>();
                    foreach (Key item in availKeys)
                    {
                        ExistingKey aKey = new ExistingKey();
                        aKey.ID = item.ID;
                        aKey.Name = item.Name;
                        aKey.System = item.Application;
                        aKey.Description = item.Description;
                        _availableKeys.Add(aKey);
                    }

                    //remove the keys from the available list that the user already has access to
                    foreach (ExistingKey key in _userAssignedKeys)
                    {
                        _availableKeys.RemoveAll(p => p.Name == key.Name);
                    }
                    dgvAddRole.DataSource = _availableKeys;
                    dgvRemoveRole.DataSource = _userAssignedKeys;
                    dgvAddRole.Refresh();
                    dgvRemoveRole.Refresh();
                    dgvAddRole.Focus();
                }
            }
            else if (cboSystem.SelectedIndex == 0)
            {
                cboRole_SelectedIndexChanged(sender, e);
            }
        }

        /// <summary>
        /// Removes the chosen keys from the selected role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvRemoveRole.RowCount == 0)
            {
                MessageBox.Show("There are no access keys to remove", "No Access Keys", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //For each selected key, remove the key from the user and add it back to the available keys
            foreach (DataGridViewRow item in dgvRemoveRole.SelectedRows)
            {
                if (item.Cells["ID"].Value != null)
                {
                    ExistingKey key = new ExistingKey();
                    key.ID = int.Parse(item.Cells["ID"].Value.ToString());
                    key.System = item.Cells["System"].Value.ToString();
                    key.Description = item.Cells["Description"].Value.ToString();
                    key.Name = item.Cells["Name"].Value.ToString();
                    _da.RemoveKeyFromRole(key.ID, ((Role)cboRole.SelectedItem).RoleID);
                    _userAssignedKeys.RemoveAll(p => p.Name == key.Name);
                    _availableKeys.Add(key);
                }
            }
            dgvAddRole.DataSource = null;
            dgvAddRole.DataSource = _availableKeys.OrderBy(p => p.Name).ToList();
            dgvAddRole.Refresh();

            dgvRemoveRole.DataSource = null;
            dgvRemoveRole.DataSource = _userAssignedKeys.OrderBy(p => p.Name).ToList();
            dgvRemoveRole.Refresh();
        }

        /// <summary>
        /// Gets a list of assigned keys for the selected role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //for each selected key, remove the key from the available keys and add it to the role
            foreach (DataGridViewRow item in dgvAddRole.SelectedRows)
            {
                if (item.Cells["ID"].Value != null)
                {
                    ExistingKey key = new ExistingKey();
                    key.ID = int.Parse(item.Cells["ID"].Value.ToString());
                    key.System = item.Cells["System"].Value.ToString();
                    key.Description = item.Cells["Description"].Value.ToString();
                    key.Name = item.Cells["Name"].Value.ToString();
                    _da.AddKeysToRole(key.ID, ((Role)cboRole.SelectedItem).RoleID);
                    _userAssignedKeys.Add(key);
                    _availableKeys.RemoveAll(p => p.Name == key.Name);
                }
            }
            dgvAddRole.DataSource = null;
            dgvAddRole.DataSource = _availableKeys.OrderBy(p => p.Name).ToList();
            dgvAddRole.Refresh();

            dgvRemoveRole.DataSource = null;
            dgvRemoveRole.DataSource = _userAssignedKeys.OrderBy(p => p.Name).ToList();
            dgvRemoveRole.Refresh();
        }

        #endregion

        #region Modeling

        /// <summary>
        /// Loads the keys for the selected role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboRoleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvModelAfter.DataSource = null;
            if (cboRoleModel.SelectedIndex != 0 && cboRoleModel.SelectedIndex != -1)
            {
                if (cboRoleChange.Text.Trim() == cboRoleModel.Text.Trim() && cboRoleModel.Text.Trim() != string.Empty)
                {
                    //Check to see if the Role to change is the same as the role to model after
                    MessageBox.Show("You can't model a role after itself. Please choose another role", "Choose Another Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboRoleModel.SelectedIndex = -1;
                    return;
                }
                //Get a list of keys assigned to role, add them to a new ExistingKey so we don't have to display all the Key properties
                List<Key> roleKeys = _da.GetRoleAssignedKeys(((Role)cboRoleModel.SelectedItem).RoleID, "").ToList();
                List<ExistingKey> keys = new List<ExistingKey>();
                foreach (Key item in roleKeys)
                {
                    ExistingKey eKey = new ExistingKey();
                    eKey.ID = item.ID;
                    eKey.Name = item.Name;
                    eKey.System = item.Application;
                    eKey.Description = item.Description;
                    keys.Add(eKey);
                }
                dgvModelAfter.DataSource = keys;
                dgvModelAfter.Focus();
            }
        }

        /// <summary>
        /// Gets a list of assigned keys for the selected role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboRoleChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvModelChanged.DataSource = null;
            if (cboRoleChange.Text.Trim() == cboRoleModel.Text.Trim() && cboRoleChange.Text.Trim() != string.Empty)
            {
                //Check to see if the Role to change is the same as the role to model after
                MessageBox.Show("You can't model a role after itself. Please choose another role", "Choose Another Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRoleChange.SelectedIndex = -1;
            }
            if (cboRoleChange.SelectedIndex != 0 && cboRoleChange.SelectedIndex != -1)
            {
                //Get a list of keys assigned to role, add them to a new ExistingKey so we don't have to display all the Key properties
                List<Key> roleKeys = _da.GetRoleAssignedKeys(((Role)cboRoleChange.SelectedItem).RoleID, "").ToList();
                List<ExistingKey> keys = new List<ExistingKey>();
                foreach (Key item in roleKeys)
                {
                    ExistingKey eKey = new ExistingKey();
                    eKey.ID = item.ID;
                    eKey.Name = item.Name;
                    eKey.System = item.Application;
                    eKey.Description = item.Description;
                    keys.Add(eKey);
                }
                dgvModelChanged.DataSource = keys;
                dgvModelChanged.Focus();
            }
        }

        /// <summary>
        /// Removes the access key from the role and adds the new keys it is being modeled after
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbxModel_Click(object sender, EventArgs e)
        {
            List<Key> modelAfter = _da.GetRoleAssignedKeys(((Role)cboRoleModel.SelectedItem).RoleID, "").ToList();
            List<Key> currentKeys = _da.GetRoleAssignedKeys(((Role)cboRoleChange.SelectedItem).RoleID, "").ToList();
            foreach (Key item in currentKeys)
            {
                //Remove all the current access keys
                _da.RemoveKeyFromRole(item.ID, ((Role)cboRoleChange.SelectedItem).RoleID);
            }
            foreach (Key item in modelAfter)
            {
                //Add all the new access keys from the role being modeled after
                _da.AddKeysToRole(item.ID, ((Role)cboRoleChange.SelectedItem).RoleID);
            }
            dgvModelChanged.DataSource = null;
            dgvModelChanged.DataSource = modelAfter;
            MessageBox.Show("The role has been updated", "Model Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Role History

        private void cboSortRoleHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSortRoleHistory.Text.Trim() != string.Empty)
            {
                dgvRoleHistory.DataSource = null;
                switch (cboSortRoleHistory.SelectedIndex)
                {
                    case 1:
                        dgvRoleHistory.DataSource = _roleHistory.OrderBy(p => p.RoleID).ToList();
                        break;
                    case 2:
                        dgvRoleHistory.DataSource = _roleHistory.OrderBy(p => p.RoleName).ToList();
                        break;
                    case 3:
                        dgvRoleHistory.DataSource = _roleHistory.OrderBy(p => p.AddedBy).ToList();
                        break;
                    case 4:
                        dgvRoleHistory.DataSource = _roleHistory.OrderBy(p => p.StartDate).ToList();
                        break;
                    case 5:
                        dgvRoleHistory.DataSource = _roleHistory.OrderBy(p => p.RemovedBy).ToList();
                        break;
                    case 6:
                        dgvRoleHistory.DataSource = _roleHistory.OrderBy(p => p.EndDate).ToList();
                        break;
                }
                dgvRoleHistory.Focus();
            }
        }

        private void chkRoleHistory_CheckedChanged(object sender, EventArgs e)
        {
            dgvRoleHistory.DataSource = null;
            _roleHistory = null;
            _roleHistory = _da.GetRoleHistory(chkRoleHistory.Checked).ToList();
            dgvRoleHistory.DataSource = _roleHistory;
            dgvRoleHistory.Focus();
            cboSortRoleHistory_SelectedIndexChanged(sender, e);
        }

        private void btnPrintRoleHistory_Click(object sender, EventArgs e)
        {
            RoleHistoryReport roleReport = new RoleHistoryReport();
            switch (cboSortRoleHistory.SelectedIndex)
            {
                case 1:
                    roleReport.SetDataSource(_roleHistory.OrderBy(p => p.RoleID));
                    break;
                case 2:
                    roleReport.SetDataSource(_roleHistory.OrderBy(p => p.RoleName));
                    break;
                case 3:
                    roleReport.SetDataSource(_roleHistory.OrderBy(p => p.AddedBy));
                    break;
                case 4:
                    roleReport.SetDataSource(_roleHistory.OrderBy(p => p.StartDate));
                    break;
                case 5:
                    roleReport.SetDataSource(_roleHistory.OrderBy(p => p.RemovedBy));
                    break;
                case 6:
                    roleReport.SetDataSource(_roleHistory.OrderBy(p => p.EndDate));
                    break;
                default:
                    roleReport.SetDataSource(_roleHistory);
                    break;
            }
            string location = @"T:\RoleHistoryReport " + DateTime.Now.ToString("MM_dd_yyyy hh_mm_ss tt") + ".pdf";
            roleReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, location);
            if (MessageBox.Show("The report is located at " + location + ".\r\n\r\nDo you want to open the report?", "Open Report", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Process.Start(location);
            }
        }

        #endregion

        #region Role Key Access

        private void cboRolesKeyHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvRoleKeyAccess.DataSource = null;
            cboSortRoleKey.SelectedIndex = -1;
            cboSortRoleKey.Enabled = false;
            if (cboRolesKeyHistory.SelectedIndex != -1 && cboRolesKeyHistory.SelectedIndex != 0)
            {
                _roleHistoryKeys = _da.GetKeyAssignedToRoles(((Role)cboRolesKeyHistory.SelectedItem).RoleID, chkHistory.Checked).ToList();
                if (_roleHistoryKeys.Count > 0)
                {
                    dgvRoleKeyAccess.DataSource = _roleHistoryKeys;
                    dgvRoleKeyAccess.Columns[0].Visible = false;
                    dgvRoleKeyAccess.Focus();
                    cboSortRoleKey.Enabled = true;
                }
            }
        }

        private void cboRoleKeySortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRolesKeyHistory.Text.Trim() != string.Empty)
            {
                if (!string.IsNullOrEmpty(cboSortRoleKey.Text.Trim()))
                {
                    dgvRoleKeyAccess.DataSource = null;
                    switch (cboSortRoleKey.SelectedIndex)
                    {
                        case 1:
                            dgvRoleKeyAccess.DataSource = _roleHistoryKeys.OrderBy(p => p.ID).ToList();
                            break;
                        case 2:
                            dgvRoleKeyAccess.DataSource = _roleHistoryKeys.OrderBy(p => p.Name).ToList();
                            break;
                        case 3:
                            dgvRoleKeyAccess.DataSource = _roleHistoryKeys.OrderBy(p => p.Application).ToList();
                            break;
                        case 4:
                            dgvRoleKeyAccess.DataSource = _roleHistoryKeys.OrderBy(p => p.Description).ToList();
                            break;
                        case 5:
                            dgvRoleKeyAccess.DataSource = _roleHistoryKeys.OrderBy(p => p.AddedBy).ToList();
                            break;
                        case 6:
                            dgvRoleKeyAccess.DataSource = _roleHistoryKeys.OrderBy(p => p.StartDate).ToList();
                            break;
                        case 7:
                            dgvRoleKeyAccess.DataSource = _roleHistoryKeys.OrderBy(p => p.RemovedBy).ToList();
                            break;
                        case 8:
                            dgvRoleKeyAccess.DataSource = _roleHistoryKeys.OrderBy(p => p.EndDate).ToList();
                            break;
                    }
                    dgvRoleKeyAccess.Focus();
                }
            }
        }

        private void chkHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (cboRolesKeyHistory.Text.Trim() != string.Empty)
            {
                _roleHistoryKeys = null;
                cboSortRoleKey.SelectedIndex = -1;
                _roleHistoryKeys = _da.GetKeyAssignedToRoles(((Role)cboRolesKeyHistory.SelectedItem).RoleID, chkHistory.Checked).ToList();
                dgvRoleKeyAccess.DataSource = null;
                dgvRoleKeyAccess.DataSource = _roleHistoryKeys;
                dgvRoleKeyAccess.Focus();
            }
        }

        private void btnPrintRoleKey_Click(object sender, EventArgs e)
        {
            RoleKeyHistoryReport roleReport = new RoleKeyHistoryReport();
            switch (cboSortRoleKey.SelectedIndex)
            {
                case 1:
                    roleReport.SetDataSource(_roleHistoryKeys.OrderBy(p => p.ID));
                    break;
                case 2:
                    roleReport.SetDataSource(_roleHistoryKeys.OrderBy(p => p.Name));
                    break;
                case 3:
                    roleReport.SetDataSource(_roleHistoryKeys.OrderBy(p => p.Application));
                    break;
                case 4:
                    roleReport.SetDataSource(_roleHistoryKeys.OrderBy(p => p.Description));
                    break;
                case 5:
                    roleReport.SetDataSource(_roleHistoryKeys.OrderBy(p => p.AddedBy));
                    break;
                case 6:
                    roleReport.SetDataSource(_roleHistoryKeys.OrderBy(p => p.StartDate));
                    break;
                case 7:
                    roleReport.SetDataSource(_roleHistoryKeys.OrderBy(p => p.RemovedBy));
                    break;
                case 8:
                    roleReport.SetDataSource(_roleHistoryKeys.OrderBy(p => p.EndDate));
                    break;
                default:
                    roleReport.SetDataSource(_roleHistoryKeys);
                    break;
            }
            string location = @"T:\RoleKeyHistoryReport " + DateTime.Now.ToString("MM_dd_yyyy hh_mm_ss tt") + ".pdf";
            roleReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, location);
            if (MessageBox.Show("The report is located at " + location + ".\r\n\r\nDo you want to open the report?", "Open Report", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Process.Start(location);
            }
        }

        #endregion

        #region Key History

        private void btnPrintKeyHistory_Click(object sender, EventArgs e)
        {
            KeyHistoryReport keyReport = new KeyHistoryReport();
            switch (cboSortKey.SelectedIndex)
            {
                case 1:
                    keyReport.SetDataSource(_historyKeys.OrderBy(p => p.ID));
                    break;
                case 2:
                    keyReport.SetDataSource(_historyKeys.OrderBy(p => p.Name));
                    break;
                case 3:
                    keyReport.SetDataSource(_historyKeys.OrderBy(p => p.Application));
                    break;
                case 4:
                    keyReport.SetDataSource(_historyKeys.OrderBy(p => p.Description));
                    break;
                case 5:
                    keyReport.SetDataSource(_historyKeys.OrderBy(p => p.AddedBy));
                    break;
                case 6:
                    keyReport.SetDataSource(_historyKeys.OrderBy(p => p.StartDate));
                    break;
                case 7:
                    keyReport.SetDataSource(_historyKeys.OrderBy(p => p.RemovedBy));
                    break;
                case 8:
                    keyReport.SetDataSource(_historyKeys.OrderBy(p => p.EndDate));
                    break;
                default:
                    keyReport.SetDataSource(_historyKeys);
                    break;
            }
            string location = @"T:\KeyHistoryReport " + DateTime.Now.ToString("MM_dd_yyyy hh_mm_ss tt") + ".pdf";
            keyReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, location);
            if (MessageBox.Show("The report is located at " + location + ".\r\n\r\nDo you want to open the report?", "Open Report", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Process.Start(location);
            }
        }

        private void chkKeyHistory_CheckedChanged(object sender, EventArgs e)
        {
            dgvKeyHistory.DataSource = null;
            _historyKeys = null;
            _historyKeys = _da.GetKeyHistory(chkKeyHistory.Checked);
            dgvKeyHistory.DataSource = _historyKeys;
            dgvKeyHistory.Refresh();
            dgvKeyHistory.Focus();
            cboKeySort_SelectedIndexChanged(sender, e);
        }

        private void cboKeySort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboSortKey.Text.Trim()))
            {
                dgvKeyHistory.DataSource = null;
                switch (cboSortKey.SelectedIndex)
                {
                    case 1:
                        dgvKeyHistory.DataSource = _historyKeys.OrderBy(p => p.ID).ToList();
                        break;
                    case 2:
                        dgvKeyHistory.DataSource = _historyKeys.OrderBy(p => p.Name).ToList();
                        break;
                    case 3:
                        dgvKeyHistory.DataSource = _historyKeys.OrderBy(p => p.Application).ToList();
                        break;
                    case 4:
                        dgvKeyHistory.DataSource = _historyKeys.OrderBy(p => p.Description).ToList();
                        break;
                    case 5:
                        dgvKeyHistory.DataSource = _historyKeys.OrderBy(p => p.AddedBy).ToList();
                        break;
                    case 6:
                        dgvKeyHistory.DataSource = _historyKeys.OrderBy(p => p.StartDate).ToList();
                        break;
                    case 7:
                        dgvKeyHistory.DataSource = _historyKeys.OrderBy(p => p.RemovedBy).ToList();
                        break;
                    case 8:
                        dgvKeyHistory.DataSource = _historyKeys.OrderBy(p => p.EndDate).ToList();
                        break;
                }
                dgvKeyHistory.Focus();
            }
        }

        #endregion

        #region User History

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"T:\";
            dialog.Filter = "xml files (*.xml)|*.xml";
            string filePath = string.Empty;
            _userHistory = new List<UserHistory>();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filePath = dialog.FileName;
                lblFileOpened.Text = filePath;
                if (!filePath.Substring(filePath.Length - 3).ToUpper().Contains("XML"))
                {
                    MessageBox.Show("Please choose a valid XML document", "Invalid File Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    StringReader reader;
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string xdoc = sr.ReadToEnd().Replace("<?xml version='1.0' encoding='UTF-8'?>\n<?xml version='1.0' encoding='UTF-8'?>", "<?xml version='1.0' encoding='UTF-8'?>");
                        reader = new StringReader(xdoc);
                    }
                    XPathDocument doc = new XPathDocument(reader);
                    XPathNavigator nav = doc.CreateNavigator();

                    XPathExpression expr;
                    expr = nav.Compile("/results/result");
                    XPathNodeIterator iterator = nav.Select(expr);
                    while (iterator.MoveNext())
                    {
                        try
                        {
                            //Get the list of Event ID's from the database
                            List<int> eventIDs = _da.GetSplunkEventID();
                            //Check to see if the XML document has an event code
                            XPathNodeIterator eventID = iterator.Current.Select("field[@k='EventCode']");
                            int splunkID = eventID.MoveNext() == true ? int.Parse(eventID.Current.Value) : 0;
                            if (splunkID != 0) { if (!eventIDs.Contains(splunkID)) { continue; } }
                            else { continue; }
                            //If the event code in the XML matches one in the database, load the data into the grid view
                            UserHistory userH = new UserHistory();
                            userH.EventCode = splunkID;
                            XPathNodeIterator accountAffected = iterator.Current.Select("field[@k='CN']");
                            userH.AccountAffected = accountAffected.MoveNext() == true ? accountAffected.Current.Value : "";
                            XPathNodeIterator actionTake = iterator.Current.Select("field[@k='Message']");
                            userH.ActionTaken = actionTake.MoveNext() == true ? actionTake.Current.Value.Substring(0, actionTake.Current.Value.IndexOf("\r\n")) : "";
                            XPathNodeIterator groupName = iterator.Current.Select("field[@k='Group_Name']");
                            userH.GroupName = groupName.MoveNext() == true ? groupName.Current.Value : "";
                            XPathNodeIterator changedBy = iterator.Current.Select("field[@k='Account_Name']");
                            userH.ChangedBy = changedBy.MoveNext() == true ? changedBy.Current.Value.Substring(0, changedBy.Current.Value.ToUpper().IndexOf("CN")) : "";
                            XPathNodeIterator dateChanged = iterator.Current.Select("field[@k='TimeGenerated']");
                            string date = dateChanged.MoveNext() == true ? dateChanged.Current.Value : "";
                            string month = date.Substring(4, 2);
                            string day = date.Substring(6, 2);
                            string year = date.Substring(0, 4);
                            string hour = (int.Parse(date.Substring(8, 2)) - 7).ToString();
                            string min = date.Substring(10, 2);
                            string sec = date.Substring(12, 2);
                            userH.DateTimeChanged = string.Format("{0}/{1}/{2} {3}:{4}:{5}", month, day, year, hour, min, sec);
                            _userHistory.Add(userH);
                        }
                        catch (Exception ex)
                        {
                            LogRun.AddNotification("There was a problem parsing the XML document.", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                            continue;
                        }
                    }
                }
                dgvUserHistory.DataSource = _userHistory;
                dgvUserHistory.Focus();
            }
        }

        private void btnUserHistoryPrint_Click(object sender, EventArgs e)
        {
            UserHistoryReport userReport = new UserHistoryReport();
            userReport.SetDataSource(_userHistory);
            string location = @"T:\UserHistoryReport " + DateTime.Now.ToString("MM_dd_yyyy hh_mm_ss tt") + ".pdf";
            userReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, location);
            if (MessageBox.Show("The report is located at " + location + ".\r\n\r\nDo you want to open the report?", "Open Report", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Process.Start(location);
            }
        }

        #endregion

    }//class

    /// <summary>
    /// Compares the UserKey names in a List<RoleKey>
    /// </summary>
    class Comparer : IEqualityComparer<RoleKey>
    {
        public static readonly Comparer Instance = new Comparer();

        public bool Equals(RoleKey x, RoleKey y)
        {
            return x.UserKeyID.Equals(y.UserKeyID);
        }

        public int GetHashCode(RoleKey obj)
        {
            return obj.UserKeyID.GetHashCode();
        }
    }//Comparer Class]

}//namespace
