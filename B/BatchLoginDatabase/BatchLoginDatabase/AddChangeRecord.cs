using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace BatchLoginDatabase
{
    public partial class AddChangeRecord : Form
    {
        private List<LoginTypes> Types { get; set; }
        private bool IsNewId { get; set; }
        private BatchPasswordData UserIdData { get; set; }
        private bool IsValid { get; set; }
        public AddChangeRecord(BatchPasswordData userIdData = null)
        {
            InitializeComponent();
            IsNewId = userIdData == null;
            SetDataSources();
            UserIdData = userIdData;
            if (!IsNewId)
                LoadExistingData();
        }

        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "GetRelatedScriptsForLoginId")]
        private void LoadExistingData()
        {
            UserId.Text = UserIdData.UserName;
            UserId.Enabled = false;
            Delete.Enabled = true;
            Notes.Text = UserIdData.Notes;
            LoginTypes type = Types.Where(p => p.LoginTypeId == UserIdData.LoginTypeId).SingleOrDefault();
            AllowedLogins.Value = type.MaxInUse;
            LoginTypesCombo.Text = type.LoginType;
        }

        private void SetDataSources()
        {
            Types = LoginTypes.GetAllLoginTypes();
            LoginTypesCombo.DataSource = Types.Select(p => p.LoginType).ToList();
            LoginTypesCombo.SelectedIndex = -1;
        }

        private void LoginTypesCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AllowedLogins.Value = Types.Where(p => p.LoginType == LoginTypesCombo.SelectedItem.ToString()).Select(p => p.MaxInUse).SingleOrDefault();
        }

        private bool HasErrors()
        {
            List<string> errors = new List<string>();

            if (LoginTypesCombo.Text.IsNullOrEmpty())
                errors.Add("You must select a login type.");
            if (Password.Text != ConfirmPassword.Text)
                errors.Add("The password you entered does not match the confirm password or vice versa.");

            if (errors.Any())
            {
                string message = string.Join("\n\r", errors.Select(p => p));
                Dialog.Error.Ok(message);
                return true;
            }

            return false;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (HasErrors())
                return;



            if (IsNewId && IsValid)
                AddId();
            else
                if (IsValid)
                    UpdateId();

        }

        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spAddUserIdAndPassword")]
        private void AddId()
        {
            SqlParameter[] parameters = GetAddSqlParameters();
            DataAccessHelper.Execute("spAddUserIdAndPassword", DataAccessHelper.Database.BatchProcessing, parameters);
        }

        private SqlParameter[] GetAddSqlParameters()
        {
            return new SqlParameter[] 
            {
                new SqlParameter("UserId", UserId.Text),
                new SqlParameter("Password", Password.Text),
                new SqlParameter("Notes", Notes.Text),
                new SqlParameter("LoginTypeId", Types.Where(p => p.LoginType == LoginTypesCombo.Text).Select(p => p.LoginTypeId).SingleOrDefault()),
                new SqlParameter("Requester", Environment.UserName),
            };
        }

        private SqlParameter[] GetUpdateSqlParameters()
        {
            return new SqlParameter[] 
            {
                new SqlParameter("UserId", UserIdData.LoginId),
                new SqlParameter("UserName", UserId.Text),
                new SqlParameter("Password", Password.Text),
                new SqlParameter("Notes", Notes.Text),
                new SqlParameter("LoginTypeId", Types.Where(p => p.LoginType == LoginTypesCombo.Text).Select(p => p.LoginTypeId).SingleOrDefault()),
                new SqlParameter("Requester", Environment.UserName),
            };
        }

        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spUpdateUserIdsAndPasswords")]
        private void UpdateId()
        {
            SqlParameter[] parameters = GetUpdateSqlParameters();
            DataAccessHelper.Execute("spUpdateUserIdsAndPasswords", DataAccessHelper.Database.BatchProcessing, parameters);
        }

        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spDeleteUserIdsAndPasswords")]
        private void Delete_Click(object sender, EventArgs e)
        {
            if (Dialog.Warning.YesNo("Are you sure you want to delete this id?"))
            {
                DataAccessHelper.Execute("spDeleteUserIdsAndPasswords", DataAccessHelper.Database.BatchProcessing, new SqlParameter("LoginId", UserIdData.LoginId), new SqlParameter("Requestor", Environment.UserName));
                DialogResult = DialogResult.OK;
            }
        }

        private void ShowPasswords_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPasswords.Checked)
            {
                ShowPasswords.Text = "Hide Passwords";
                Password.PasswordChar = '\0';
                ConfirmPassword.PasswordChar = '\0';
            }
            else
            {
                ShowPasswords.Text = "Show Passwords";
                Password.PasswordChar = '*';
                ConfirmPassword.PasswordChar = '*';
            }
        }

        private void Password_Enter(object sender, EventArgs e)
        {
            Password.PasswordChar = '*';
        }

        private void ConfirmPassword_Enter(object sender, EventArgs e)
        {
            ConfirmPassword.PasswordChar = '*';
        }

        private void Save_OnValidate(object sender, Uheaa.Common.WinForms.ValidationEventArgs e)
        {
            if (e.FormIsValid && !LoginTypesCombo.Text.IsNullOrEmpty() && Password.Text == ConfirmPassword.Text)
            {
                IsValid = true;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
