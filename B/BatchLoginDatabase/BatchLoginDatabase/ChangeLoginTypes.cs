using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace BatchLoginDatabase
{
    public partial class ChangeLoginTypes : Form
    {
        private List<LoginTypes> Types { get; set; }
        public ChangeLoginTypes()
        {
            InitializeComponent();
            SetDataSources();
        }

        private void SetDataSources()
        {
            Types = LoginTypes.GetAllLoginTypes();
            LoginTypesCombo.DataSource = Types.Select(p => p.LoginType).ToList();
            LoginTypesCombo.SelectedIndex = -1;
        }

        private void LoginTypesCombo_Leave(object sender, EventArgs e)
        {
            int? val = (int?)Types.Where(p => p.LoginType == LoginTypesCombo.Text).Select(p => (int?)p.MaxInUse).SingleOrDefault() ?? 1;
            AllowedLogins.Value = val.Value;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!LoginTypesCombo.Text.IsNullOrEmpty())
            {
                LoginTypes type = Types.Where(p => p.LoginType == LoginTypesCombo.Text).SingleOrDefault();
                if (type != null)
                    DataAccessHelper.Execute("UpdateLoginType", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("LoginTypeId", type.LoginTypeId), SqlParams.Single("LoginType", type.LoginType), SqlParams.Single("MaxInUse", AllowedLogins.Value));
                else
                    DataAccessHelper.Execute("AddLoginType", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("LoginType", LoginTypesCombo.Text), SqlParams.Single("MaxInUse", AllowedLogins.Value));

                SetDataSources();
            }
            else
                Dialog.Error.Ok("You must select a LoginType or type one in");
        }

        private void LoginTypesCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int? val = (int?)Types.Where(p => p.LoginType == LoginTypesCombo.Text).Select(p => (int?)p.MaxInUse).SingleOrDefault() ?? 1;
            AllowedLogins.Value = val.Value;
        }

        private void LoginTypesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? val = (int?)Types.Where(p => p.LoginType == LoginTypesCombo.Text).Select(p => (int?)p.MaxInUse).SingleOrDefault() ?? 1;
            AllowedLogins.Value = val.Value;
        }
    }
}
