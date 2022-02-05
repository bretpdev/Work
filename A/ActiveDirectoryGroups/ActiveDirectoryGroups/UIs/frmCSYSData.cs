using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ActiveDirectoryGroups
{
    partial class frmCSYSData : Form
    {
        private ViewMode _viewMode { get; set; } = ViewMode.none;
        private DataAccess DA { get; set; }
        private List<BusinessUnit> Units { get; set; }
        private List<Role> Roles { get; set; }
        List<SystemUsers> Users { get; set; }
        public Func<SystemUsers, object> Predicate { get; set; }
        public bool IsDesdending { get; set; }
        public int SortingNumber { get; set; }

        private enum ViewMode
        {
            none,
            active,
            inactive,
            all
        }

        public frmCSYSData(DataAccess da)
        {
            InitializeComponent();
            lblTestMode.Text = DataAccessHelper.CurrentMode.ToString();
            DA = da;
            Predicate = p => p.WindowsUserName;
            IsDesdending = true;
        }

        /// <summary>
        /// Sends Active Users status to load grid view, sets the ViewMode to Active
        /// </summary>
        private void activeUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewMode = ViewMode.none;
            ChangeMode(sender, e);
        }

        /// <summary>
        /// Sends Inactive Users status to load grid view, sets the ViewMode to Inactive
        /// </summary>
        private void inactiveUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewMode = ViewMode.active;
            ChangeMode(sender, e);
        }

        /// <summary>
        /// Sends empty string to LoadDataGridView which will select all statuses, sets the ViewMode to all
        /// </summary>
        private void allUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewMode = ViewMode.inactive;
            ChangeMode(sender, e);
        }

        /// <summary>
        /// Does what it says, loads the grid view with data
        /// </summary>
        private void LoadDataGridView(string viewMode)
        {
            Users = LoadSystemUser(Units, Roles, viewMode).ToList();
            Users = Users.Where(p => p != null).ToList();

            dgvTable.DataSource = null;
            if (IsDesdending)
                dgvTable.DataSource = Users.OrderBy(Predicate).ToList();
            else
                dgvTable.DataSource = Users.OrderByDescending(Predicate).ToList();
        }

        /// <summary>
        /// Changes the List of BaseUser to a List of SystemUsers
        /// </summary>
        /// <param name="units">List of BusinessUnit</param>
        /// <param name="roles">List of Role</param>
        /// <param name="viewMode">The Status of Users</param>
        /// <returns>List of SystemUsers</returns>
        public List<SystemUsers> LoadSystemUser(List<BusinessUnit> units, List<Role> roles, string viewMode)
        {
            List<BaseUser> user = DA.GetTableData(viewMode);
            List<SystemUsers> users = new List<SystemUsers>();
            List<string> userBU = new List<string>();
            List<string> userRole = new List<string>();
            Parallel.ForEach(user, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, item =>
            {
                SystemUsers singleUser = new SystemUsers();
                singleUser.SqlUserID = item.SqlUserID.ToString();
                singleUser.WindowsUserName = item.WindowsUserName.Trim();
                singleUser.FirstName = item.FirstName.Trim();
                singleUser.MiddleInitial = item.MiddleInitial != null ? item.MiddleInitial.Trim() : "";
                singleUser.LastName = item.LastName.Trim();
                singleUser.EmailAddress = item.EMail.Trim();
                singleUser.Extension = item.Extension != null ? item.Extension.Trim() : "";
                singleUser.Extension2 = item.Extension2 != null ? item.Extension2.Trim() : "";
                singleUser.BusinessUnit = item.BusinessUnit;
                singleUser.Role = item.Role;
                singleUser.Status = item.Status.Trim();
                singleUser.Title = item.Title;
                singleUser.AesUserId = item.AesUserId;
                users.Add(singleUser);
                if (singleUser.BusinessUnit.IsNullOrEmpty())
                    userBU.Add(singleUser.WindowsUserName);
                if (singleUser.Role.IsNullOrEmpty())
                    userRole.Add(singleUser.WindowsUserName);
            });
#if !DEBUG
            //if (users.Any(p => p.BusinessUnit.IsNullOrEmpty()))
            //{
            //    string buMessage = "The Business Unit is missing for the following people\r\n";
            //    MessageBox.Show(buMessage + string.Join("\r\n", userBU.ToArray()), "Business Unit Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //if (users.Any(p => p.Role.IsNullOrEmpty()))
            //{
            //    string buMessage = "The Role is missing for the following people\r\n";
            //    MessageBox.Show(buMessage + string.Join("\r\n", userRole.ToArray()), "Role Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
#endif
            return users;
        }

        /// <summary>
        /// Finds the row that is selected and sets the cell to Inactive and updates the table
        /// </summary>
        private void btnInactivate_Click(object sender, EventArgs e)
        {
            if (dgvTable.DataSource != null)
            {
                int squid = 0;
                string userName = "";
                DataGridViewRow row = dgvTable.SelectedRows[0];
                if (row.Cells["Status"].Value.ToString() == "Active")
                {
                    //Get a list of Squid's to inactivate
                    squid = int.Parse(row.Cells["SqlUserId"].Value.ToString());
                    //Get the names of users being inactivated for display message
                    userName = row.Cells["FirstName"].Value.ToString() + " " + row.Cells["LastName"].Value.ToString();
                }
                if (squid > 0)
                {
                    string message = "Are you sure you want to Inactivate " + userName + "?";
                    if (MessageBox.Show(message, "Invalidate User?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        DA.SetUserInactive(squid);
                        MessageBox.Show("Inactivated Succesfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                        return;
                }
                else
                    MessageBox.Show("Please choose a valid user", "Invalid User", MessageBoxButtons.OK, MessageBoxIcon.Error);

                ResetDataSource(sender, e);
            }
        }

        /// <summary>
        /// Sends a dataset with the new user information to be added
        /// </summary>
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            //Get the business units and roles
            Units = DA.GetBusinessUnits();
            Roles = DA.GetRoles();
            Units.Insert(0, new BusinessUnit() { Name = string.Empty, ID = 0 });
            Roles.Insert(0, new Role() { RoleName = string.Empty, RoleID = 0 });

            ActiveDirectoryUser user = new ActiveDirectoryUser();
            frmAddUser addUser = new frmAddUser(user, Units, Roles, DA);
            if (addUser.ShowDialog() == DialogResult.OK)
            {
                if (DA.InsertUser(user))
                {
                    MessageBox.Show("Inserted Succesfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ResetDataSource(sender, e);
                }
            }
        }

        /// <summary>
        /// Sends a dataset with all the changes to be updated
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the business units and roles
            Units = DA.GetBusinessUnits();
            Roles = DA.GetRoles();
            Units.Insert(0, new BusinessUnit() { Name = string.Empty, ID = 0 });
            Roles.Insert(0, new Role() { RoleName = string.Empty, RoleID = 0 });

            if (dgvTable.SelectedRows.Count > 0)
            {
                //Create an ActiveDirectoryUser from the selected row
                DataGridViewRow row = dgvTable.SelectedRows[0];
                ActiveDirectoryUser user = new ActiveDirectoryUser();
                user.SqlUserID = int.Parse(row.Cells["SqlUserID"].Value.ToString()) != 0 ? int.Parse(row.Cells["SqlUserID"].Value.ToString()) : 0;
                user.WindowsUserName = row.Cells["WindowsUserName"].Value != null ? row.Cells["WindowsUserName"].Value.ToString() : string.Empty;
                user.FirstName = row.Cells["FirstName"].Value != null ? row.Cells["FirstName"].Value.ToString() : string.Empty;
                user.MiddleInitial = row.Cells["MiddleInitial"].Value != null ? row.Cells["MiddleInitial"].Value.ToString() : string.Empty;
                user.LastName = row.Cells["LastName"].Value != null ? row.Cells["LastName"].Value.ToString() : string.Empty;
                string email = row.Cells["EmailAddress"].Value != null ? row.Cells["EmailAddress"].Value.ToString() : string.Empty;
                user.EmailAddress = email != string.Empty ? email.Remove(email.IndexOf("@"), email.Length - email.IndexOf("@")) : string.Empty;
                user.Extension = row.Cells["Extension"].Value != null ? row.Cells["Extension"].Value.ToString() : string.Empty;
                user.Extension2 = row.Cells["Extension2"].Value != null ? row.Cells["Extension2"].Value.ToString() : string.Empty;
                string bu = row.Cells["BusinessUnit"].Value != null ? row.Cells["BusinessUnit"].Value.ToString() : string.Empty;
                user.BusinessUnit = Units.Where(p => p.Name == bu).First();
                string role = row.Cells["Role"].Value != null ? row.Cells["Role"].Value.ToString() : string.Empty;
                user.Role = Roles.Where(p => p.RoleName == role).First();
                user.Status = row.Cells["Status"].Value.ToString() == "Active" ? true : false;
                user.Title = row.Cells["Title"].Value != null ? row.Cells["Title"].Value.ToString() : string.Empty;
                user.AesUserID = row.Cells["AesUserId"].Value != null ? row.Cells["AesUserId"].Value.ToString() : string.Empty;

                frmAddUser addUser = new frmAddUser(user, Units, Roles, DA);
                if (addUser.ShowDialog() == DialogResult.OK)
                {
                    if (DA.UpdateData(user))
                    {
                        MessageBox.Show(user.FirstName + ' ' + user.LastName + " has been updated", "User Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetDataSource(sender, e);
                    }
                }
            }
            else
                MessageBox.Show("Please choose a user to update", "No Selection Made", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Clears the datasource then calls the event that resets the source according to the view mode
        /// </summary>
        private void ResetDataSource(object sender, EventArgs e)
        {
            //Set the datasource to null and call the event that resets the datasource
            dgvTable.DataSource = null;
            switch (_viewMode)
            {
                case ViewMode.active:
                    activeUsersToolStripMenuItem_Click(sender, e);
                    break;
                case ViewMode.inactive:
                    inactiveUsersToolStripMenuItem_Click(sender, e);
                    break;
                case ViewMode.all:
                    allUsersToolStripMenuItem_Click(sender, e);
                    break;
                case ViewMode.none:
                    break;
            }
        }

        private void SortDataSource()
        {
            dgvTable.DataSource = null;
            if (IsDesdending)
                dgvTable.DataSource = Users.OrderBy(Predicate).ToList();
            else
                dgvTable.DataSource = Users.OrderByDescending(Predicate).ToList();
        }

        /// <summary>
        /// Changes the view mode
        /// </summary>
        private void btnViewMode_ButtonClick(object sender, EventArgs e)
        {
            ChangeMode(sender, e);
        }

        /// <summary>
        /// Changes the mode according the users status (Active, Inactive)
        /// </summary>
        public void ChangeMode(object sender, EventArgs e)
        {
            Units = DA.GetBusinessUnits();
            Roles = DA.GetRoles();

            switch (_viewMode)
            {
                case ViewMode.none:
                    dgvTable.ClearSelection();
                    btnAddUser.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnInactivateUser.Enabled = true;
                    Search.Enabled = true;
                    lblSearch.Enabled = true;
                    lblViewMode.Text = "Active Users";
                    LoadDataGridView("Active");
                    _viewMode = ViewMode.active;
                    break;
                case ViewMode.active:
                    dgvTable.ClearSelection();
                    btnAddUser.Enabled = false;
                    btnUpdate.Enabled = true;
                    btnInactivateUser.Enabled = false;
                    Search.Enabled = true;
                    lblSearch.Enabled = true;
                    lblViewMode.Text = "Inactive Users";
                    LoadDataGridView("Inactive");
                    _viewMode = ViewMode.inactive;
                    break;
                case ViewMode.inactive:
                    dgvTable.ClearSelection();
                    btnAddUser.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnInactivateUser.Enabled = true;
                    Search.Enabled = true;
                    lblSearch.Enabled = true;
                    lblViewMode.Text = "All Users";
                    LoadDataGridView("");
                    _viewMode = ViewMode.all;
                    break;
                case ViewMode.all:
                    lblViewMode.Text = "";
                    dgvTable.DataSource = null;
                    _viewMode = ViewMode.none;
                    btnAddUser.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnInactivateUser.Enabled = false;
                    Search.Enabled = false;
                    lblSearch.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// Does an update for the row that is selected
        /// </summary>
        private void dgvTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnUpdate_Click(sender, e);
        }

        /// <summary>
        /// Searches every cell in the grid and returns the first row that finds the value
        /// </summary>
        private void Search_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvTable.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Trim().Contains(Search.Text.ToLower().Trim()))
                    {
                        dgvTable.Rows[dgvTable.Rows.IndexOf(row)].Selected = true;
                        dgvTable.FirstDisplayedScrollingRowIndex = dgvTable.Rows.IndexOf(row);
                        return;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lblTestMode.Visible == false)
                lblTestMode.Visible = true;
            else
                lblTestMode.Visible = false;
        }

        private void dgvTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == SortingNumber)
                IsDesdending = !IsDesdending;
            switch (e.ColumnIndex)
            {
                case 0:
                    Predicate = p => p.SqlUserID;
                    break;
                case 1:
                    Predicate = p => p.WindowsUserName;
                    break;
                case 2:
                    Predicate = p => p.FirstName;
                    break;
                case 3:
                    Predicate = p => p.MiddleInitial;
                    break;
                case 4:
                    Predicate = p => p.LastName;
                    break;
                case 5:
                    Predicate = p => p.EmailAddress;
                    break;
                case 6:
                    Predicate = p => p.Extension;
                    break;
                case 7:
                    Predicate = p => p.Extension2;
                    break;
                case 8:
                    Predicate = p => p.BusinessUnit;
                    break;
                case 9:
                    Predicate = p => p.Role;
                    break;
                case 10:
                    Predicate = p => p.Status;
                    break;
                case 11:
                    Predicate = p => p.Title;
                    break;
                case 12:
                    Predicate = p => p.AesUserId;
                    break;
            }
            SortDataSource();
            if (Search.Text.IsPopulated())
            {
                string text = Search.Text;
                Search.Text = "";
                Search.Text = text;
            }
            SortingNumber = e.ColumnIndex;
        }
    }
}