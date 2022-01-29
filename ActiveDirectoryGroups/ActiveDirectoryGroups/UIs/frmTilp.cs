using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace ActiveDirectoryGroups
{
    public partial class frmTilp : Form
    {
        public DataAccess DA { get; set; }
        public List<AuthList> AuthList { get; set; }

        public frmTilp(string userId, DataAccess da)
        {
            InitializeComponent();
            DA = da;

            LoadAuthList();
            LoadUser(userId);
        }

        private void LoadUser(string userId)
        {
            TilpUser user = DA.GetTilpUser(userId);
            UserIdtxt.Text = userId;
            if (user != null)
            {
                AuthListCbo.Text = AuthList.Where(p => p.LevelDesc == user.LevelDesc).FirstOrDefault().AuthLevel.ToString();
                ValidChk.Checked = user.Valid;
                OK.Text = "Update";
            }
        }

        private void LoadAuthList()
        {
            AuthList = DA.GetAuthList();
            AuthList.Insert(0, new AuthList() { AuthLevel = 0, LevelDesc = "" });
            AuthListCbo.DataSource = AuthList;
            AuthListCbo.DisplayMember = "AuthLevel";
            AuthListCbo.ValueMember = "LevelDesc";
        }

        private void AuthListCbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AuthListCbo.SelectedIndex > -1)
                Description.Text = ((AuthList)AuthListCbo.SelectedItem).LevelDesc;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                TilpUser user = new TilpUser()
                {
                    UserId = UserIdtxt.Text,
                    AuthLevel = ((AuthList)AuthListCbo.SelectedItem).AuthLevel,
                    Valid = ValidChk.Checked
                };

                if (OK.Text.ToLower() == "update")
                    DA.UpdateTilpUser(user);
                else
                    DA.InsertTilpUser(user);
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateData()
        {
            string message = $"Please provide the following missing data\r\n\r\n";
            bool isValid = true;

            if (UserIdtxt.Text.IsNullOrEmpty())
            {
                message += "User Id\r\n";
                isValid = false;
            }
            if (AuthListCbo.SelectedIndex < 1)
            {
                message += "Authorization level";
                isValid = false;
            }

            if (!isValid)
                Dialog.Warning.Ok(message, "Missing Data");

            return isValid;
        }
    }
}