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
    public partial class SackerScriptInfo : Form
    {
        private List<string> SackerScripts { get; set; }
        private List<UserIdScriptMapping> UserIdsFromDb { get; set; }
        


        public SackerScriptInfo()
        {
            InitializeComponent();
            SackerScriptIds.DataSource = DataAccessHelper.ExecuteList<string>("GetAllScriptIds", DataAccessHelper.Database.Bsys);
            SackerScriptIds.SelectedIndex = -1;
            
            
        }

        private void PopulateCheckBoxList(string scriptId)
        {
            UserIdsFromDb = UserIdScriptMapping.GetAllIds(scriptId);
            foreach (UserIdScriptMapping map in UserIdsFromDb)
            {
                UserIds.Items.Add(map.UserName);
                UserIds.SetItemChecked(UserIds.Items.Count - 1, map.IsRelated);
            }
        }

        private void SackerScriptIds_Leave(object sender, EventArgs e)
        {
            if(!SackerScriptIds.Text.IsNullOrEmpty())
                PopulateCheckBoxList(SackerScriptIds.Text);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            DataTable checkedIds = new DataTable();
            checkedIds.Columns.Add("UserId");

            foreach (var item in UserIds.CheckedItems)
                checkedIds.Rows.Add(item.ToString());

            DataAccessHelper.Execute("InsertLoginScriptTracking", DataAccessHelper.Database.BatchProcessing, SackerScriptIds.Text.ToSqlParameter("SackerScriptId"),
                SqlParams.Single("MaxLogins", AllowedLogins.Value), SqlParams.Single("Relateduserids", checkedIds));
            DialogResult = DialogResult.OK;
            
        }
    }
}
