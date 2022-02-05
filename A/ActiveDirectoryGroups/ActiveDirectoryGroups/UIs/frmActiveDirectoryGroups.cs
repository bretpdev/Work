using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ActiveDirectoryGroups
{
    partial class frmActiveDirectoryGroups : Form
    {
        List<SqlUser> UserList { get; set; }
        List<string> DevGroups = new List<string>() { "CornerStoneClearance", "CornerStoneUsers", "Developers", "OcheUsers", "SystemAnalysts", "UheaaUsers" };
        List<string> Groups { get; set; }
        private DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public frmActiveDirectoryGroups(ProcessLogRun logRun)
        {
            InitializeComponent();

            DA = new DataAccess(logRun);
            Groups = new List<string>(DA.GetRoles().OrderBy(p => p.RoleName).Select(p => p.RoleName));
            for (int i = 0; i < DevGroups.Count; i++)
                Groups.Insert(i, DevGroups[i]);
        }

        /// <summary>
        /// Generates a list of all the users that are in the pre-defined active directory groups.w
        /// </summary>
        private void GetUserList()
        {
            UserList = new List<SqlUser>();
            List<string> users = new List<string>();
            using (DirectoryEntry entry = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local"))
            {
                AddUsers(users);

                //Set the progress bar max
                //progress.Maximum = users.Count;
                //progress.Value = 0;

                FilterUsersByADGroup(users, entry);
            }

        }

        /// <summary>
        /// Add users to Users list
        /// </summary>
        private void AddUsers(List<string> users)
        {
            Parallel.ForEach(Groups, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, group =>
            {
                if (group != "")
                {
                    DirectorySearcher search = new DirectorySearcher();
                    search.Filter = string.Format(("(&(ObjectClass=group)(CN={0}))"), group);
                    SearchResult res = search.FindOne();
                    if (res != null)
                    {
                        using (DirectoryEntry groupEntry = new DirectoryEntry(res.Path))
                        {
                            //PropertyValueCollection pcoll = groupEntry.Properties["member"];
                            foreach (object item in groupEntry.Properties["member"])
                            {
                                users.Add(item.ToString());
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// User DirectorySearcher to Filter users
        /// </summary>
        private void FilterUsersByADGroup(List<string> users, DirectoryEntry entry)
        {
            Parallel.ForEach(users, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, item =>
            {
                DirectorySearcher searcher = new DirectorySearcher();
                searcher.SearchRoot = entry;
                searcher.Filter = string.Format("CN={0}", item.Substring(3, item.IndexOf(",") - 3));
                SearchResult result = searcher.FindOne();
                if (result != null)
                {
                    try
                    {
                        SqlUser user = new SqlUser()
                        {
                            WindowsUserName = result.Properties["SAMAccountName"][0].ToString(),
                            FirstName = result.Properties["givenname"][0].ToString(),
                            LastName = result.Properties["sn"][0].ToString(),
                            LegalName = result.Properties["cn"][0].ToString()
                        };
                        if (!string.IsNullOrEmpty(result.Properties["mail"][0].ToString()) && !UserList.Exists(p => p.LegalName == user.LegalName))
                        {
                            UserList.Add(user);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                //IncrementProgressBar();
            });
        }

        private void IncrementProgressBar()
        {
            progress.Increment(1);
            int percent = (int)(((double)progress.Value / (double)progress.Maximum) * 100);
            Application.DoEvents();
            progress.Refresh();
            progress.CreateGraphics().DrawString(percent.ToString() + "%",
                new Font("Arial", (float)9, FontStyle.Regular),
                Brushes.Black,
                new PointF(progress.Width / 2 - 10, progress.Height / 2 - 7));
        }

        /// <summary>
        /// Searches Active Directory for list of users/groups for the given input
        /// </summary>
        private void cboStaffMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblName.Text == "User Name:")
            {
                lbxGroups.Items.Clear();
                SqlUser user = UserList.SingleOrDefault(p => p.LegalName == cboStaffMember.Text);
                if (user != null && !string.IsNullOrEmpty(user.WindowsUserName))
                {
                    foreach (string item in GetActiveDirectoryGroups(user.WindowsUserName))
                    {
                        lbxGroups.Items.Add(item);
                    }
                    lblTitle.Text = string.Format("{0}{1} Active Directory Groups", cboStaffMember.Text, cboStaffMember.Text.EndsWith("s") ? "'" : "'s");
                }
            }
            else if (lblName.Text == "Group Name:")
            {
                lbxGroups.Items.Clear();
                if (!string.IsNullOrEmpty(cboStaffMember.Text))
                {
                    using (DirectoryEntry entry = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local"))
                    {
                        DirectorySearcher search = new DirectorySearcher();
                        search.Filter = string.Format(("(&(ObjectClass=group)(CN={0}))"), cboStaffMember.Text);
                        SearchResult res = search.FindOne();
                        if (res != null)
                        {
                            using (DirectoryEntry groupEntry = new DirectoryEntry(res.Path))
                            {
                                PropertyCollection pcoll = groupEntry.Properties;
                                foreach (string item in groupEntry.Properties["member"])
                                {
                                    if (item.Contains("CN=ROLE"))
                                    {
                                        string name = GetUserFromGroup(item);
                                        if (name.IsPopulated())
                                            lbxGroups.Items.Add(name);
                                    }
                                    else if (!item.Contains("_") && !item.Contains("expired"))
                                    {
                                        lbxGroups.Items.Add(item.Substring(3, item.IndexOf(",") - 3));
                                    }
                                }
                            }
                        }
                        lblTitle.Text = string.Format("{0} Group Members", cboStaffMember.Text);
                    }
                }
            }
        }

        private string GetUserFromGroup(string itemPath)
        {
            string path = string.Format("LDAP://{0}", itemPath);
            using (DirectoryEntry entry = new DirectoryEntry(path))
            {
                PropertyCollection pcoll = entry.Properties;
                foreach (string item in entry.Properties["member"])
                {
                    if (item.Contains("CN=ROLE"))
                        GetUserFromGroup(item);
                    return item.Substring(3, item.IndexOf(",") - 3);
                }
            }
            return "";
        }

        //Usage: GetAllUsernamesInGroup("CornerStoneUsers")
        public static IEnumerable<string> GetAllUsernamesInGroup(string group)
        {
            List<string> userNames = new List<string>();
            var domainContext = new System.DirectoryServices.AccountManagement.PrincipalContext(ContextType.Domain);
            var groupPrincipal = GroupPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, group);
            foreach (var sub in groupPrincipal.GetGroups())
                userNames.AddRange(GetAllUsernamesInGroup(sub.Name));
            var users = groupPrincipal.Members.Where(o => o.StructuralObjectClass == "user").Select(o => o.SamAccountName);
            userNames.AddRange(users);
            return userNames.Distinct();
        }

        ///<summary>
        ///Finds all the Active Directory groups a user belongs to, non-recursively.
        ///</summary>
        ///<param name="windowsUserName">The username of the user to search.</param>
        private List<string> GetActiveDirectoryGroups(string windowsUserName)
        {
            List<string> groupMemberships = new List<string>();
            using (DirectoryEntry searchEntry = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local"))
            {
                DirectorySearcher searcher = new DirectorySearcher();
                searcher.SearchRoot = searchEntry;
                searcher.Filter = string.Format("SAMAccountName={0}", windowsUserName);
                SearchResult result = searcher.FindOne();
                if (result != null)
                {
                    ResultPropertyCollection attributes = result.Properties;
                    foreach (string prop in attributes["memberOf"])
                    {
                        int equalsIndex = prop.IndexOf("=", 1);
                        int commaIndex = prop.IndexOf(",", 1);
                        if (equalsIndex >= 0)
                        {
                            groupMemberships.Add(prop.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                        }
                    }
                }
            }
            return groupMemberships;
        }

        /// <summary>
        /// Sets up the combobox with the user that are returned from GetUserList()
        /// </summary>
        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Loading Users";
            lblTitle.Visible = true;
            if (UserList == null)
            {
                //progress.Visible = true;
                GetUserList();
                UserList.Insert(0, new SqlUser());
                //progress.Visible = false;
            }
            if (UserList.Count > 0)
            {
                lblName.Text = "User Name:";
                cboStaffMember.Enabled = true;
                cboStaffMember.DataSource = UserList.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
                cboStaffMember.DisplayMember = "LegalName";
                cboStaffMember.ValueMember = "ID";
                cboStaffMember.Focus();
            }
            lblTitle.Text = "";
        }

        /// <summary>
        /// Adds the list of Active Directory Groups in the _groups[]
        /// </summary>
        private void activeDirectoryGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblTitle.Text = string.Empty;
            lblName.Text = "Group Name:";
            cboStaffMember.Enabled = true;
            Groups.Insert(0, "");
            cboStaffMember.DataSource = Groups;
            cboStaffMember.Focus();
        }

        /// <summary>
        /// Opens the frmCSYSData form, resets the _userList to null after form is closed.
        /// </summary>
        private void tsmUpdateCsys_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmCSYSData csys = new frmCSYSData(DA);
            csys.ShowDialog();
            this.Show();
        }
    }
}